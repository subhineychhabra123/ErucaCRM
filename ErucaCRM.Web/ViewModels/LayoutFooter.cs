using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public static class LayoutFooter
    {
        public static string Module { get { return "Layout"; } }

        public static string Features
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Features"); }

        }
     
        public static string FeatureLeadsByStages
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureLeadsByStages"); }

        }
        public static string FeatureTagging
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureTagging"); }

        }


        public static string FeatureDocumentsLibrary
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureDocumentsLibrary"); }

        }

      

        public static string FeatureContactManagement
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureContactManagement"); }

        }
        public static string FeatureCustomerRelationship
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureCustomerRelationship"); }

        }

        public static string FeatureMobileCRM
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureMobileCRM"); }

        }
          public static string FeatureWeChatIntegration
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeatureWeChatIntegration"); }

        }
        
        public static string Resources
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Resources"); }

        }

        public static string ResourcesForum
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ResourcesForum"); }

        }
        public static string ResourcesDeveloperAPI
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ResourcesDeveloperAPI"); }

        }
        public static string ResourcesCaseStudies
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ResourcesCaseStudies"); }

        }

        public static string ResourcesHelpVideos
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ResourcesHelpVideos"); }

        }

        public static string ResourcesApps
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ResourcesApps"); }

        }

       

        public static string AboutCRM
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutCRM"); }

        }
        public static string AboutCRMWhyErucaCRM
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutCRMWhyErucaCRM"); }

        }
        public static string AboutCRMCompany
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutCRMCompany"); }

        }

        public static string AboutCRMCustomers
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutCRMCustomers"); }

        }
        public static string AboutCRMPricing
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutCRMPricing"); }

        }

        public static string AboutCRMContactUs
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutCRMContactUs"); }

        }
        

        public static string ContactUs
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ContactUs"); }

        }
        public static string SpreadTheWord
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "SpreadTheWord"); }

        }

        public static string FooterLast
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FooterLast"); }

        }

    }
}