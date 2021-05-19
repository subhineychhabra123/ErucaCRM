using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class AddressModel
    {
        public  int? AddressId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string OtherStreet { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherZipCode { get; set; }
        public Nullable<int> OtherCountryId { get; set; }
        private CountryModel _country;
        public CountryModel CountryModel {
            get
            {
                if (this._country == null)
                {
                    this._country = new CountryModel();
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
