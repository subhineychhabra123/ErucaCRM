using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class Stages
    {
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string DefaultRatingId { get; set; }
        public bool? IsInitialStage { get; set; }
        public bool? IsLastStage { get; set; }
    }
}