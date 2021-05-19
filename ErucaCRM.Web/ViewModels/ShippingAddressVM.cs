using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
   [CultureModuleAttribute(ModuleName = "ShippingAddress")]
    public class ShippingAddressVM
    {
        [Display(Name = "Shipping Street")]
        public string Street { get; set; }
        [Display(Name = "Shipping City")]
        public string City { get; set; }
        [Display(Name = "Shipping State")]
        public string State { get; set; }
        [Display(Name = "Shipping Code")]
        public string Zipcode { get; set; }
        [Display(Name = "Shipping Country")]
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