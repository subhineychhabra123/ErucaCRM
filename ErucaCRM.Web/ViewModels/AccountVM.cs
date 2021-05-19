using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Domain;
using System.ComponentModel.DataAnnotations;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Account")]
    public class AccountVM : BaseModel
    {
        public string AccountId { get; set; }
        [Required(ErrorMessage = "Account.AccountNameRequired")]
        public string AccountName { get; set; }
        public string ParentAccountName { get; set; }
        public Nullable<int> ParentAccountId { get; set; }
        public string AccountOwnerId { get; set; }
        public string AccountOwner { get; set; }
        public Nullable<int> AccountTypeId { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        public string AccountTypeName
        {
            get
            {

                if (this.AccountType != null)
                    return CommonFunctions.GetGlobalizedLabel("DropDowns", this.AccountType.AccountTypeName);
                else
                    return "";
            }
        }
        public Nullable<int> NumberOfEmployee { get; set; }
        public string AccountLocation { get; set; }
        public string AccountWebsite { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string NewTagNames { get; set; }
        public string AccountTagIds { get; set; }
      
        public Nullable<decimal> AnnualRevenue { get; set; }
        [RegularExpression(@"^\$?([0-9]{1,3},([0-9]{3},)*[0-9]{3}|[0-9]+)(.[0-9][0-9])?$", ErrorMessage = "Account.AccountNumericRequired")]
        public string AccountAnnualRevenue  //use this field on your form input
        {
            get { return AnnualRevenue.ToString(); }
            set { AnnualRevenue = decimal.Parse(value); } //assign Amount
        } 
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

        public virtual AccountTypeModel AccountType { get; set; }
        public virtual ICollection<AccountTagModel> AccountTags { get; set; }

        private BillingAddressVM _billingAddress;
        private ShippingAddressVM _shippingAddress;
        public virtual BillingAddressVM Address
        {
            get
            {
                if (this._billingAddress == null)
                {
                    this._billingAddress = new BillingAddressVM();
                }
                return this._billingAddress;
            }
            set
            {
                this._billingAddress = value;
            }
        }
        public virtual ShippingAddressVM Address1
        {
            get
            {
                if (this._shippingAddress == null)
                {
                    this._shippingAddress = new ShippingAddressVM();
                }
                return this._shippingAddress;
            }
            set
            {
                this._shippingAddress = value;
            }
        }

        public virtual CompanyVM Company { get; set; }
        public virtual IndustryVM Industry { get; set; }

        public virtual ICollection<AccountCaseVM> AccountCases { get; set; }
        //public virtual ICollection<AccountCaseVM> AccountCases
        //{
        //    get
        //    {
        //        if (this.AccountCases == null)
        //            this.AccountCases = new List<AccountCaseVM>();
        //        return this.AccountCases;
        //    }
        //    set { this.AccountCases = value; }
        //}
        public virtual ICollection<AccountContactVM> AccountContact { get; set; }

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

        private ICollection<LeadVM> _Leads;
        public virtual ICollection<LeadVM> Leads
        {
            get
            {
                if (this._Leads == null)
                    this._Leads = new List<LeadVM>();
                return this._Leads;
            }
            set { this._Leads = value; }
        }

        public virtual List<AccountVM> ChildAccounts { get; set; }

        public virtual ICollection<SalesOrderVM> SalesOrders { get; set; }




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

        public IList<ParentAccountListModel> ParentAccountList { get; set; }
        public IList<OwnerListModel> AccountOwnerList { get; set; }
        public IList<AccountTypeModel> AccountTypeList { get; set; }
        public IList<IndustryModel> IndustryList { get; set; }
        public IList<CountryVM> CountryList { get; set; }

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
        //public PageToolTip PageToolTips
        //{
        //    get
        //    {
        //        return new PageToolTip();
        //    }

        //}
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
            public string Company { get; set; }
            public string Contact { get; set; }
            public string AccountType { get; set; }
            public string AccountOwner { get; set; }
            public string Email { get; set; }
            public string PhoneNo { get; set; }
            public string ContactCompany { get; set; }

            //Lead Grid

            public string GridHeaderLeadName { get; set; }

            public string GridHeaderLeadOwner { get; set; }

            public string GridHeaderLeadCreatedOn { get; set; }

            //View Lead Detail Document Grid
            public string FileName { get; set; }
            public string AttachedBy { get; set; }

            //Products Grid

            public string ProductName { get; set; }
            public string ProductCode { get; set; }

            //Quote/Sale Order/Invoice -- Grids
            public string Subject { get; set; }
            public string GrandTotal { get; set; }
            public string CaseNumber { get; set; }

            public string CaseOrigin { get; set; }
            public string PriorityName { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
        }

        public class PageButton
        {
            public string ButtonAddQuote { get; set; }
            public string ButtonAddTag { get; set; }
            public string ButtonAccountSearchByTag { get; set; }
            public string ButtonAccountTagSearchName { get; set; }
            public string ButtonAddSalesOrder { get; set; }
            public string ButtonAddAssociatedLead { get; set; }
            public string ButtonAddInvoice { get; set; }

            public string ButtonAddActivity { get; set; }
            public string ButtonAddCase { get; set; }
            public string ButtonAddProduct { get; set; }
            public string ButtonRemoveProduct { get; set; }
            public string ButtonAssociateProduct { get; set; }
            public string ButtonAttach { get; set; }
            public string ButtonAddContact { get; set; }
            public string ButtonUpload { get; set; }
            public string ButtonAddAccount { get; set; }
            public string LinkButtonCloseTagMenu { get; set; }
            public string ButtonEditAssociatedLead { get; set; }
            public string EditButtonToopTip { get; set; }
            public string DeleteButtonToopTip { get; set; }
        }

        public class PageSubHeader
        {
            public string ContactTab { get; set; }
            public string AccountTab { get; set; }
            public string PageSubHeaderAccountInfo { get; set; }
            public string PageSubHeaderTags { get; set; }
            public string PageSubHeaderAddressInfo { get; set; }
            public string PageSubHeaderDescription { get; set; }
            public string PageSubHeaderAttachments { get; set; }
            public string PageSubHeaderActivities { get; set; }
            public string PageSubHeaderAssociatedLeadInfo { get; set; }
            public string PageSubHeaderAddActivity { get; set; }
            public string PageSubHeaderAddContact { get; set; }
            public string PageSubHeaderAttachFile { get; set; }
            public string ButtonAddCase { get; set; }
            public string PageSubHeaderCases { get; set; }
            public string PageSubHeaderProducts { get; set; }
            public string PageSubHeaderAddProduct { get; set; }
            public string PageSubHeaderQuotes { get; set; }
            public string PageSubHeaderSalesOrders { get; set; }
            public string PageSubHeaderInvoices { get; set; }
            public string PageSubHeaderProductCreate { get; set; }
            public string PageHeaderAddContact { get; set; }
            public string PageSubHeaderAccounts { get; set; }
            public string PageSubHeaderCaseInfo { get; set; }
            public string PageSubHeaderShippingAddressInfo { get; set; }
            public string PageSubHeaderBillingAddressInfo { get; set; }
            public string PageSubHeaderAddNewLead { get; set; }
            public string PageSubHeaderAddNewContact { get; set; }
        }

        public class PageLabel
        {
            public string AccountOwner { get; set; }
            public string Tag { get; set; }
            public string ParentAccount { get; set; }
            public string AccountType { get; set; }
            public string Industry { get; set; }
            public string TagFilteredByText { get; set; }
            public string EnterTagPlaceHolder { get; set; }
            public string SearchByTagPlaceHolder { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
            public string Title { get; set; }
            public string OwnerName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
        }
        //public class PageToolTip
        //{
        //    public string EditToopTip { get; set; }
        //    public string DeleteToopTip { get; set; }
        //}
    }
}