using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Web;


namespace ErucaCRM.Utility.WebClasses
{
    [Serializable]
    public class CurrentUser
    {
        public int UserId { get; set; }
        public string UserIdEncrypted { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string FullName { get; set; }
        private string _currentCulture;
        public string CurrentCulture
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._currentCulture) ? CultureInformationManagement.ApplicationDefaultCulture : this._currentCulture;
            }

            set { this._currentCulture = value; }
        }
        public Enums.UserType Role { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        private string _profileImageUrl = string.Empty;
        private string OffSet { get; set; }
        public string ProfileImageUrl
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._profileImageUrl) ? "no_image.gif" : this._profileImageUrl;
            }
            set { this._profileImageUrl = value; }
        }

        public string RoleName
        {
            get
            {
                return Role.ToString();
            }
        }
        public string TimeZoneOffSet { get; set; }
    }

    public class CultureLabel
    {
        public string LabelName { get; set; }
        public string LabelDisplayText { get; set; }
    }

    public class UserCulture
    {

        public string CultureName { get; set; }
        public XmlDocument CultureXML { get; set; }

    }

    public class AssociatedModuleResponses
    {
        public int Id { get; set; }
        public String value { get; set; }
    }

    public class Priority
    {
        public int PriorityId { get; set; }
        public String PriorityName { get; set; }
    }
    public class Origin
    {
        public int OriginId { get; set; }
        public String OriginName { get; set; }
    }
    public class CaseType
    {
        public int CaseTypeId { get; set; }
        public String CaseTypeName { get; set; }
    }
    public class Status
    {
        public int StatusId { get; set; }
        public String StatusName { get; set; }
    }
    public class Response
    {
        public string Status;
        public int StatusCode;
        public string Message;
    }

    public class ChangePasswordInfo
    {
        public string CurrentPassword;
        public string NewPassword;
    }

    public class ApplicationPageInfo
    {
        public string ApplicationPageId { get; set; }
        public string CustomPageId { get; set; }
        public string CultureInformationId { get; set; }
        public bool IsViewAll { get; set; }
    }

    public class Response<T>
    {
        public string Status;
        public int StatusCode;
        public string Message;
        public List<T> List;
        public int TotalRecords;
        public int TotalPages;
    }


    public class ListingParameters
    {
        public string AccountId { get; set; }
        public string TagId { get; set; }
        public string TagSearchName { get; set; }
        public string ContactID { get; set; }
        public string StageId { get; set; }
        public int CurrentPage { get; set; }
        public string SearchLeadName { get; set; }
        public string AccountCaseId { get; set; }
        public string LeadId { get; set; }
        public string sortColumnName { get; set; }
        public string sortdir { get; set; }

       
    }

    public class Owner
    {
        public int OwnerId { get; set; }
        public String OwnerName { get; set; }
    }

    public class StageSort
    {
        public string StageId_encrypted { get; set; }
        public int StageId { get { return StageId_encrypted.Decrypt(); } }
        public int StageIndex { get; set; }
    }

    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool HasAccess { get; set; }
    }
    public class MailBodyTemplate
    {
        public string RecipientName { get; set; }
        public string LeadId { get; set; }
        public string DayDifference { get; set; }
        public string StageName { get; set; }
        public string MailBody { get; set; }
        public string Title { get; set; }
        public string StageId { get; set; }
        public string Subject { get; set; }
        public string LeadInfo { get; set; }

    }
}
