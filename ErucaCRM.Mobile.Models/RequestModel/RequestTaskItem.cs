using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.RequestModel
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