using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    [DataContract]
    public class Lead
    {
        [DataMember]
        public string LeadId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string FinalStageId { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string LeadCompanyName { get; set; }

        public string RatingImage { get; set; }
        [DataMember]
        public Nullable<decimal> Amount { get; set; }
        [DataMember]
        public Nullable<int> LeadOwnerId { get; set; }
        [DataMember]
        public string LeadOwnerName { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal ExpectedRevenue { get; set; }
        [DataMember]
        public int RatingConstant { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public bool? ContactExists { get; set; }
        [DataMember]
        public bool IsClosedWin { get; set; }
        [DataMember]
        public bool IsLeadClosed { get; set; }
    }
}