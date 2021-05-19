using System;
using System.Collections.Generic;
using System.Linq;

namespace ErucaCRM.Mobile.Models.Model
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