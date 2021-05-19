using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErucaCRM.Mobile.Models.RequestModel
{
    public class RequestStages : RequestListInfo
    {
        public int StageId { get; set; }
        public int RatingId { get; set; }
        public bool IsInitialStage { get; set; }
        public bool IsLastStage { get; set; }
    }
}