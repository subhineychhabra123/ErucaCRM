using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    public class ErrorMessage
    {
        [DataMember]
        public string Message { get; set; }
        public string StatusCode { get; set; }
    }
}