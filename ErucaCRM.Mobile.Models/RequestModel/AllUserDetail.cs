using ErucaCRM.Mobile.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.RequestModel
{
    public class AllUserDetail : RequestListInfo
    {
        public string UserId { get; set; }
        public string LeadAuditId { get; set; }
        public bool IsLoadMore { get; set; }
        public int MaxLeadAuditId { get; set; }
        public bool UpdateNotification { get; set; }
        //DashboardData

        public string Interval { get; set; }
     //   public string UserId { get; set; }
    }
}