using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "FileAttachment")]
    public class FileAttachmentVM
    {
        public long DocumentId { get; set; }
        public string DocumentName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> AttachedBy { get; set; }
        public string DocumentPath { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserIdEncrypt { get; set; }
        public string AccountId { get; set; }
        public string TaskId { get; set; }
        public String LeadId { get; set; }
        public string ContactId { get; set; }
        public string AccountCaseId { get; set; }
        public Nullable<int> CompanyId { get; set; }         
        public virtual LeadUserVM User { get; set; }
        public Nullable<int> CaseMessageBoardId { get; set; }
        public string CaseMessageBoardFilePath { get; set; }
        public string AccountCaseFilePath { get; set; }
        public string LeadFilePath { get; set; }
        public string UserName { get; set; }
    }
}