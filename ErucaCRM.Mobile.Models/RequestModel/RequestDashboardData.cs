using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.RequestModel
{
    public class RequestDashboardData : RequestListInfo
    {
        [DataMember]
        public string Interval { get; set; }
        public string UserId { get; set; }
    }
}