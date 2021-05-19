using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    public class Contact
    {
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string ContactId { get; set; }
        [DataMember]
        public string  LeadId { get; set; }
        public string AccountId { get; set; }
        [DataMember]
        public string OwnerName { get; set; }
        [DataMember]
        public virtual String FirstName { get; set; }
        [DataMember]
        public virtual String LastName { get; set; }
        public virtual String EmailAddress { get; set; }
        [DataMember]
        public virtual String Phone { get; set; }
        [DataMember]
        public string ContactCompanyName { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public virtual String HomePhone { get; set; }
        [DataMember]
        public virtual String OtherPhone { get; set; }
        [DataMember]
        public string JobPosition { get; set; }
        [DataMember]
        public Nullable<int> AddressId { get; set; }
    }
}