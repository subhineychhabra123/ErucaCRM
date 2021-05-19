using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Login")]
    public class LoginModel:BaseModel
    {
        [Required(ErrorMessage = "Login.EmailRequired")]
        //[EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Login.PasswordRequired")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public bool isChecked { get; set; }
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

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }

        public class PageLabel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ErucaCRMAccount { get; set; }
            public string KeepMeSingnedIn { get; set; }
            public string DontHaveErucaCRMAccount { get; set; }

        }
        public class PageSubHeader
        {
            public string PageSubHeaderLoginInfo { get; set; }
        
        }


        public class PageButton
        {
            public string ButtonSignIn { get; set; }
            public string LinkButtonWhatIsThis { get; set; }
            public string LinkButtonForgotPassword { get; set; }
            public string LinkButtonCannotAccessYourAccount { get; set; }
            public string LinkButtonSignUpNow { get; set; }

        }
    }
}