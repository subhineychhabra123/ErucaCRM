using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ErucaCRM.Mobile.Models.RequestModel
{
    public class RequestLead : RequestListInfo
    {
        public string UserId { get; set; }
        public string StageId { get; set; }
        public string LeadId { get; set; }
        public bool? IsWinStage { get; set; }
        public string Title { get; set; }
        public string LeadCompanyName { get; set; }
        public bool? IsClosedWin { get; set; }
        public int? ModifiedBy { get; set; }
        public int?     RatingId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> LeadOwnerId { get; set; }
        public string Description { get; set; }
        public int DocId { get; set; }
        public string filedata { get; set; }
        public string FileName { get; set; }
        public string FileDuration { get; set; }
        public string AudioStatus { get; set; }
        //public long DocumentId { get; set; }
        }
    }
