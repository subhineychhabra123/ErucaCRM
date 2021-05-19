using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Utility
{
    public static class Constants
    {
        public const string AUTHENTICATION_ROLE_ADMIN = "Admin";
        public const string AUTHENTICATION_ROLE_USER = "User";
        public const string PROFILE_IMAGE_PATH = "/project/branches/Working/ErucaCRM.Web/Uploads/users/";

        public const string LEAD_DOCS_PATH = "/Uploads/leads/";
        public const string LEAD_AUDIO_PATH = "/Uploads/leads/LeadAudio/";
        public const string ACCOUNT_DOCS_PATH = "/Uploads/accounts/";
        public const string ACTIVITY_DOCS_PATH = "/Uploads/activities/";
        public const string CONTACT_DOCS_PATH = "/Uploads/contacts/";


        public const string LEAD_DOCS_BLOB = "leads";
        public const string LEAD_AUDIO_BLOB= "leadAudio";
        public const string ACCOUNT_DOCS_BLOB = "accounts";
        public const string ACTIVITY_DOCS_BLOB = "activities";
        public const string CONTACT_DOCS_BLOB = "contacts";
        public const string CONTACT_BULKUPLOAD_DOCS_BLOB = "contactsbulkupload";
        public const string PROFILE_IMAGE_BLOB = "profile";

        public const string CONTACT_SAMPLE_FILE_PATH = "/Uploads/contacts/BulkUpload/Sample.xlsx";  
        public const string PROFILE_IMAGE_NAME_PREFIX = "ProfileImage_";
        public const string PERMISSION_VIEW = "Ve";
        public const string PERMISSION_CREATE = "Cr";
        public const string PERMISSION_CREATEDOCUMENT = "CDoc";
        public const string PERMISSION_DELETEDOCUMENT = "DDoc";
        //public const string PERMISSION_EDITDOCUMENT = "EDoc";
        public const string PERMISSION_VIEWDOCUMENT = "VDoc";    
        public const string PERMISSION_EDIT = "E";
        public const string PERMISSION_DELETE = "D";
        public const string MODULE_DOCUMENT = "DocumentsPermissions";
        public const string MODULE_USER = "User";
        public const string MODULE_ROLE = "Role";
        public const string MODULE_PROFILE = "Profile";
        public const string MODULE_LEAD = "Lead";
        public const string MODULE_REPORTSANDDASHBOARDS = "ReportsAndDashboardsPermissions";
        public const string MODULE_CONTACT = "Contact";
        public const string MODULE_CASE = "AccountCase";
        public const string MODULE_TASK = "Task";
        public const string MODULE_ACCOUNT = "Account";
        public const string MODULE_SALESORDER = "SalesOrder";
        public const string MODULE_QUOTE = "Quote";
        public const string MODULE_INVOICE = "Invoice";
        public const string MODULE_CONTENTMANAGEMENT = "ContentManagement";
        public const string MODULE_USERMANAGEMENT = "UserManagement";
        public const string MODULE_PLANMANAGHEMENT = "PlanManagement";
        public const string MODULE_TAG = "Tag";
        public const string MODULE_STAGE = "Stage";
        public const string EXCEL_FILE_EXTENSION = ".xlsx";
        public const string JS_FILE_EXTENSION = ".js";
        public const string DEFAULT_CULTURE_NAME = "English(en-US)";
        public const string AND = "|and|";
        public const string EQUALSTO = "|equalsto|";
        //public pages name

        public const string PUBLIC_PAGE_HOME = "Home";
        public const string PUBLIC_PAGE_LOGIN = "Login";
        public const string PUBLIC_PAGE_REGISTRATION = "Registration";
        public const string PUBLIC_PAGE_FORGOTPASSWORD = "ForgotPassword";
        public const string PUBLIC_PAGE_FEATURE = "Feature";
        public const string PUBLIC_PAGE_MobileCRM = "MobileCRM";
        public const string PUBLIC_PAGE_CONTACTUS = "Contactus";
        public const string PUBLIC_PAGE_ABOUTUS = "Aboutus";
        public const string CULTURE_SPECIFIC_SHEET_DROPDOWNS = "DropDowns";
        public const string CULTURE_SPECIFIC_SHEET_LABELS = "Labels";
          public const int LEAD_STATUS_LOST = 5;
          public const int ACCOUNT_LEAD_STAGE = 2;

          public const string CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION = "SelectOption";


          public const string LEADS_TIMEEXCEED_NOTIFICATION_SUBJECT = "Notification for delay leads";
          public const string Registration_User_Name = "{Registration_User_Name}";
          public const string Stage_Name = "{Stage_Name}";
          public const string Stage_Duration = "{Stage_Duration}";
          public const string Lead_Duration = "{Lead_Duration}";
          public const string Lead_Name = "{Lead_Name}";

          public const string Lead_Info = "{Lead_Info}";
           

    }
}
