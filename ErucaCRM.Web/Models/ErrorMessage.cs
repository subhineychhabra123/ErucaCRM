using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class ErrorMessage
    {
        [DataMember]
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
}