using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;
namespace ErucaCRM.Web.ViewModels
{
    public class CaseMessageBoardVM : BaseModel
    {
        public string CaseMessageBoardId { get; set; }
        public string AccountCaseId { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByEncrypted { get; set; }
        public string CreatedByName { get; set; }
        public string UserId { get; set; }
        public string FilePathAndName { get; set; }
        public string CreatedByUserImg { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public virtual UserVM User { get; set; }
        public virtual ICollection<FileAttachmentVM> FileAttachments { get; set; }
        public string CreatedDateTimeText
        {
            get
            {
                return CreatedDate.ToDateTimeNow().ToString("MMMM dd, yyyy H:mm:ss");
            }
        }
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
        public class PageSubHeader
        {
            public string PageSubHeaderCaseMessageBoard { get; set; }
            public string PageSubHeaderAttachFile { get; set; }
        }
        public class PageButton
        {
            public string ButtonUpload { get; set; }
            public string ButtonAttach { get; set; }          
        }
    }
}