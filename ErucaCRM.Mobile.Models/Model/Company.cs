using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    [DataContract]
    public class Company
    {
        
        public string CompanyName { get; set; }
         [DataMember]
        public int CompanyId { get; set; }

    }
}