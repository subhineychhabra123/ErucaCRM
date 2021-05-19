using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
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