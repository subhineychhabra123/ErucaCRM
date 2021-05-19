using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class AccountModel
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public Nullable<int> ParentAccountId { get; set; }
        public int AccountOwnerId { get; set; }
        public Nullable<int> AccountTypeId { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        public Nullable<int> NumberOfEmployee { get; set; }
        public string AccountLocation { get; set; }
        public string AccountOwner{ get; set; }
        //{
        //    get
        //    {
        //        if (this.UserModel != null)
        //            return this.UserModel.FullName;
        //        else
        //            return "";
            
        //    }
        //}
        public string AccountWebsite { get; set; }
        public string NewTagNames { get; set; }
        public string AccountTagIds { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public Nullable<decimal> AnnualRevenue { get; set; }
        public Nullable<int> IndustryId { get; set; }
        public Nullable<int> BillingAddressId { get; set; }
        public Nullable<int> ShippingAddressId { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public virtual ICollection<AccountTagModel> AccountTagModels { get; set; }
        
        private AccountTypeModel _accountTypeModel;
        public virtual AccountTypeModel AccountTypeModel
        {
            get
            {
                if (this._accountTypeModel == null)
                {
                    this._accountTypeModel = new AccountTypeModel();
                }
                return this._accountTypeModel;
            }
            set
            {
                this._accountTypeModel = value;
            }
        }

        private AddressModel _billingAddress;
        private AddressModel _shippingAddress;
        public virtual AddressModel AddressModel
        {
            get
            {
                if (this._billingAddress == null)
                {
                    this._billingAddress = new AddressModel();
                }
                return this._billingAddress;
            }
            set
            {
                this._billingAddress = value;
            }
        }
        public virtual AddressModel AddressModel1
        {
            get
            {
                if (this._shippingAddress == null)
                {
                    this._shippingAddress = new AddressModel();
                }
                return this._shippingAddress;
            }
            set
            {
                this._shippingAddress = value;
            }
        }
        public virtual CompanyModel Company { get; set; }
        public virtual IndustryModel Industry { get; set; }

        private ICollection<ContactModel> _ContactModels;
        public virtual ICollection<ContactModel> ContactModels
        {
            get
            {
                if (this._ContactModels == null)
                    this._ContactModels = new List<ContactModel>();
                return this._ContactModels;
            }
            set { this._ContactModels = value; }
        }

      


        private ICollection<FileAttachmentModel> _FileAttachmentModels;
        public virtual ICollection<FileAttachmentModel> FileAttachmentModels
        {
            get
            {
                if (this._FileAttachmentModels == null)
                    this._FileAttachmentModels = new List<FileAttachmentModel>();
                return this._FileAttachmentModels;
            }
            set { this._FileAttachmentModels = value; }
        }

        private ICollection<LeadModel> _LeadsModels;
      
        public virtual ICollection<LeadModel> LeadsModels
        {
            get
            {
                if (this._LeadsModels == null)
                    this._LeadsModels = new List<LeadModel>();
                return this._LeadsModels;
            }
            set { this._LeadsModels = value; }
        }


        public virtual ICollection<SalesOrderModel> SalesOrdersModels { get; set; }
        public virtual ICollection<AccountCaseModel> AccountCaseModels { get; set; }


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
    }
}
