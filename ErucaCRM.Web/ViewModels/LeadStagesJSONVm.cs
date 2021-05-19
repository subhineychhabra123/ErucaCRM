using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    public class LeadStagesJSONVm
    {

        public string StageName { get; set; }
        public List<LeadVM> Leads { get; set; }
        public string StageId { get; set; }
        public bool IsInitialStage { get; set; }
        public bool IsLastStage { get; set; }
        public int TotalRecords { get; set; }
    
    }
}