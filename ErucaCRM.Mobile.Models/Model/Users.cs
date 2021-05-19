using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    [DataContract]
    public class Users
    {
        [DataMember]
        public string UserIdEncrypted { get; set; }
        [DataMember]
        public string UserName { get; set; }
    }
}