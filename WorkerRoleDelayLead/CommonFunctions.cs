using ErucaCRM.Business;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Utility.WebClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WorkerRoleDelayLead
{
   public class CommonFunctions
    {
      public static Dictionary<string, XmlDocument> objCultureInformation = new Dictionary<string, XmlDocument>();
       public static string GetGlobalizedLabel(string page, string label,string CultureName)
       {
           if (string.IsNullOrWhiteSpace(label))
               return label;
           if (label.Contains(' '))
               return label;

           UserCulture userCulture = new UserCulture();

           userCulture = GetCultureObject(CultureName);
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
       public static void GetAllCultureObject()
       {
              UnitOfWork unitOfWork = new UnitOfWork();     

            CultureInformationBusiness CultureInfoBusiness = new CultureInformationBusiness(unitOfWork);

            List<ErucaCRM.Domain.CultureInformationModel> listCultureInformationModel = CultureInfoBusiness.LoadAllUserCultures();
          
            for (int i = 0; i < listCultureInformationModel.Count; i++)
            {
               
                XmlDocument xmlDocCulture = new XmlDocument();
                if (!string.IsNullOrEmpty(listCultureInformationModel[i].LabelsXML))
                {
                    xmlDocCulture.LoadXml(listCultureInformationModel[i].LabelsXML);
                }
                objCultureInformation.Add(listCultureInformationModel[i].CultureName, xmlDocCulture);
                
            }
       }
       public static UserCulture GetCultureObject(string CultureName)
       {
           UserCulture user = new UserCulture();
           user.CultureName = CultureName;
           user.CultureXML = objCultureInformation[CultureName];
           return user;
       }
       public static void SetCurrentUserCulture()
       {
           System.Globalization.CultureInfo culture;
           try
           {
               culture = new System.Globalization.CultureInfo(WorkerRoleDelayLead.CultureName);
               System.Threading.Thread.CurrentThread.CurrentCulture = culture;

               System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

           }
           catch (Exception ex) { }
       }
    }
}

