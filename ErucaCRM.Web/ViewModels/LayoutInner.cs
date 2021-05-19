using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public static class LayoutInner
    {
        public static string Module { get { return "LayoutInner"; } }

        public static string QuickNavigationWhatsNew
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "WhatsNew"); }

        }
        public static string QuickNavigationSubscription
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Subscription"); }

        }
        public static string QuickNavigationSetup
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Setup"); }

        }

        public static string QuickNavigationHelp
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Help"); }

        }
        public static string SignOut
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "SignOut"); }

        }

        public static string MyAccount
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "MyAccount"); }

        }
        public static string LoginInfoWelCome
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Welcome"); }

        }
        public static string LoginInfoAt
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "At"); }

        }
        public static string AddItemToolTip
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "QuickAddItem"); }

        }
        public static string SettingToolTip
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Setting"); }

        }
        public static string SearchWaterMark
        {

            get { return CommonFunctions.GetGlobalizedLabel(Module, "SearchWaterMark"); }

        }
        public static string SelectedLanguageInfo
        {
            get { return "Language-"; }

        }  
    }
}