using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Login")]
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Login.EmailRequired")]
        [Display(Name = "Email Id")]
        [EmailAddress(ErrorMessage = "Login.InvalidEmailAddress")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        public string CultureSpecificPageContent { get; set; }
        public string CultureSpecificPageMetaTitle { get; set; }
        public string CultureSpecificPageMetaTags { get; set; }

        public PageHeader PageHeaders
        {
            get { return new PageHeader(); }
        }
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
            public string Email { get; set; }

        }



        public class PageButton
        {
            public string ButtonRecoverForgotPassword { get; set; }

        }

        public class PageHeader
        {
            public string PageHeaderRecoverYourPassword { get; set; }


        }
    }
}