using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace ErucaCRM.Domain
{
    public class AccountCaseModel
    {
        public int AccountCaseId { get; set; }
        public int CaseTypeId { get; set; }
        public string CaseNumber { get; set; }
        public int AccountId { get; set; }
        public int CaseOwnerId { get; set; }
        public String CaseOwnerName { get; set; }
        public String AccountName { get; set; }
        public string CaseOrigin { get; set; }
        public int CaseOriginId { get; set; }
        public string Subject { get; set; }
        public string CaseType { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public int TotalCaseMessageBoards { get; set; }
     
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public virtual UserModel User { get; set; }
      
        public virtual ICollection<CaseMessageBoardModel> CaseMessageBoards { get; set; }
        public virtual ICollection<FileAttachmentModel> FileAttachments { get; set; }

      
    }
}
