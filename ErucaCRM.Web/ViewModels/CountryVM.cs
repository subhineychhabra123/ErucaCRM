using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Country")]
    public class CountryVM
    {
        public int CountryId { get; set; }
        [Display(Name = "Country")]
        public string CountryName { get; set; }
        public int OtherCountryId { get; set; }
    }
}