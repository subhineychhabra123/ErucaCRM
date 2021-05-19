using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    [DataContract]
    public class ResponseStatus
    {
        [DataMember]
        public int Status { get; set; }
         [DataMember]
        public string Message { get; set; }
    }
}