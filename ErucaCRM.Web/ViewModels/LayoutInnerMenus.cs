using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public static class LayoutInnerMenus
    {
        public static string Module { get { return "LayoutInner"; } }
        public static string Home
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Home"); }

        }

        public static string Dashboard
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Dashboard"); }

        }

        public static string Leads
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Leads"); }

        }

        public static string Contacts
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Contacts"); }

        }
        public static string Activities
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Activities"); }

        }
        public static string Helpbutton
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Helpbutton"); }

        }
      
        public static string Quotes
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Quotes"); }

        }
        public static string Accounts
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AccountsContacts"); }

        }

        public static string AccountAndContact
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Account And Contact"); }

        }

        public static string SaleOrders
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "SaleOrders"); }

        }

        public static string Cases
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Cases"); }

        }
        public static string Invoices
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Invoices"); }

        }
        public static string SideMenuConfiguration
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "SideMenuConfiguration"); }

        }

        public static string SideMenuConfigurationUsers
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ConfigurationUsers"); }

        }

        public static string SideMenuConfigurationRoles
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ConfigurationRoles"); }

        }
        public static string SideMenuConfigurationProfiles
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "ConfigurationProfiles"); }

        }

        public static string Tags
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Tags"); }

        }

        public static string SideMenuConfigurationAccountSettings
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "AccountSettings"); }

        }
        public static string SideMenuConfigurationStages
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "Stages"); }

        }


        public static string AddLead
        {

            get { return CommonFunctions.GetGlobalizedLabel(Module, "AddLead"); }

        }

      

        public static string AddAccount
        {

            get { return CommonFunctions.GetGlobalizedLabel(Module, "AddAccount"); }

        }

        public static string AddContact
        {

            get { return CommonFunctions.GetGlobalizedLabel(Module, "AddContact"); }

        }
        public static string AddActivity
        {

            get { return CommonFunctions.GetGlobalizedLabel(Module, "AddActivity"); }

        }
        public static string Menu
        {

            get { return CommonFunctions.GetGlobalizedLabel(Module, "Menu"); }

        }
        public static string BellTitle
        {
            get
            {
                return CommonFunctions.GetGlobalizedLabel(Module, "BellTitle");
            }
        }
        public static string ViewAll
        {
            get
            {
                return CommonFunctions.GetGlobalizedLabel(Module, "ViewAll");
            }
        }
    }
}