using ErucaCRM.Business;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Threading;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;


namespace WorkerRoleRecentActivity
{
    public class RecentActivitiesAlert
    {
        List<string> leadAuditIds = new List<string>();
        StringBuilder logdata = new StringBuilder("CompanyId, UserId, Status, Message");
        bool hasError = false;
        static  IUnitOfWork unitOfWork = new UnitOfWork();
        List<RecentActivitieModel> RecentActivitiesModel = new List<RecentActivitieModel>();
        LeadNotifcationBusiness objLeadNotifcationBusiness = new LeadNotifcationBusiness(unitOfWork);
        UserBusiness userBusiness = new UserBusiness(unitOfWork);
        List<UserSetting> accountSetting = new List<UserSetting>();
        string MailBodyTemplat = "";
        string mailSubjectTemplat = "";
        int companyID = 0;
        int maxLeadAuditId = 0;
        DateTime startdate = DateTime.Now;
        ErucaCRM.Utility.MailHelper objmailhelper = new ErucaCRM.Utility.MailHelper();
       /// <summary>
       /// Save Recent Activites emails to azure table 
       /// </summary>
        public  void GetRecentActivitiesEmailData()
        {
            Trace.TraceInformation("Worker Role RecentActivity start GetRecentActivitiesEmailData() {0}", DateTime.Now);
          
            try
            {
                List<int> objallcompany = objLeadNotifcationBusiness.GetAllActiveCompanies();              
                foreach (int companyId in objallcompany)
                {
                    try
                    {
                        accountSetting = userBusiness.GetSendNotificationsActiveUsersId(companyId);
                    }
                    catch(Exception accountSettingExp)
                    {
                        hasError = true;
                        logdata.Append("\n");
                        logdata.Append(companyID + "," + " " + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + accountSettingExp.Message + ". Error Occured When Feching  User Account Setting Data");
                        continue;
                    }
                    try
                    {
                        foreach (var user in accountSetting)
                        {

                            WorkerRoleRecentActivity.CultureName = user.User.CultureInformation == null ? "en-US" : user.User.CultureInformation.CultureName;
                         WorkerRoleCommonFunctions.SetCurrentUserCulture();
                         WorkerRoleRecentActivity.UserId = user.UserId;
                            companyID = companyId;
                            WorkerRoleRecentActivity.TimeZoneOffSet = user.User.TimeZone == null ? "0" : user.User.TimeZone.offset.ToString();
                            int TotalRecords = 0;
                            List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivities = new List<SSP_GetRecentActivitesForEmailNotification_Result>() ;
                                 try
                                 {
                                     RecentActivities = objLeadNotifcationBusiness.GetRecentActivitiesForNotification(ErucaCRM.Utility.ReadConfiguration.PageSize, companyId, user.UserId, ref TotalRecords);
                                     AutoMapper.Mapper.Map(RecentActivities, RecentActivitiesModel);
                                 }
                                 catch (Exception exRecentActivities)
                                 {
                                     hasError = true;
                                     logdata.Append("\n");
                                     logdata.Append(companyID + "," + WorkerRoleRecentActivity.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exRecentActivities.Message + ". Error Occured When Feching Recent Activites Data");
                                     continue;
                                 }

                                 objmailhelper = new ErucaCRM.Utility.MailHelper();
                            if (RecentActivities.Count > 0)
                            {
                                maxLeadAuditId = RecentActivities.Max(x => x.LeadAuditId);
                            }
                            foreach (var homemodel in RecentActivitiesModel)
                            {
                                try
                                {
                                    MailBodyTemplat = WorkerRoleCommonFunctions.GetGlobalizedLabel("EmailTemplates", "RecentActivityEmailBody", WorkerRoleRecentActivity.CultureName);
                                    mailSubjectTemplat = WorkerRoleCommonFunctions.GetGlobalizedLabel("EmailTemplates", "RecentActivityEmailSubject", WorkerRoleRecentActivity.CultureName);
                                    objmailhelper.Body = objmailhelper.Body + string.Format(MailBodyTemplat, ErucaCRM.Utility.ReadConfiguration.OwnerDetail + Convert.ToInt32(homemodel.CreatedBy).Encrypt(), ErucaCRM.Utility.ReadConfiguration.NoImage, homemodel.LeadAuditId, homemodel.ActivityText, homemodel.ActivityCreatedTime);
                                    if (objmailhelper.Subject == null)
                                    {
                                        objmailhelper.Subject = string.Format(mailSubjectTemplat, TotalRecords);
                                    }
                                    leadAuditIds.Add(homemodel.LeadAuditId);
                                }
                                catch (Exception expEmailTemplate)
                                {
                                    hasError = true;
                                    logdata.Append("\n");
                                    logdata.Append(companyID + "," + WorkerRoleRecentActivity.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + expEmailTemplate.Message + ". Error Occured When Reading Config File (OwnerDetail|NoImage|) || Reading Culture File.(EmailTemplates(RecentActivityEmailBody|RecentActivityEmailSubject|))");
                                  
                                }
                            }
                            if (RecentActivitiesModel.Count > 0)
                            {
                                try
                                {
                                    objmailhelper.Body = "<div style='width: 450px'> <div style='width:100%;text-align:center'><img src='" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath + "'></div>" + objmailhelper.Body;

                                    if (TotalRecords > ErucaCRM.Utility.ReadConfiguration.PageSize)
                                    {
                                        objmailhelper.Body = objmailhelper.Body + " <div style='text-align: right; margin: 15px 0 0 0;'><a href='" + ErucaCRM.Utility.ReadConfiguration.PopUrl + "' style='text-decoration:none;background: none repeat scroll 0 0 #0798bc; width:300px;color: #fff; font-size: 9px; border: 1px solid #8db6e4; border-radius: 3px; cursor: pointer; margin: 14px 5px 13px; padding: 1px;'>" + WorkerRoleCommonFunctions.GetGlobalizedLabel("DashBoard", "ViewAll", WorkerRoleRecentActivity.CultureName) + "</a></div></div>";
                                    }
                                    //send email
                                    objmailhelper.ToAddress = user.User.EmailId;
                                    objmailhelper.RecipientName = user.User.FirstName +" "+ user.User.LastName;
                                    //objmailhelper.SendMailMessage(objmailhelper.ToAddress, objmailhelper.Subject, objmailhelper.Body);

                                    // Retrieve the storage account from the connection string.
                                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                    CloudConfigurationManager.GetSetting("StorageConnectionString"));
                                    // Create the table client.
                                    CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                                    // Create the table if it doesn't exist.
                                    CloudTable table = tableClient.GetTableReference("Message");
                                    table.CreateIfNotExists();
                                    // Create a new customer entity.
                               
                                    Message message = new Message();
                                    message.ToAddress = objmailhelper.ToAddress;
                                    message.Subject = objmailhelper.Subject;
                                    message.Body = objmailhelper.Body;
                                    message.ReciepientName = objmailhelper.RecipientName;                                 
                                    var sendEmailRow = new SendEmail
                                    {
                                        PartitionKey = message.ReciepientName,
                                        RowKey = message.ToAddress,
                                        EmailAddress = message.ToAddress,
                                        EmailSent = false,
                                        MessageBody = message.Body,
                                        ScheduledDate = DateTime.Now,
                                        FromEmailAddress = ErucaCRM.Utility.ReadConfiguration.EmailForScheduler,
                                        SubjectLine = message.Subject,
                                       
                                    };

                                    try
                                    {
                                        TableOperation insertOperation = TableOperation.InsertOrReplace(sendEmailRow);
                                        table.Execute(insertOperation);
                                        Trace.TraceInformation("Worker Role RecentActivity saved data in message table {0}", DateTime.Now);
                                    }
                                    catch (Exception ex)
                                    {
                                        string err = "Error creating SendEmail row:  " + ex.Message;
                                        if (ex.InnerException != null)
                                        {
                                            err += " Inner Exception: " + ex.InnerException;
                                        }
                                        Trace.TraceError(err);
                                    }

                                    string queueMessageString =
                                    sendEmailRow.PartitionKey + "," +
                                    sendEmailRow.RowKey + ",";
                                    // Create the queue client.
                                    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                                    // Retrieve a reference to a queue.
                                    CloudQueue queue = queueClient.GetQueueReference("azuremailqueue");
                                    // Create the queue if it doesn't already exist.
                                    queue.CreateIfNotExists();
                                    var queueMessage = new CloudQueueMessage(queueMessageString);
                                    // Create a message and add it to the queue.
                                    CloudQueueMessage cloudMessage = new CloudQueueMessage("azuremailqueue");
                                    queue.AddMessage(queueMessage);
                                    Trace.TraceInformation("Worker Role RecentActivity saved data in queue table {0}",DateTime.Now);
                                    userBusiness.UpdateUserNotification(WorkerRoleRecentActivity.UserId, maxLeadAuditId);

                                    logdata.Append("\n");
                                    logdata.Append(companyID + "," + WorkerRoleRecentActivity.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Success.ToString() + "," + objmailhelper.Subject);
                                }
                                catch (System.Net.Mail.SmtpException expEmailSend)
                                {
                                    hasError = true;
                                    logdata.Append("\n");
                                    logdata.Append(companyID + "," + WorkerRoleRecentActivity.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + expEmailSend.Message + ". Error Occured When Sending Email || Reading Config File File(PopUrl|SiteUrl|WebsiteLogoPath|PageSize).");
                                }
                                catch (Exception expEmailSend)
                                {
                                    hasError = true;
                                    logdata.Append("\n");
                                    logdata.Append(companyID + "," + WorkerRoleRecentActivity.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + expEmailSend.Message + ". Error Occured When Sending Email || Reading Config File File(PopUrl|SiteUrl|WebsiteLogoPath|PageSize).");
                                }
                            }
                        }
                    }
                    catch (Exception exeption)
                    {
                        hasError = true;
                        logdata.Append("\n");
                        logdata.Append(companyID + "," + WorkerRoleRecentActivity.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exeption.Message);

                    }
                }
                SaveLogData();
            }
            catch(Exception exep)
            { 
                logdata.Append("\n");
                logdata.Append("" + "," + "" + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exep.Message+ ". Error Occur When Feching All Company Data.Please Check Network Releated Information.");
                hasError = true;
                SaveLogData();
       
            }

           
        }

        /// <summary>
        /// save log data in sql server in both case success or failure
        /// </summary>
        private void SaveLogData()
        {
            Trace.TraceInformation("Worker Role RecentActivity saved log data in sql server table {0}",DateTime.Now);
            LeadEmailNotificationModel leadEmailNotificationModel = new LeadEmailNotificationModel();
            leadEmailNotificationModel.HasError = hasError;
            leadEmailNotificationModel.LogData = logdata.ToString(); ;
            leadEmailNotificationModel.LogType = (int)ErucaCRM.Utility.Enums.LogErrorType.RecentActivity;
            leadEmailNotificationModel.EndDate = DateTime.Now;
            leadEmailNotificationModel.StartDate = startdate;
            objLeadNotifcationBusiness.SaveEmailNotificationDetail(leadEmailNotificationModel);
                
        }
       
    }

}
