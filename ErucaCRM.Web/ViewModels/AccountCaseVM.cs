using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System.Web.Script.Serialization;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "AccountCase")]
    public class AccountCaseVM : BaseModel
    {
        public string AccountCaseId { get; set; }
        public int CaseOriginId { get; set; }
        [Display(Name = "Case Type")]
        public int CaseTypeId { get; set; }
        [Display(Name = "Case Number")]
        public String CaseNumber { get; set; }
      
        [Display(Name = "AccountName")]
        public String AccountName { get; set; }
        [Display(Name = "Case Owner Name")]
        public String CaseOwnerName { get; set; }
        //[Display(Name = "Case Origin")]
        //public String CaseOrigin { get; set; }
        [Required(ErrorMessage = "AccountCase.SubjectNameRequired")]
        [Display(Name = "Subject")]
        public String Subject { get; set; }
        public int AccountId { get; set; }
        public string AccountIdEncrypted { get; set; }
        public int CaseOwnerId { get; set; }
        public string CaseOwnerIdEncrypted { get; set; }
        public string OwnerImage { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int TotalCaseMessageBoards { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public string CaseCreatedTime
        {
            get {
                return CreatedDate!=null? CreatedDate.Value.ToDateTimeNow().ToString("MMMM dd, yyyy H:mm:ss"):DateTime.UtcNow.ToString();
            }
        }
        public string PriorityName
        {
            get
            {
                return CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.CasePriority), this.PriorityId));
            }
        }
        public string CaseOrigin
        {
            get
            {
                return CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.CaseOrigin), this.CaseOriginId));
            }
        }
        public string CaseStatus
        {
            get
            {
                return CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.CaseStaus), this.StatusId));
            }
        }
        public string CaseType
        {
            get
            {
                return CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.CaseType), this.CaseTypeId));
            }
        }
        public IList<Utility.WebClasses.Owner> Owners { get; set; }
        private UserVM _User;
        public virtual UserVM User
        {
            get
            {
                if (this._User == null)
                    this._User = new UserVM();
                return this._User;
            }
            set { this._User = value; }
        }
        public IList<ParentCaseListModel> ParentCaseList { get; set; }
        public IList<OwnerListModel> AccountOwnerList { get; set; }
        public IList<CaseOriginListModel> CaseOriginList { get; set; }
        public IList<CaseOwnerListModel> CaseOwnerList { get; set; }
        public IList<CaseTypeModel> CaseTypeList { get; set; }
        public virtual ICollection<FileAttachmentVM> FileAttachments { get; set; }
        //[ScriptIgnore]
        public virtual ICollection<CaseMessageBoardVM> CaseMessageBoards { get; set; }
        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }
        public GridHeader GridHeaders
        {
            get
            {
                return new GridHeader();
            }

        }
        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }
        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }

        public class GridHeader
        {
            public string AccountName { get; set; }
            public string AccountOwner { get; set; }
            public string CaseNumber { get; set; }
            public string Subject { get; set; }
            public string CaseOrigin { get; set; }
            public string CaseOwnerName { get; set; }
            public string Type { get; set; }
            public string Priority { get; set; }
            public string FileName { get; set; }
            public string AttachedBy { get; set; }
            public string Description { get; set; }
            public string MessageBoardDescription { get; set; }
            public string CreatedBy { get; set; }

        }
        public class PageLabel
        {
            public string AccountName { get; set; }
            public string CaseOwnerName { get; set; }
            public string Subject { get; set; }
            public string CaseOrigin { get; set; }
            public string CaseType { get; set; }
            public string PriorityName { get; set; }
            public string Email { get; set; }
            public string Attachment { get; set; }
        }
        public class PageButton
        {

            public string ButtonAddCase { get; set; }
            public string ButtonAttach { get; set; }
            public string ButtonUpload { get; set; }
            public string ButtonAddActivity { get; set; }
            public string EditButtonToolTip { get; set; }
            public string DeleteButtonToolTip { get; set; }
        }
        public class PageSubHeader
        {
            public string PageSubHeaderCaseInfo { get; set; }
            public string PageSubHeaderDescription { get; set; }
            public string PageSubHeaderAttachmentsInfo { get; set; }
            public string PageSubHeaderAttachFile { get; set; }
            public string PageSubHeaderAddCaseMessageBoard { get; set; }
            public string PageSubHeaderCaseMessageBoardInfo { get; set; }
            public string PageSubHeaderMessageBoardDescription { get; set; }
            public string PageSubHeaderActivitiesInfo { get; set; }
            public string PageSubHeaderAttachmentBy { get; set; }
            public string PageSubHeaderAttachments { get; set; }
        }

    }
}