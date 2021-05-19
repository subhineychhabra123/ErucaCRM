using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Registration")]
    public class RegistrationVM : BaseModel
    {
        [Required(ErrorMessage = "Registration.FirstNameRequired")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Registration.LastNameRequired")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public CompanyVM Company { get; set; }
        [Required(ErrorMessage = "Registration.EmailRequired")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Registration.InvalidEmailAddress")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Registration.PasswordRequired")]
        [DataType(DataType.Password)]
        //[StringLength(15, MinimumLength = 3)]
        [Display(Name = "Password")]
        public string Password { get; set; }  
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Registration.PasswordConfirmPasswordDoesNotMatches")]
        public string ConfirmPassword { get; set; }
        public string TimeZoneId { get; set; }
        public string CultureInformationId { get; set; }

        public string CultureSpecificPageContent { get; set; }
        public string CultureSpecificPageMetaTitle { get; set; }
        public string CultureSpecificPageMetaTags { get; set; }

        public PageLabel PageLabels
        {
            get { return new PageLabel(); }

        }




        public PageButton PageButtons
        {
            get { return new PageButton(); }

        }



        public class PageLabel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string CompanyName { get; set; }
            public string EmailId { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string Culture { get; set; }
            public string TimeZone { get; set; }
        }



        public class PageButton
        {
            public string ButtonRegister { get; set; }
            public string LinkButtonAlreadyHaveAnAccount { get; set; }
            public string LinkButtonSignUpNow { get; set; }
            public string LinkButtonForgotPassword { get; set; }

        }


    }

}