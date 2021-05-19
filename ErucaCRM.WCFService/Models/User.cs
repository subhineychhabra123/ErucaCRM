using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    [DataContract]
    public class User:ResponseStatus
    {

        public int UserId { get; set; }
        [DataMember]
        public string UserIdEncrypted { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string EmailId { get; set; }
        [DataMember]
        public int CultureInformationId { get; set; }
        [DataMember]
        public int TimeZoneId { get; set; }
        [DataMember]
        public string TimeZoneDescription { get; set; }
        [DataMember]
        public string CultureDescription { get; set; }
         [DataMember]
        public bool IsChecked { get; set; }
        [DataMember]
        public int? CompanyId { get; set; }
        private Company _company;
        [DataMember]
        public virtual Company Company
        {
            get
            {
                if (this._company == null)
                    this._company = new Company();
                return this._company;
            }
            set { this._company = value; }
        }
  
    
    
    }
    
}