using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    //  [Authorize(Module= Constants.AUTHENTICATION_ROLE_ADMIN)]
    [CultureModuleAttribute(ModuleName = "User")]
    public class UserVM : BaseModel
    {
        public string ModuleName
        {

            get
            {
                return "User";
            }
        }
        public string UserId { get; set; }
        [Required(ErrorMessage = "User.FirstNameRequired")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }



        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public CompanyVM Company { get; set; }
        [Required(ErrorMessage = "User.EmailRequired")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "User.InvalidEmailAddress")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        private ProfileAddressVM _address;
        [CultureModuleAttribute(ModuleName = "Address")]
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
        public bool IsCurrentUser
        {
            get
            {
                return this.UserId == ErucaCRM.Utility.SessionManagement.LoggedInUser.UserIdEncrypted;
            }
        }
        [Required(ErrorMessage = "User.RoleNameRequired")]
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
        [Display(Name = "Name")]
        public string FullName
        {
            get
            {
                return CommonFunctions.ConcatenateStrings(this.FirstName, this.LastName);
            }
        }


        public PageLabel PageLabels
        {
            get { return new PageLabel(); }
        }

        public static class GridHeaders
        {



        }


        public static class PageSubHeadrs
        {
            public static string ContactInformation { get; set; }


        }


        public int UserTypeId { get; set; }
        public int ProfileId { get; set; }
        public string ImageURL { get; set; }
        public bool IsSelected { get; set; }
        public string TimeZoneId { get; set; }
        public string CultureInformationId { get; set; }


    }

    public class PageLabel
    {
        public string TimeZone { get; set; }
        public string Culture { get; set; }

    }
    public class UseListInfo
    {  
        public string UserID { get; set; }
        public int CurrentPage { get; set; }
        public string UserStatus { get; set; }
        public string CompanyId { get; set; }
    }

    public class CompanyListInfo
    {
        public string CompanyId { get; set; }
        public int CurrentPage { get; set; }
        public string CompanyStatus { get; set; }

    }
}