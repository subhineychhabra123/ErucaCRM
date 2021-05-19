using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.Web.Models
{
    public partial class LeadComment : RequestListInfo
    {
        [DataMember]
        public string LeadCommentId { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public string LeadId { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string AudioFileName { get; set; }
        [DataMember]
        public string AudioFileDuration { get; set; }
        [DataMember]
        public string Audiofiledata { get; set; }


        public virtual User User { get; set; }
        public virtual Lead Lead { get; set; }
    }
}