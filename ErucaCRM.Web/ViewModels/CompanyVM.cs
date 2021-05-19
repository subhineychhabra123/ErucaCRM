using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Company")]
    public class CompanyVM
    {
        public string CompanyId { get; set; }
        [Required(ErrorMessage = "Registration.CompanyNameRequired")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
[Display(Name = "Status")]
        public bool IsActive { get; set; }
    }
}