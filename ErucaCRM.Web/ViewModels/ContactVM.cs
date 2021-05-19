using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Domain;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Contact")]
    public class ContactVM : BaseModel
    {
        public string ContactId { get; set; }

        /// <summary>
        /// Gets or sets the OwnerId value.
        /// </summary>
        public Int32 OwnerId { get; set; }
        public String OwnerName { get; set; }
        public string NewTagNames { get; set; }
        public string ContactTagIds { get; set; }
        public string JobPosition { get; set; }
        /// <summary>
        /// Gets or sets the LeadSourceId value.
        /// </summary>
        public Int32 LeadSourceId { get; set; }
        [Display(Name = "Contact Owner")]
        public String ContactOwner { get; set; }
        /// <summary>
        /// Gets or sets the FirstName value.
        /// </summary>
        [Required(ErrorMessage = "Contact.FirstNameRequired")]
        [Display(Name = "First Name")]
        public String FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName value.
        /// </summary>
        [Display(Name = "Last Name")]
        public String LastName { get; set; }

        /// <summary>
        /// Gets or sets the DOB value.
        /// </summary>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress value.
        /// </summary>
       // [Required(ErrorMessage = "Contact.EmailRequired")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Contact.EmailRequired")]
        [EmailAddress(ErrorMessage = "Contact.InvalidEmailAddress")]
        [DataType(DataType.EmailAddress)]
        public String EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets the Phone value.
        /// </summary>
        //[Phone(ErrorMessage = "Invalid phone no.")]
        public String Phone { get; set; }

        /// <summary>
        /// Gets or sets the HomePhone value.
        /// </summary>
        public String HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the OtherPhone value.
        /// </summary>
        public String OtherPhone { get; set; }

        /// <summary>
        /// Gets or sets the Mobile value.
        /// </summary>
        //[Phone(ErrorMessage = "Invalid mobile phone no.")]
        public String Mobile { get; set; }

        /// <summary>
        /// Gets or sets the Fax value.
        /// </summary>
        public String Fax { get; set; }

        /// <summary>
        /// Gets or sets the Department value.
        /// </summary>

        public String Department { get; set; }
        [Display(Name = "Company Name")]
        public string ContactCompanyName { get; set; }
        /// <summary>
        /// Gets or sets the Assistant value.
        /// </summary>
        public String Assistant { get; set; }

        /// <summary>
        /// Gets or sets the ReportsTo value.
        /// </summary>
        public String ReportsTo { get; set; }

        /// <summary>
        /// Gets or sets the AddressId value.
        /// </summary>
        public Int32? AddressId { get; set; }

        /// <summary>
        /// Gets or sets the AsstPhone value.
        /// </summary>
        public String AsstPhone { get; set; }

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Gets or sets the Tags value.
        /// </summary>
        public String Tags { get; set; }

        /// <summary>
        /// Gets or sets the CompanyId value.
        /// </summary>
        public Int32 CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public Int32 CreatedBy { get; set; }

        public string AccountId { get; set; }
        public string LeadId { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy value.
        /// </summary>
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate value.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the RecordDeleted value.
        /// </summary>
        public virtual Boolean RecordDeleted { get; set; }
        private string contactName;
        public string ContactName
        {

            get
            {
                return ((this.FirstName ?? "") + " " + (this.LastName ?? ""));

            }
            set
            {
                this.contactName = value;
            }
        }

        public virtual ICollection<ContactTagModel> ContactTags { get; set; }
        private ProfileAddressVM _address;
        public virtual ProfileAddressVM Address
        {
            get
            {
                if (this._address == null)
                    this._address = new ProfileAddressVM();
                return this._address;
            }
            set { this._address = value; }
        }
        private UserVM _user;
        public virtual UserVM User
        {
            get
            {
                if (this._user == null)
                    this._user = new UserVM();
                return this._user;
            }
            set { this._user = value; }
        }
        public virtual ICollection<FileAttachmentVM> FileAttachments { get; set; }
        //Grid Specific Properties



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
        public DropDownItem DropDownItems
        {
            get
            {
                return new DropDownItem();
            }

        }


        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }

        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }
        public string GetMessage()
        {

            return "";
        }

        public class DropDownItem
        {
            public string AllContacts { get; set; }
            public string MyContacts { get; set; }
            public string ThisWeekContacts { get; set; }
            public string LastWeekContacts { get; set; }
        }

        public class GridHeader
        {
            //Contact List
            public string ContactName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Company { get; set; }
            [Display(Name = "Contact.FirstName")]
            public string FirstName { get; set; }
            public string LastName { get; set; }  
            public string OwnerName { get; set; }
            //Contact View 
          
            public string FileName { get; set; }
            public string AttachedBy { get; set; }
            public string ErrorDescription { get; set; }
        }

        public class PageSubHeader
        {
            public string PageHeaderContactManager { get; set; }
            public string PageSubHeaderContactInfo { get; set; }
            public string PageSubHeaderTags { get; set; }
            public string PageSubHeaderAddressInfo { get; set; }
            public string PageSubHeaderAttachments { get; set; }
            public string PageSubHeaderActivities { get; set; }
            public string PageSubHeaderAddActivity { get; set; }

            public string PageSubHeaderAttachFile { get; set; }
            public string PageSubHeaderUpload { get; set; }

        }

        public class PageButton
        {
            public string ButtonAddTag { get; set; }
            public string ButtonAddActivity { get; set; }
        
            public string ButtonContactTagSearchName { get; set; }

            public string ButtonDownloadSampleFile { get; set; }
            public string ButtonBulkUpload { get; set; }
            
            public string ButtonAttach { get; set; }

            public string ButtonUpload { get; set; }
            public string LinkButtonCloseTagMenu { get; set; }

            public string DeleteButtonToopTip { get; set; }
            public string EditButtonToolTip { get; set; }
            public string ButtonTagSearchName { get; set; }
           
        }

        public class PageLabel
        {

            public string TagFilteredByText { get; set; }
            public string EnterTagPlaceHolder { get; set; }
            public string SearchByTagPlaceHolder { get; set; }
            public string OwnerName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

        }

    }



    public class Title
    {
        public string TitleName { get; set; }
    }


    public class ContactInfo
    {
        public string FilterBy { get; set; }
        public int CurrentPageNo { get; set; }
        public Boolean IsSearchByTag { get; set; }
        public string SearchTags { get; set; }
        public string AccountId { get; set; }
        public string LeadId { get; set; }
    }

}