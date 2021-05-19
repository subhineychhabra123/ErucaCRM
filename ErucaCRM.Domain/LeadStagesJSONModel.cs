using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class LeadStagesJSONModel
    {
        public string StageName { get; set; }
        public List<LeadModel> Leads { get; set; }
        public int StageId { get; set; }
        public string StageOrder { get; set; }
        public bool IsInitialStage { get; set; }
        public bool IsLastStage { get; set; }
        public int TotalRecords { get; set; }
    }
}
