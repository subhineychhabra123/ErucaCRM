using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
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
        public bool? IsClosedWin { get; set; }
        [DataMember]
        public bool IsLeadClosed { get; set; }
        [DataMember]
        public List<Contact> LeadContactModel { get; set; }
        [DataMember]
        public List<TaskItem> TaskItems { get; set; }
        [DataMember]
        public virtual List<FileAttachment> FileAttachments { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FileDuration { get; set; }
        [DataMember]
        public string DocumentPath { get; set; }
        [DataMember]
        public long DocumentId { get; set; }
    }
}