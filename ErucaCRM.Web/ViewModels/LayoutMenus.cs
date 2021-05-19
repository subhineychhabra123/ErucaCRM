using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public  class LayoutMenus
    {
        public static string Module { get { return "Layout"; } }
        public static string Home
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Home"); }

        }
        public static string FeaturesBenefits
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FeaturesBenefits"); }

        }

        public static string AboutUs
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AboutUs"); }

        }
        public static string MobileCRM
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "MobileCRM"); }

        }
        public static string ContactUs
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ContactUs"); }

        }

        public static string PricingAndSignIn
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "PricingAndSignIn"); }

        }
    }
}