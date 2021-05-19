using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.RequestModel
{
    public class RequestTaskItem : RequestListInfo
    {
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string LeadAuditId { get; set; }
        [DataMember]
        public bool IsLoadMore { get; set; }

        public  int MaxLeadAuditId { get; set; }
        public bool UpdateNotification { get; set; }
        
    }
}