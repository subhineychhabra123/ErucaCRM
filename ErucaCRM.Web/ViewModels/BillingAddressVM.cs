using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
   [CultureModuleAttribute(ModuleName = "BillingAddress")]
    public class BillingAddressVM
    {
        public int AddressId { get; set; }

        [Display(Name = "Billing Street")]
        public string Street { get; set; }
        [Display(Name = "Billing City")]
        public string City { get; set; }
        [Display(Name = "Billing State")]
        public string State { get; set; }
        [Display(Name = "Billing Code")]
        public string Zipcode { get; set; }
        [Display(Name = "Billing Country")]
        public Nullable<int> CountryId { get; set; }
        private CountryVM _country;
        public CountryVM Country
        {
            get
            {
                if (this._country == null)
                {
                    this._country = new CountryVM();
                }
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }
       
    }
}