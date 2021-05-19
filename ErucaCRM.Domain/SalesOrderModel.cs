using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ErucaCRM.Domain
{
    public class SalesOrderModel
    {
        public SalesOrderModel()
        {
            this.ProductSalesOrderAssociationModels = new HashSet<ProductSalesOrderAssociationModel>();
        }

        public int SalesOrderId { get; set; }
        public string Subject { get; set; }
        public Nullable<int> QuoteId { get; set; }
        public Nullable<int> LeadId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public string Carrier { get; set; }
        public Nullable<decimal> SalesCommission { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<int> DiscountType { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<bool> TaxApplied { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<bool> VatApplied { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<int> AdjustmentType { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public string Terms { get; set; }
        public string OwnerName { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public Nullable<int> BillingAddressId { get; set; }
        public Nullable<int> ShippingAddressId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
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

        //public String OwnerName
        //{
        //    get
        //    {
        //        if (this.UserModel != null && this.UserModel.FullName != " ")
        //            return this.UserModel.FullName;
        //        else
        //            return string.Empty;
        //    }
        //}

        //public String LeadName
        //{
        //    get
        //    {
        //        if (this.LeadModel != null && this.LeadModel.Name != null)
        //            return this.LeadModel.Name;
        //        else
        //            return string.Empty;
        //    }
        //}
        //public String AccountName
        //{
        //    get
        //    {
        //        if (this.AccountModel != null && this.AccountModel.AccountName != null)
        //            return this.AccountModel.AccountName;
        //        else
        //            return string.Empty;
        //    }
        //}
        private ICollection<ProductSalesOrderAssociationModel> _ProductSalesOrderAssociationModels;
        public virtual ICollection<ProductSalesOrderAssociationModel> ProductSalesOrderAssociationModels
        {
            get
            {
                if (this._ProductSalesOrderAssociationModels == null)
                    this._ProductSalesOrderAssociationModels = new List<ProductSalesOrderAssociationModel>();
                return this._ProductSalesOrderAssociationModels;
            }
            set { this._ProductSalesOrderAssociationModels = value; }
        }

        //private LeadModel _LeadModel;
        //[ScriptIgnore]
        //public virtual LeadModel LeadModel
        //{
        //    get
        //    {
        //        if (this._LeadModel == null)
        //            this._LeadModel = new LeadModel();
        //        return this._LeadModel;
        //    }
        //    set { this._LeadModel = value; }
        //}
        private AccountModel _AccountModel;
        [ScriptIgnore]
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
        [ScriptIgnore]
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

        private QuoteModel _QuoteModel;
        public virtual QuoteModel QuoteModel
        {
            get
            {
                if (this._QuoteModel == null)
                    this._QuoteModel = new QuoteModel();
                return this._QuoteModel;
            }
            set { this._QuoteModel = value; }
        }
    }
}
