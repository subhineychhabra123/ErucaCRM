using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using ErucaCRM.Utility.WebClasses;
using System.Xml;
using System.Data.OleDb;
using OfficeOpenXml;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using System.Threading;
using System.Globalization;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace ErucaCRM.Utility
{
    public static class CommonFunctions
    {
        public static void SetCookie(WebClasses.CurrentUser loggedInUser, string pipeSeperatedPermissions, bool rememberMe)
        {
            // Success, create non-persistent authentication cookie.
            FormsAuthentication.SetAuthCookie(loggedInUser.UserName, false);
            FormsAuthenticationTicket ticket1 =
               new FormsAuthenticationTicket(
                    1,                                   // version
                    loggedInUser.UserId + "|" + loggedInUser.CompanyId,   // get username  from the form
                    DateTime.Now,                        // issue time is now
                    DateTime.Now.AddMinutes(2080),
                    false,      // cookie is not persistent
                    loggedInUser.RoleName + "|" + pipeSeperatedPermissions// role assignment is stored
                // in userData
                    );


            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket1));
            HttpContext.Current.Response.Cookies.Add(cookie);
            SessionManagement.LoggedInUser = loggedInUser;

            HttpContext.Current.Response.Cookies.Add(new HttpCookie("userid", loggedInUser.UserId.Encrypt()));
            HttpCookie permissionCookie = HttpContext.Current.Response.Cookies["Permissions"];
            if (permissionCookie != null)
                permissionCookie = new HttpCookie("Permissions", "");
            permissionCookie = new HttpCookie("Permissions", pipeSeperatedPermissions);
            //permissionCookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(permissionCookie);
            HttpCookie rememberCookie = new HttpCookie("RememberMe");
            if (rememberMe)
            {
                rememberCookie.Values["userName"] = loggedInUser.UserName;
                rememberCookie.Values["password"] = loggedInUser.Password;
                rememberCookie.Values["emailId"] = loggedInUser.EmailId;
                rememberCookie.Expires = DateTime.Now.AddYears(1);
            }
            else
            {
                rememberCookie.Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.Current.Response.Cookies.Add(rememberCookie);
        }
        public static void RemoveCookies()
        {
            HttpCookie rememberCookie = new HttpCookie("RememberMe");
            rememberCookie.Expires = DateTime.Now.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(rememberCookie);
        }
        public static void UploadFile(HttpPostedFileBase file, string fileSavingPath, int imageWidth = 0, int imageHeight = 0)
        {

            //if (!Directory.Exists(imageSavingPath))
            //    Directory.CreateDirectory(imageSavingPath);
            if (System.IO.File.Exists(fileSavingPath))
            {
                System.IO.File.Delete(fileSavingPath);
            }
            file.SaveAs(fileSavingPath);
            if (imageWidth > 0 && imageHeight > 0)
            {
                var resizeSettings = new ImageResizer.ResizeSettings
                {
                    Scale = ImageResizer.ScaleMode.DownscaleOnly,
                    Width = imageWidth,
                    Height = imageHeight,
                    Mode = ImageResizer.FitMode.Crop
                };
                var b = ImageResizer.ImageBuilder.Current.Build(fileSavingPath, resizeSettings);
                b.Save(fileSavingPath);

            }


        }
        public static void UploadFile(HttpPostedFileBase file, string fileSavingPath, string filename, string filecontainer)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container =
                blobClient.GetContainerReference(filecontainer);

            container.CreateIfNotExists();

            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Container

                });

            CloudBlockBlob blob = container.GetBlockBlobReference(fileSavingPath);
         
            blob.UploadFromStream(file.InputStream);

        }
        //this Method use for WebService
        public static void UploadFile(Stream file, string fileSavingPath, string filename, string filecontainer)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container =
                blobClient.GetContainerReference(filecontainer);

            container.CreateIfNotExists();

            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Container

                });

            CloudBlockBlob blob = container.GetBlockBlobReference(fileSavingPath);

            blob.UploadFromStream(file);

        }
        public static void RemoveBlobDocument(string fileUrl, string filename, string filecontainer)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container =
                blobClient.GetContainerReference(filecontainer);
            string[] files = fileUrl.Split(new string[] { filecontainer }, StringSplitOptions.None);
            filename = files[1].Remove(0, 1);
            CloudBlockBlob blob = container.GetBlockBlobReference(filename);
            blob.Delete();
            // blob.OpenRead();


        }
        public static string ToShortGuid(this Guid newGuid, int length)
        {
            string modifiedBase64 = Convert.ToBase64String(newGuid.ToByteArray())
                .Replace("+", " ").Replace("/", " ").Replace("-", " ") // avoid invalid URL characters
                .Substring(6, 6 + length);
            return modifiedBase64;
        }
        public static string ConcatenateStrings(string firstString, string secondString = "")
        {
            string concatenatedString = string.Empty;
            if (!string.IsNullOrEmpty(firstString) && !string.IsNullOrEmpty(secondString))
                concatenatedString = firstString + " " + secondString;
            else if (!string.IsNullOrEmpty(firstString))
                concatenatedString = firstString;
            else if (!string.IsNullOrEmpty(secondString))
                concatenatedString = secondString;

            return concatenatedString;
        }


        public static DataSet ImportExceltoDataset(string file)
        {
            DataSet data = new DataSet();
            try
            {
                DataTable tbl;
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                    throw new Exception("File " + file + " Does Not Exists");
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    for (int i = 1; i <= xlPackage.Workbook.Worksheets.Count; i++)
                    {
                        tbl = new DataTable();
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i];

                        bool hasHeader = true;
                        foreach (var firstRowCell in worksheet.Cells[1, 1, 1, 2])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? 2 : 1;
                        for (var rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 1, rowNum, 2];
                            var row = tbl.NewRow();
                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text.Trim();
                            }
                            tbl.Rows.Add(row);

                        }
                        tbl.TableName = worksheet.Name;
                        data.Tables.Add(tbl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return data;

        }


        public static DataSet ImportExceltoDatasetContact(string file)
        {
            DataSet data = new DataSet();
            try
            {
                DataTable tbl;
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                    throw new Exception("File " + file + " Does Not Exists");
                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    for (int i = 1; i <= xlPackage.Workbook.Worksheets.Count; i++)
                    {
                        tbl = new DataTable();
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i];

                        bool hasHeader = true;

                        foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? 2 : 1;
                        for (var rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                            var row = tbl.NewRow();
                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text.Trim();
                            }
                            tbl.Rows.Add(row);

                        }
                        tbl.TableName = worksheet.Name;
                        data.Tables.Add(tbl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return data;

        }

        //read file from azure blob
        public static DataSet ImportExceltoDatasetContact(string file, string filecontainer)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
         CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container =
                blobClient.GetContainerReference(filecontainer);
            CloudBlockBlob blob = container.GetBlockBlobReference(file);
            DataSet data = new DataSet();
            try
            {
                DataTable tbl;
                //FileInfo fileInfo = new FileInfo(file);
                //if (!fileInfo.Exists)
                //    throw new Exception("File " + file + " Does Not Exists");
                using (ExcelPackage xlPackage = new ExcelPackage(blob.OpenRead()))
                {
                    for (int i = 1; i <= xlPackage.Workbook.Worksheets.Count; i++)
                    {
                        tbl = new DataTable();
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[i];

                        bool hasHeader = true;

                        foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? 2 : 1;
                        for (var rowNum = startRow; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                            var row = tbl.NewRow();
                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text.Trim();
                            }
                            tbl.Rows.Add(row);

                        }
                        tbl.TableName = worksheet.Name;
                        data.Tables.Add(tbl);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return data;

        }
        public static string GetGlobalizedLabel(string page, string label)
        {
            if (string.IsNullOrWhiteSpace(label))
                return label;
            if (label.Contains(' '))
                return label;

            UserCulture userCulture = new UserCulture();

            userCulture = CultureInformationManagement.GetCultureObject();
            if (userCulture.CultureXML != null)
            {
                string globalTextForLabel = label;

                string xmlSearchPattern = "/CultureInformation/" + page + "/" + label;

                XmlNode objNode = userCulture.CultureXML.SelectSingleNode(xmlSearchPattern);

                if (objNode != null)
                {
                    globalTextForLabel = objNode.InnerText;
                }

                return globalTextForLabel;
            }
            return label;
        }


        public static string GetGlobalizedDropDownItemText(string dropDownName, string itemText)
        {
            UserCulture userCulture = new UserCulture();

            userCulture = CultureInformationManagement.GetCultureObject();
            if (userCulture.CultureXML != null)
            {
                string globalTextForLabel = itemText;

                string xmlSearchPattern = "/CultureInformation/" + dropDownName + "/" + itemText;

                XmlNode objNode = userCulture.CultureXML.SelectSingleNode(xmlSearchPattern);

                if (objNode != null)
                {
                    globalTextForLabel = objNode.InnerText;
                }

                return globalTextForLabel;
            }
            return itemText;
        }

        public static string GetCurrentCulturScript()
        {
            return CultureInformationManagement.CurrentUserCulture + ".js";


        }

        public static string GetCurrentCulturLogo()
        {
            return CultureInformationManagement.CurrentUserCulture + "_logo.png";
        }

        public static string GetCurrentCulturLoginLogo()
        {
            return CultureInformationManagement.CurrentUserCulture + "_loginlogo.png";
        }

        public static string GetCurrentCulturMenuLogo()
        {
            return CultureInformationManagement.CurrentUserCulture + "_menulogo.png";
        }



        public static string GenerateToken()
        {
            Guid g = Guid.NewGuid();
            string token = Convert.ToBase64String(g.ToByteArray());
            token = token.Replace("=", "");
            token = token.Replace("+", "");
            return token;

        }

        public static string GetRatingImage(string image, bool islastStage = false, bool isWinClose = false)
        {
            string fileName = "";

            if (islastStage && isWinClose)
                fileName = "winlead";
            else if (islastStage && !isWinClose)
                fileName = "lostlead";
            else
                fileName = image;
            return fileName;
        }
        public static IList<Utility.WebClasses.Priority> GetPriorities()
        {
            IList<Utility.WebClasses.Priority> priorities = new List<Utility.WebClasses.Priority>();
            Priority priority = null;
            Array values = Enum.GetValues(typeof(Utility.Enums.TaskPriority));

            foreach (Utility.Enums.TaskPriority item in values)
            {
                priority = new Priority();
                priority.PriorityId = (int)item;
                priority.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item);
                priorities.Add(priority);
            }
            return priorities;
        }

        public static IList<Module> GetModuls()
        {
            List<Module> associatedModules = new List<Module>();
            Module module = new Module();
            module.ModuleId = (int)Utility.Enums.Module.Account;
            module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Account);
            associatedModules.Add(module);
            module = new Module();
            module.ModuleId = (int)Utility.Enums.Module.Lead;
            module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Lead);
            associatedModules.Add(module);
            module = new Module();
            module.ModuleId = (int)Utility.Enums.Module.Contact;
            module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Contact);
            associatedModules.Add(module);
            return associatedModules;
        }

        public static string ReadFile(string filePath)
        {
            string text = string.Empty;

            if (File.Exists(filePath))
            {
                System.IO.StreamReader myFile =
            new System.IO.StreamReader(filePath);
                text = myFile.ReadToEnd();
                myFile.Close();
            }
            return text;
        }
        public static string ConfigurePasswordRecoveryMailBody(MailBodyTemplate mailBodyTemplate)
        {
            string messageBody = mailBodyTemplate.MailBody;
            messageBody = messageBody.Replace(Constants.Registration_User_Name, mailBodyTemplate.RecipientName)
                .Replace(Constants.LEADS_TIMEEXCEED_NOTIFICATION_SUBJECT, mailBodyTemplate.Subject)
                .Replace(Constants.Lead_Info, mailBodyTemplate.LeadInfo);

            //.Replace(Constants.Stage_Name, mailBodyTemplate.StageName)
            //.Replace(Constants.Stage_Duration, mailBodyTemplate.DayDifference)
            //.Replace(Constants.Lead_Name, mailBodyTemplate.Title)
            //.Replace(Constants.Lead_Duration, mailBodyTemplate.DayDifference);

            return messageBody;
        }
        public static DateTime ConvertToDateByCulture(this string DueDate,string CultureName)
        {
            if((CultureName!=null)&&(CultureName!="")){
            Thread.CurrentThread.CurrentCulture = new CultureInfo(CultureName.Replace("_","-"));
            }
            DateTime dateResult = new DateTime();
            DateTime.TryParse(DueDate, out dateResult);
            return dateResult;
        }
    }

}
