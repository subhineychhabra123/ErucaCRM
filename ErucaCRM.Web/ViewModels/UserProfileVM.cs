using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "User")]
    public class UserProfileVM : BaseModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "User.FirstNameRequired")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public CompanyVM Company { get; set; }
        [Required(ErrorMessage = "User.EmailRequired")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "User.InvalidEmailAddress")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
        private ProfileAddressVM _address;
        public ProfileAddressVM Address
        {
            get
            {
                if (this._address == null)
                {
                    this._address = new ProfileAddressVM();
                }
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }
        public string AddressDetails { get; set; }
        private string _profileImageUrl = string.Empty;
        private RoleVM _role;
        public RoleVM Role
        {
            get
            {
                if (this._role == null)
                {
                    this._role = new RoleVM();
                }
                return this._role;
            }
            set
            {
                this._role = value;
            }
        }
        private ProfileVM _profile;
        public ProfileVM Profile
        {
            get
            {
                if (this._profile == null)
                {
                    this._profile = new ProfileVM();
                }
                return this._profile;
            }
            set
            {
                this._profile = value;
            }
        }
        public string ImageURL
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._profileImageUrl) ? "no_image.gif" : this._profileImageUrl;
            }
            set { this._profileImageUrl = value; }
        }

        public bool IsCurrentUser
        {
            get
            {
                return this.UserId == ErucaCRM.Utility.SessionManagement.LoggedInUser.UserId.Encrypt();
            }
        }

        public string TimeZoneId { get; set; }
        public string CultureInformationId { get; set; }
        private CultureInformationVM __cultureInformation;
        public CultureInformationVM CultureInformationVM
        {
            get
            {
                if (this.__cultureInformation == null)
                {
                    this.__cultureInformation = new CultureInformationVM();
                }
                return this.__cultureInformation;
            }
            set
            {
                this.__cultureInformation = value;
            }

        }

        private TimeZoneVM _timeZone;
        public TimeZoneVM TimeZoneVM
        {
            get
            {
                if (this._timeZone == null)
                {
                    this._timeZone = new TimeZoneVM();
                }
                return this._timeZone;
            }
            set
            {
                this._timeZone = value;
            }

        }

        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }

        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }

        public class GridHeaders
        {

        }


        public class PageSubHeader
        {
            public string PageSubHeaderContactInfo { get; set; }
            public string PageSubHeaderChangePassword { get; set; }
         

        }

        public class PageLabel
        {
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
            public string ConfirmPassword { get; set; }
            public string TimeZone { get; set; }
            public string Culture { get; set; }
            public string Address { get; set; }
        }

        public class PageButton
        {
            public string ButtonChangePassword { get; set; }
            public string ButtonUpload { get; set; }


        }
     

    }
}