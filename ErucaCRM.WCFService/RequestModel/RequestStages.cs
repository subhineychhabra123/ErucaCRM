using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.WCFService.RequestModel
{
    public class RequestStages : RequestListInfo
    {
        public int StageId { get; set; }
        public int RatingId { get; set; }
        public bool IsInitialStage { get; set; }
        public bool IsLastStage { get; set; }
    }
}