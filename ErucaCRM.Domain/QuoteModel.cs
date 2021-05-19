using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Web.Script;
using System.Web.Script.Serialization;

namespace ErucaCRM.Domain
{
    public class QuoteModel
    {
        public QuoteModel()
        {
            this.ProductQuoteAssociationModels = new HashSet<ProductQuoteAssociationModel>();
        }
        public int QuoteId { get; set; }
        public string Subject { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<int> LeadId { get; set; }
        public Nullable<System.DateTime> ValidTill { get; set; }
        public string Carrier { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<int> DiscountType { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<bool> TaxApplied { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<bool> VatApplied { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<int> AdjustmentType { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public string Terms { get; set; }
        public Nullable<int> BillingAddressId { get; set; }
        public Nullable<int> ShippingAddressId { get; set; }
        public string Description { get; set; }
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
        public String OwnerName
        {
            get
            {
                if (this.UserModel != null && this.UserModel.FullName != null)
                    return this.UserModel.FullName;
                else
                    return string.Empty;
            }
        }

        public String LeadName
        {
            get
            {
                if (this.LeadModel != null && this.LeadModel.Name != null)
                    return this.LeadModel.Name;
                else
                    return string.Empty;
            }
        }

        private LeadModel _LeadModel;
        [ScriptIgnore]
        public virtual LeadModel LeadModel
        {
            get
            {
                if (this._LeadModel == null)
                    this._LeadModel = new LeadModel();
                return this._LeadModel;
            }
            set { this._LeadModel = value; }
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

        private ICollection<ProductQuoteAssociationModel> _ProductQuoteAssociationModels;
        public virtual ICollection<ProductQuoteAssociationModel> ProductQuoteAssociationModels
        {
            get
            {
                if (this._ProductQuoteAssociationModels == null)
                    this._ProductQuoteAssociationModels = new List<ProductQuoteAssociationModel>();
                return this._ProductQuoteAssociationModels;
            }
            set { this._ProductQuoteAssociationModels = value; }
        }
    }
}
