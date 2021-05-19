using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Web.Script.Serialization;
using System.Web;

namespace ErucaCRM.Business
{
    public class CultureInformationBusiness : ICultureInformationBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CultureInformationRepository cultureInformationRepository;
        private readonly ApplicationPageRepository appliationPageRepository;
        ContentApplicationPageRepository contentApplicationPageRepository;
        public CultureInformationBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            cultureInformationRepository = new CultureInformationRepository(unitOfWork);
            appliationPageRepository = new ApplicationPageRepository(unitOfWork);
            contentApplicationPageRepository = new ContentApplicationPageRepository(unitOfWork);
        }


        public List<CultureInformationModel> GetUserCultures()
        {

            //  userCultureRepository = new UserCultureRepository(unitOfWork);

            List<CultureInformationModel> listUserCulturesModel = new List<CultureInformationModel>();
            List<CultureInformation> listUserCultures = cultureInformationRepository.GetAll(r => r.IsActive == true).ToList();
            AutoMapper.Mapper.Map(listUserCultures, listUserCulturesModel);
            return listUserCulturesModel;
        }

        public List<CultureInformationModel> GetAllCultures()
        {
            List<CultureInformationModel> listUserCulturesModel = new List<CultureInformationModel>();
            List<SSP_GetAllVisibleCulture_Result> culturelist = cultureInformationRepository.GetAllCultures();
            AutoMapper.Mapper.Map(culturelist, listUserCulturesModel);
            return listUserCulturesModel;
        }
        public void EditDocument(CultureInformationModel cultureInfomodel)
        {
            CultureInformation cultureInformationObj = cultureInformationRepository.SingleOrDefault(r => r.CultureInformationId == cultureInfomodel.CultureInformationId);
            cultureInformationObj.LabelsXML = cultureInfomodel.LabelsXML;
            cultureInformationObj.ExcelFilePath = cultureInfomodel.ExcelFilePath;
            cultureInformationRepository.Update(cultureInformationObj);

        }

        public List<ErucaCRM.Domain.CultureInformationModel> LoadAllUserCultures()
        {
            List<ErucaCRM.Domain.CultureInformationModel> lisUserCultures = GetUserCultures();
            return lisUserCultures;
            //for (int i = 0; i < lisUserCultures.Count; i++)
            //{
            //    if (lisUserCultures[i].IsDefault == true)
            //    {
            //        ErucaCRM.Utility.CultureInformationManagement.SetDefaultCulture(lisUserCultures[i].CultureName);
            //    }

            //    ErucaCRM.Utility.CultureInformationManagement.SetCulture(lisUserCultures[i].CultureName, lisUserCultures[i].LabelsXML);
            //}

        }

        public CultureInformationModel GetCultureDetails(int CultureInformationId)
        {
            CultureInformationModel cultureInfoModelObj = new CultureInformationModel();
            CultureInformation cultureInformationObj = cultureInformationRepository.SingleOrDefault(r => r.CultureInformationId == CultureInformationId);
            AutoMapper.Mapper.Map(cultureInformationObj, cultureInfoModelObj);
            return cultureInfoModelObj;
        }

        public List<CultureInformationModel> GetAllCultureNames()
        {
            List<CultureInformationModel> listCultureInformationModel = new List<CultureInformationModel>();
            List<CultureInformation> listCultureInfo = cultureInformationRepository.GetAll(x => x.IsActive == true && x.IsVisible == true).ToList();
            AutoMapper.Mapper.Map(listCultureInfo, listCultureInformationModel);
            return listCultureInformationModel;
        }

        public void UpdateDefaultLanguage(int CultureInformationId)
        {
            CultureInformation cultureInfoObj = cultureInformationRepository.SingleOrDefault(r => r.IsDefault == true);
            if (cultureInfoObj != null)
            {
                cultureInfoObj.IsDefault = false;
                cultureInformationRepository.Update(cultureInfoObj);
            }

            cultureInfoObj = cultureInformationRepository.SingleOrDefault(r => r.CultureInformationId == CultureInformationId);
            if (cultureInfoObj != null)
            {
                cultureInfoObj.IsDefault = true;
                cultureInformationRepository.Update(cultureInfoObj);
            }
        }
        public void SaveLanguage(CultureInformationModel cultureModel)
        {

            CultureInformation cultureInformation = cultureInformationRepository.SingleOrDefault(r => r.CultureInformationId == cultureModel.CultureInformationId);
            cultureInformation.IsActive = cultureModel.IsActive;
            cultureInformationRepository.Update(cultureInformation);
            List<ContentApplicationPage> contentPageList = new List<ContentApplicationPage>();
            List<ApplicationPage> applicationPage = new List<ApplicationPage>();
            applicationPage = appliationPageRepository.GetAll(r => r.IsApplicationPage == true).ToList();
            foreach (var v in applicationPage)
            {
                ContentApplicationPage contentpage = new ContentApplicationPage();
                contentpage = contentApplicationPageRepository.SingleOrDefault(r => r.ApplicationPageId == v.ApplicationPageId && r.CultureInformationId == cultureModel.CultureInformationId);
                if (contentpage == null)
                {
                    contentpage = new ContentApplicationPage();
                    contentpage.ApplicationPageId = v.ApplicationPageId;
                    contentpage.CultureInformationId = cultureModel.CultureInformationId;
                    contentpage.UseDefaultContent = true;
                    contentApplicationPageRepository.Insert(contentpage);
                }
            }

        }


        public CultureInformationModel GetDefaultCultureDetail()
        {
            CultureInformation cultureInformation = cultureInformationRepository.SingleOrDefault(r => r.IsDefault == true);
            CultureInformationModel cultureInformationModel = new CultureInformationModel();
            AutoMapper.Mapper.Map(cultureInformation, cultureInformationModel);
            return cultureInformationModel;
        }

        public string ProcessCultureSpecificData(DataSet dsObject, string CultureName)
        {
            string jsDocPath = ReadConfiguration.CultureJSFilePath;
            string jsFilepath = HttpContext.Current.Server.MapPath((@"~" + jsDocPath + CultureName + Constants.JS_FILE_EXTENSION));
            string jsonString = String.Empty;
            StringBuilder jsonContent = new StringBuilder();
            XmlDocument xmlDocCultureLabels = new XmlDocument();
            XmlNode objRootNode = xmlDocCultureLabels.CreateNode(XmlNodeType.Element, "CultureInformation", null);

            jsonContent.AppendLine("jQuery.namespace('ErucaCRM.Messages');");
            jsonContent.AppendLine("ErucaCRM.Messages = {");

            List<XmlElement> listModuleNodes = new List<XmlElement>();

            for (int i = 0; i < dsObject.Tables.Count; i++)
            {
                int tableRowCounter = 0;
                string tableName = dsObject.Tables[i].TableName;
                jsonString = GetJson(dsObject.Tables[i], ref tableRowCounter);
                jsonString = jsonString.Trim().Substring(1, jsonString.Trim().Length - 2);
                jsonContent.Append(tableName + ":" + jsonString + ",").AppendLine();

                ///Get module XML Elements
                listModuleNodes.Add(GetModuleXmlElement(dsObject.Tables[i], xmlDocCultureLabels, tableRowCounter));
            }
            jsonContent.Remove(jsonContent.ToString().LastIndexOf(','), 1);
            jsonContent.Append("}");
            System.IO.File.WriteAllText(jsFilepath, jsonContent.ToString());

            //Append All module to root node
            for (int i = 0; i < listModuleNodes.Count; i++)
            {
                objRootNode.AppendChild(listModuleNodes[i]);
            }
            xmlDocCultureLabels.AppendChild(objRootNode);
            CreateCultureSpecificHelpTips(dsObject, CultureName);
            return Convert.ToString(xmlDocCultureLabels.InnerXml);
        }
        private void CreateCultureSpecificHelpTips(DataSet dsObject, string CultureName)
        {
            string jsDocPath = ReadConfiguration.CultureHelpJSFilePath;
            string jsFilepath = HttpContext.Current.Server.MapPath((@"~" + jsDocPath + CultureName + Constants.JS_FILE_EXTENSION));
            string jsonString = String.Empty;
            StringBuilder jsonContent = new StringBuilder(); 
            jsonContent.AppendLine("jQuery.namespace('ErucaCRM.Helps');");
            jsonContent.AppendLine("ErucaCRM.Helps = {");
            for (int i = 0; i < dsObject.Tables.Count; i++)
            {
                int tableRowCounter = 0;
                string tableName = dsObject.Tables[i].TableName;
                jsonString = GetHelpJson(dsObject.Tables[i], ref tableRowCounter);
                jsonString = jsonString.Trim();
                jsonContent.Append(tableName + ":" + jsonString + ",").AppendLine();
            }
            jsonContent.Remove(jsonContent.ToString().LastIndexOf(','), 1);
            jsonContent.Append("}");
            System.IO.File.WriteAllText(jsFilepath, jsonContent.ToString());


        }

        private string GetHelpJson(DataTable dt, ref int tableRowCounter)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            int localTableRowCounter = 0;
            var row = new Dictionary<string, object>();
            var innerrow=new Dictionary<string ,object>();
            List<DataRow> listDataRow = dt.Rows.Cast<DataRow>().ToList();
            string header = "";
            string helpkey = "";
            for (int i = tableRowCounter; i < listDataRow.Count(); i++)
            {

                if(header == "HelpTips")
                {
                    if (listDataRow[i][0].ToString() != "HelpKey")
                    {
                        innerrow.Add(listDataRow[i][0].ToString(), listDataRow[i][1].ToString());
                    }
                    else
                    {

                        if (innerrow.Count > 0)
                        {
                            row.Add(helpkey, innerrow);
                            helpkey = listDataRow[i][1].ToString();
                            innerrow = new Dictionary<string, object>();
                        }
                        else
                        {
                            helpkey = listDataRow[i][1].ToString();
                            innerrow = new Dictionary<string, object>();
                        }
                       
                    }
                }
                if (Convert.ToString(listDataRow[i][0]).ToLower() == "helptips")
                {
                    header = "HelpTips";
                }
                localTableRowCounter++;
            }
            if (innerrow.Count > 0)
            {
                row.Add(helpkey, innerrow);              
                innerrow = new Dictionary<string, object>();
            }
            dataRows.Add(row);
            tableRowCounter = localTableRowCounter;
            return  "{\"Tips\":"+ser.Serialize(dataRows)+"}";

        }
        private string GetJson(DataTable dt, ref int tableRowCounter)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            int localTableRowCounter = 0;
            var row = new Dictionary<string, object>();
            List<DataRow> listDataRow = dt.Rows.Cast<DataRow>().ToList();
         
            for (int i = tableRowCounter; i < listDataRow.Count(); i++)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(listDataRow[i][0])) && !string.IsNullOrEmpty(Convert.ToString(listDataRow[i][1])))
                {
                  row.Add(listDataRow[i][0].ToString(), listDataRow[i][1].ToString());
                  
                }
                else if (Convert.ToString(listDataRow[i][0]).ToLower() == "labels" && Convert.ToString(listDataRow[i][1]).ToLower() == "")
                {
                    break;
                }



                localTableRowCounter++;
            }
            dataRows.Add(row);
            tableRowCounter = localTableRowCounter;
            return ser.Serialize(dataRows);

        }
        private XmlElement GetModuleXmlElement(DataTable dt, XmlDocument xmlDoc, int tableRowCounter)
        {
            XmlElement objModuleXMlNode = xmlDoc.CreateElement(dt.TableName);
            XmlNode objLabelNode = null;
            XmlNode objLabelTextNode = null;
            List<DataRow> listDataRow = dt.Rows.Cast<DataRow>().ToList();
            //dt.Rows.Cast<DataRow>().ToList().ForEach(dtrow =>
            for (int i = tableRowCounter; i < listDataRow.Count(); i++)
            {

                if (!string.IsNullOrEmpty(Convert.ToString(listDataRow[i][0])) && !string.IsNullOrEmpty(Convert.ToString(listDataRow[i][1])))
                {
                    objLabelNode = xmlDoc.CreateNode(XmlNodeType.Element, listDataRow[i][0].ToString(), null);
                    objLabelTextNode = xmlDoc.CreateNode(XmlNodeType.Element, "LabelDisplayText", null);
                    objLabelTextNode.InnerText = listDataRow[i][1].ToString();
                    objLabelNode.AppendChild(objLabelTextNode);
                    objModuleXMlNode.AppendChild(objLabelNode);
                }
            }

            return objModuleXMlNode;

        }
    }
}
