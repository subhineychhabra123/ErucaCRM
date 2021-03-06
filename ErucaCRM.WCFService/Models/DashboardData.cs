using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class DashboardData
    {
        [DataMember]
        public Nullable<double> TotalLead { get; set; }
        [DataMember]
        public Nullable<double> NewClient { get; set; }
        [DataMember]
        public Nullable<double> WinLead { get; set; }
        [DataMember]
        public Nullable<double> LostLead { get; set; }
        [DataMember]
        public Nullable<double> ClosedLead { get; set; }
        [DataMember]
        public Nullable<double> TotalRevenue { get; set; }
        [DataMember]
        public Nullable<double> LeadsInStages { get; set; }
    }
}