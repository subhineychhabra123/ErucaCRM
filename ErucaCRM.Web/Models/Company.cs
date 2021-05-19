using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    [DataContract]
    public class Company
    {
        
        public string CompanyName { get; set; }
         [DataMember]
        public int CompanyId { get; set; }

    }
}