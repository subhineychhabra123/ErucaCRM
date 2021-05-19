using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    [DataContract]
    public class LeadAudit
    {

        public int LeadAuditId { get; set; }
        [DataMember]
        public String StageName { get; set; }
        [DataMember]
        public Nullable<decimal> Amount { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ClosingDate { get; set; }
        [DataMember]
        public Nullable<decimal> Proability { get; set; }
        [DataMember]
        public Nullable<decimal> ExpectedRevenue { get; set; }
        [DataMember]
        public string Duration { get; set; }
    }
}