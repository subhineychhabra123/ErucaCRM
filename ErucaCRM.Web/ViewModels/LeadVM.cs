using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Lead")]
    public class LeadVM : BaseModel
    {
        public string LeadId { get; set; }
        public string StageId { get; set; }
        public string Abbreviation { get; set; }
        public string FinalStageId { get; set; }
        [Display(Name = "Lead Owner")]
        public Nullable<int> LeadOwnerId { get; set; }
        public string LeadOwnerName
        {
            get;
            set;
        }
        public string LeadOwnerImage
        {
            get;
            set;
        }
        public Nullable<Decimal> Amount { get; set; }
        [Required(ErrorMessage = "Lead.LeadTitleRequired")]
        public string Title { get; set; }


        public string RatingId { get; set; }
        public StageVM Stage { get; set; }
        public string ExpectedLeadRevenue
        {
            get
            {
                return ExpectedRevenue.ToString("N2");
            }
        }
        public Decimal ExpectedRevenue
        {
            get;
            set;
        }
        public string AccountId { get; set; }

        public Nullable<int> IndustryId { get; set; }
        public string Phone { get; set; }
        [StringLength(75)]
        public string LeadCompanyName { get; set; }
        public virtual ICollection<LeadContactVM> LeadContact { get; set; }
        public string Mobile { get; set; }

        public Nullable<int> LeadSourceId { get; set; }

        public Nullable<int> LeadStatusId { get; set; }
        public string Description { get; set; }

        public string LeadCreatedTime
        {
            get
            {

                return CreatedDate == null ? string.Empty : CreatedDate.Value.ToDateTimeNow().ToLongDateString();

            }
            set { }
        }
        public string FileName { get; set; }
        public string AudioPath { get { return string.Concat(ReadConfiguration.SiteUrl,ReadConfiguration.LeadDocumentPath, this.FileName); } }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string NewTagNames { get; set; }
        public string LeadTagIds { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public Nullable<bool> IsClosedWin { get; set; }
        public virtual ICollection<LeadTagVM> LeadTags { get; set; }
        private List<RatingVM> _RatingVMList;
        public virtual List<RatingVM> RatingList
        {
            get
            {
                if (this._RatingVMList == null)
                    this._RatingVMList = new List<RatingVM>();
                return this._RatingVMList;
            }
            set { this._RatingVMList = value; }
        }



        public string ContactEmail { get; set; }

        public string Company { get; set; }

        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        public virtual List<TaskItemVM> TaskItems { get; set; }

        public virtual RatingVM Rating { get; set; }
        public virtual IndustryVM Industry { get; set; }
        public virtual LeadSourceVM LeadSource { get; set; }
        public virtual LeadStatusVM LeadStatus { get; set; }
        public virtual LeadUserVM User { get; set; }
        public virtual ICollection<FileAttachmentVM> FileAttachments { get; set; }
        public virtual ICollection<AssociatedProductVM> AssociatedProducts { get; set; }
        public virtual ICollection<ProductLeadAssociationVM> ProductLeadAssociations { get; set; }
        public virtual ICollection<ProductVM> AllProducts { get; set; }
        public virtual ICollection<QuoteVM> Quotes { get; set; }
        public virtual ICollection<InvoiceVM> Invoices { get; set; }
        public virtual ICollection<SalesOrderVM> SalesOrders { get; set; }
        //Property related to grid

        public virtual ICollection<LeadContactVM> AccountContact { get; set; }

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }

        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }

        public PageSubTab PageSubTabs
        {
            get
            {
                return new PageSubTab();
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

        public class PageLabel
        {
            public string Rating { get; set; }
            public string SearchByLeadTitlePlaceHolder { get; set; }
            public string EnterTagPlaceHolder { get; set; }
            public string SearchByTagPlaceHolder { get; set; }
            public string TagFilteredByText { get; set; }
            public string Title { get; set; }
            public string Tag { get; set; }
            public string FilterByLabel { get; set; }
            public string ContactName { get; set; }
            public string ContactEmail { get; set; }
            public string FilterTag { get; set; }
            public string FilterLeads { get; set; }
            public string FilterLeadByName { get; set; }
            public string FilterMenuHeader { get; set; }
        }

        public class GridHeader
        {
            public string LeadName { get; set; }
            public string Company { get; set; }
            public string Contact { get; set; }
            public string Phone { get; set; }
            //View Lead Detail Document Grid
            public string FileName { get; set; }
            public string AttachedBy { get; set; }
            public string ContactName { get; set; }
            public string Detail { get; set; }
            public string JobPosition { get; set; }
            public string Mobile { get; set; }
            //Products Grid

            public string ProductName { get; set; }
            public string ProductCode { get; set; }

            //Quote/Sale Order/Invoice -- Grids
            public string Subject { get; set; }
            public string GrandTotal { get; set; }


            public string Email { get; set; }

            public string GridHeaderStage { get; set; }
            public string GridHeaderAmount { get; set; }
            public string GridHeaderProbability { get; set; }
            public string GridHeaderExpectedRevenue { get; set; }
            public string GridHeaderClosingDate { get; set; }
            public string GridHeaderStageDuration { get; set; }
            public string GridHeaderModifiedTime { get; set; }
            public string DeletedStageInfo { get; set; }
            public string TaskStatus { get; set; }
            public string Priority { get; set; }
            public string ActivityType { get; set; }
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
            public string ButtonRating { get; set; }
            public string ButtonUpload { get; set; }
            public string ButtonMoveStage { get; set; }

            public string ButtonAddContact { get; set; }
            public string ButtonRemoveDocument { get; set; }
            public string ButtonAddLead { get; set; }
            public string ButtonCancel { get; set; }
            public string ButtonLoadMore { get; set; }
            public string ButtonAddTag { get; set; }
            public string ButtonTagSearchName { get; set; }
            public string ButtonSearch { get; set; }
            public string ButtonAddComment { get; set; }
            public string AddCommentPlaceholder { get; set; }
            public string ButtonFilterLeads { get; set; }
            public string ButtonFilterStages { get; set; }
            public string ButtonArchivedLeads { get; set; }
            public string ButtonClose { get; set; }
            public string ButtonBack { get; set; }
            public string ButtonClearFilter { get; set; }
            public string ButtonApplyFilter { get; set; }
        }

        public class PageSubHeader
        {

            public string PageSubHeaderAddContact { get; set; }
            public string PageSubHeaderLeadInfo { get; set; }
            public string PageSubHeaderAddressInfo { get; set; }
            public string PageSubHeaderDescription { get; set; }
            public string PageSubHeaderAttachments { get; set; }
            public string PageSubHeaderActivities { get; set; }
            public string PageSubHeaderAddActivity { get; set; }
            public string PageSubHeaderAttachFile { get; set; }
            public string PageSubHeaderProducts { get; set; }
            public string PageSubHeaderAddProduct { get; set; }
            public string PageSubHeaderQuotes { get; set; }
            public string PageSubHeaderSalesOrders { get; set; }
            public string PageSubHeaderInvoices { get; set; }
            public string PageSubHeaderProductCreate { get; set; }
            public string PageSubHeaderContacts { get; set; }
            public string PageSubHeaderLeadHistory { get; set; }
            public string PageSubHeaderSearchTagOption { get; set; }
        }

        public class PageSubTab
        {
            public string PageSubtabGeneral { get; set; }
            public string PageSubtabHistory { get; set; }
            public string PageSubtabActivity { get; set; }
            public string PageSubtabDocument { get; set; }
            public string PageSubtabContact { get; set; }
            public string PageSubtabComments { get; set; }
            public string PageSubtabProducts { get; set; }
        }


    }
}