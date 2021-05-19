using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    [DataContract]
    public class Account
    {
        [DataMember]
        public string AccountId { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string ParentAccountName { get; set; }
        public Nullable<int> ParentAccountId { get; set; }
        public string AccountOwnerId { get; set; }
        [DataMember]
        public string AccountOwner { get; set; }
        public Nullable<int> AccountTypeId { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        [DataMember]
        public string AccountTypeName { get; set; }
    }
}