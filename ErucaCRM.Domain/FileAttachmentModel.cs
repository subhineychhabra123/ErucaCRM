using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class FileAttachmentModel
    {
        public long DocumentId { get; set; }
        public string DocumentName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> AttachedBy { get; set; }
        public string DocumentPath { get; set; }
        public Nullable<int> LeadId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> TaskId { get; set; }
        public Nullable<int> CaseMessageBoardId { get; set; }
        public Nullable<int> AccountCaseId { get; set; }
        public string FileDuration { get; set; }
        private AccountModel _AccountModel;
        public virtual AccountModel AccountModel
        {
            get
            {
                if (this._AccountModel == null)
                    this._AccountModel = new AccountModel();
                return this._AccountModel;
            }
            set { this._AccountModel = value; }
        }

        private UserModel _UserModel;
        public virtual UserModel UserModel
        {
            get
            {
                if (this._UserModel == null)
                    this._UserModel = new UserModel();
                return this._UserModel;
            }
            set { this._UserModel = value; }
        }
        public virtual CaseMessageBoardModel CaseMessageBoard { get; set; }
    }
}
