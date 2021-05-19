using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Address")]
    public class ProfileAddressVM
    {
        public int AddressId { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Zip Code")]
        public string Zipcode { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string OtherStreet { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherZipCode { get; set; }
        public Nullable<int> OtherCountryId { get; set; }
        private CountryVM _country;
        public CountryVM Country
        {
            get
            {
                return this._country;
            }
            set
            {
                this._country = value;
            }
        }
    }
}