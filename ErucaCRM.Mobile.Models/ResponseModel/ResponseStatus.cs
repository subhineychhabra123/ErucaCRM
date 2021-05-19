using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.ResponseModel
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