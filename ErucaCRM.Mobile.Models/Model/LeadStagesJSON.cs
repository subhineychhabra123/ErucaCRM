using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    [DataContract]
    public class LeadStagesJSON
    {
        [DataMember]
        public string StageName { get; set; }
        [DataMember]
        public List<Lead> Leads { get; set; }
        [DataMember]
        public string StageId { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
        public string StageOrder { get; set; }
        [DataMember]
        public bool IsInitialStage { get; set; }
        [DataMember]
        public bool IsLastStage { get; set; }


    }
}