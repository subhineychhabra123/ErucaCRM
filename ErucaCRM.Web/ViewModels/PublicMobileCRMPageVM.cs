using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "PublicPageMobileCRM")]

    public class PublicMobileCRMPageVM: CultureSpecificSiteContentVM
    {

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }

        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }
        public PageContent PageContents
        {
            get
            {
                return new PageContent();
            }

        }

       
        public PageLinkButton PageLinkButtons
        {
            get
            {
                return new PageLinkButton();
            }

        }

        public class PageSubHeader
        {

            public string PageSubHeaderMobileAppsForManagersAndSalesTeam { get; set; }
            public string PageSubHeaderAndroidFriendlyErucaCRM { get; set; }
       


        }

        public class PageContent
        {

            public string MobileAppsForManagersAndSalesTeamPara1Content { get; set; }

            public string MobileAppsForManagersAndSalesTeamPara2Content { get; set; }

        }

      
        public class PageLinkButton
        {
          public string PageLinkButtonManageCustomersAccountsAndAddresses { get; set; }
            public string PageLinkButtonUpdateLeadsAndOpportunities { get; set; }
            public string PageLinkButtonQuickSMSAndEmails { get; set; }
            public string PageLinkButtonJustDialForImmediateConnection  { get; set; }
            public string PageLinkButtonImportPhoneContactsToCRMEWebVersion { get; set; }
            public string PageLinkButtonConnectedToSNSTool { get; set; }


        }

        public class PageButton
        {
          
            public string ButtonStartYourFreeTrial { get; set; }
      

        }

    }
}