using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    [DataContract]
    public class FileAttachment
    {
        [DataMember]
        public long DocumentId { get; set; }
        [DataMember]
        public string DocumentName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public Nullable<long> AttachedBy { get; set; }
        [DataMember]
        public string DocumentPath { get; set; }
        public Nullable<int> UserId { get; set; }
        public string AccountId { get; set; }
        public string TaskId { get; set; }
        public string LeadId { get; set; }
        public string ContactId { get; set; }
        public string AccountCaseId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public string FileDuration { get; set; }

    }
}