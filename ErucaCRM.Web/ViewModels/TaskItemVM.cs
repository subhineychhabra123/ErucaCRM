using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "TaskItem")]
    public class TaskItemVM : BaseModel
    {
        public string TaskId { get; set; }
        [Required(ErrorMessage = "TaskItem.SubjectNameRequired")]
        [Display(Name = "Subject Name")]
        public String Subject { get; set; }
        public string TaskIdEncrypted { get; set; }
        public int Status { get; set; }
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OwnerId { get; set; }
        public string OwnerIdEncrypted { get; set; }
        [Display(Name = "Owner Name")]
        public String OwnerName { get; set; }
        public String TaskAssociatedPerson { get; set; }
        public String TaskType { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string AudioFileName { get; set; }
        public string AudioPath
        {
            get
            {
                return "http://127.0.0.1:10000/devstoreaccount1/activities/"+ this.AudioFileName;

            }
        }
        public string TaskStatus { get; set; }
        public string TaskDueDate { get; set; }
        public String Description { get; set; }
        public int AssociatedModuleId { get; set; }
        public string OwnerImage { get; set; }
        public int AssociatedModuleValue { get; set; }
        public string AccountName { get; set; }
        public string AccountCaseNumber { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean RecordDeleted { get; set; }
        private ICollection<FileAttachmentVM> _FileAttachments;
        public virtual ICollection<FileAttachmentVM> FileAttachments
        {
            get
            {
                if (this._FileAttachments == null)
                    this._FileAttachments = new List<FileAttachmentVM>();
                return this._FileAttachments;
            }
            set { this._FileAttachments = value; }
        }

        public IList<Module> AssociatedModules { get; set; }

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

        public class GridHeader
        {
            public string GridHeaderSubject { get; set; }
            public string GridHeaderDueDate { get; set; }
            public string GridHeaderStatus { get; set; }
            public string GridHeaderPriorityName { get; set; }
            public string FileName { get; set; }
            public string AttachedBy { get; set; }

        }

        public class PageButton
        {
            public string ButtonAddQuote { get; set; }

            public string ButtonAddSalesOrder { get; set; }

            public string ButtonAddInvoice { get; set; }

            public string ButtonAddActivity { get; set; }

            public string ButtonAddProduct { get; set; }
            public string ButtonRemoveProduct { get; set; }
            public string ButtonAssociateProduct { get; set; }
            public string ButtonAttach { get; set; }

            public string ButtonUpload { get; set; }

            public string EditButtonToolTip { get; set; }
            public string DeleteButtonToolTip { get; set; }

        }

        public class PageSubHeader
        {
            public string PageSubHeaderTaskInfo { get; set; }
            public string PageSubHeaderDescription { get; set; }
            public string PageSubHeaderAttachments { get; set; }
            public string PageSubHeaderAttachFile { get; set; }

        }

    }
}