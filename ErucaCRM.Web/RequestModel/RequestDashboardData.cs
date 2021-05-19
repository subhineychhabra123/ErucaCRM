using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.RequestModel
{
    public class RequestDashboardData : RequestListInfo
    {
        [DataMember]
        public string Interval { get; set; }
        public string UserId { get; set; }
    }
}