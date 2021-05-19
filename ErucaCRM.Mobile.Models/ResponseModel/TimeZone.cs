using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.Mobile.Models.ResponseModel
{
    public class TimeZone
    {
        [DataMember]
        public int TimeZoneId { get; set; }
        [DataMember]
        public string TimeZoneDescription { get; set; }
    }
   
}