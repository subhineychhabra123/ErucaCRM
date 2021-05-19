using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        private CompanyModel _companyModel;
        public virtual CompanyModel CompanyModel
        {
            get
            {
                if (this._companyModel == null)
                    this._companyModel = new CompanyModel();
                return this._companyModel;
            }
            set { this._companyModel = value; }
        }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        private AddressModel _address;
        public AddressModel AddressModel
        {
            get
            {
                if (this._address == null)
                {
                    this._address = new AddressModel();
                }
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

        private RoleModel _role;
        public RoleModel RoleModel
        {
            get
            {
                if (this._role == null)
                {
                    this._role = new RoleModel();
                }
                return this._role;
            }
            set
            {
                this._role = value;
            }
        }
        public int RoleId { get; set; }
        private ProfileModel _profile;
        public ProfileModel ProfileModel
        {
            get
            {
                if (this._profile == null)
                {
                    this._profile = new ProfileModel();
                }
                return this._profile;
            }
            set
            {
                this._profile = value;
            }
        }
        private CultureInformationModel __cultureInformation;
        public CultureInformationModel CultureInformationModel
        {
            get
            {
                if (this.__cultureInformation == null)
                {
                    this.__cultureInformation = new CultureInformationModel();
                }
                return this.__cultureInformation;
            }
            set
            {
                this.__cultureInformation = value;
            }

        }

        private TimeZoneModal _timeZone;
        public TimeZoneModal TimeZoneModel
        {
            get
            {
                if (this._timeZone == null)
                {
                    this._timeZone = new TimeZoneModal();
                }
                return this._timeZone;
            }
            set
            {
                this._timeZone = value;
            }

        }

        public string ImageURL { get; set; }

        public int UserTypeId { get; set; }
        public int ProfileId { get; set; }
        private string _fullName;
        public string FullName
        {
            get
            {
                _fullName = CommonFunctions.ConcatenateStrings(this.FirstName, this.LastName);
                return _fullName;
            }
            set { _fullName = value; }
        }
        public int LeadOwnerId { get; set; }
        public Nullable<int> TimeZoneId { get; set; }

        public Nullable<int> CultureInformationId { get; set; }
        public string tokenId { get; set; }

    }
    //public class CompanyModel
    //{
    //    public string CompanyName { get; set; }
    //    public int CompanyId { get; set; }

    //}
}
