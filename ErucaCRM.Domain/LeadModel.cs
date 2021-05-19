using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ErucaCRM.Domain
{
    public class LeadModel
    {
        public int LeadId { get; set; }
        public int FinalStageId { get; set; }
        public string Abbreviation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private string _name;
        public string Name
        {
            get
            {
                _name = CommonFunctions.ConcatenateStrings(this.FirstName, this.LastName);
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public Nullable<int> LeadOwnerId { get; set; }
        public string Title { get; set; }
        public string EmailId { get; set; }
        public Nullable<int> NumborOfEmployee { get; set; }
        public Nullable<int> IndustryId { get; set; }
        public string Phone { get; set; }
        public string LeadCompanyName { get; set; }
        public string Mobile { get; set; }
        public Nullable<int> LeadSourceId { get; set; }
        public Nullable<int> LeadStatusId { get; set; }
        public string SkypeId { get; set; }
        public string NewTagNames { get; set; }
        public string LeadTagIds { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string Description { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<int> RatingId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ContactId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual ICollection<LeadTagModel> LeadTagsModels { get; set; }
        public bool IsLeadConvertedToAccount { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileDuration { get; set; }
        public bool IsLeadClosed
        {
            get
            {

                return this.ClosingDate != null;
            }
        }

        public Nullable<bool> IsClosedWin { get; set; }
        public decimal ExpectedRevenue
        {
            get
            {
                decimal percentage = 0;
                decimal amount = this.Amount ?? 0;
                if (this.RatingId != null)
                {
                    percentage = this.Rating.ExpectedRevenuePercentage;
                }
                else
                {
                    percentage = this.Stage.Rating.ExpectedRevenuePercentage;
                }
                return (percentage * amount) / 100;
            }
        }
        public bool RecordDeleted { get; set; }
        public string RatingImage { get; set; }
        public virtual List<ContactModel> Contacts { get; set; }
        public virtual List<TaskItemModel> TaskItems { get; set; }
        public virtual List<FileAttachmentModel> DocumentFiles { get; set; }
        private AddressModel _address;
        public AddressModel AddressModel
        {
            get
            {
                if (this._address == null)
                {
                    this._address = new AddressModel();
                }
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

        private IndustryModel _IndustryModel;
        public virtual IndustryModel IndustryModel
        {
            get
            {
                if (this._IndustryModel == null)
                    this._IndustryModel = new IndustryModel();
                return this._IndustryModel;
            }
            set { this._IndustryModel = value; }
        }
        private LeadSourceModel _LeadSourceModel;
        public virtual LeadSourceModel LeadSourceModel
        {
            get
            {
                if (this._LeadSourceModel == null)
                    this._LeadSourceModel = new LeadSourceModel();
                return this._LeadSourceModel;
            }
            set { this._LeadSourceModel = value; }
        }

        private LeadStatusModel _LeadStatusModel;
        public virtual LeadStatusModel LeadStatusModel
        {
            get
            {
                if (this._LeadStatusModel == null)
                    this._LeadStatusModel = new LeadStatusModel();
                return this._LeadStatusModel;
            }
            set { this._LeadStatusModel = value; }
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
        private RatingModel _RatingModel;
        public virtual RatingModel Rating
        {
            get
            {
                if (this._RatingModel == null)
                    this._RatingModel = new RatingModel();
                return this._RatingModel;
            }
            set { this._RatingModel = value; }
        }
        private List<RatingModel> _RatingModelList;
        public virtual List<RatingModel> RatingList
        {
            get
            {
                if (this._RatingModelList == null)
                    this._RatingModelList = new List<RatingModel>();
                return this._RatingModelList;
            }
            set { this._RatingModelList = value; }
        }
        private StageModel _StageModel;
        public virtual StageModel Stage
        {
            get
            {
                if (this._StageModel == null)
                    this._StageModel = new StageModel();
                return this._StageModel;
            }
            set { this._StageModel = value; }
        }
        private ICollection<FileAttachmentModel> _FileAttachmentModels;
        public virtual ICollection<FileAttachmentModel> FileAttachments { get; set; }
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
        private ICollection<ProductLeadAssociationModel> _ProductLeadAssociationModels;
        public virtual ICollection<ProductLeadAssociationModel> ProductLeadAssociationModels
        {
            get
            {
                if (this._ProductLeadAssociationModels == null)
                    this._ProductLeadAssociationModels = new List<ProductLeadAssociationModel>();
                return this._ProductLeadAssociationModels;
            }
            set { this._ProductLeadAssociationModels = value; }
        }
        public virtual ICollection<QuoteModel> QuoteModels { get; set; }
        public virtual ICollection<InvoiceModel> InvoiceModels { get; set; }
        public virtual ICollection<SalesOrderModel> SalesOrderModels { get; set; }
        public virtual ICollection<LeadContactModel> LeadContactModel { get; set; }
        public virtual ICollection<TaskItemModel> LeadTaskModel { get; set; }
    }
}
