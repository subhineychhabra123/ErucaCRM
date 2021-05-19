using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.ResponseModel
{
    [DataContract]
    public class ResponseLead : ResponseStatus
    {

        [DataMember(EmitDefaultValue = false)]
        public List<Lead> LeadList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Lead lead { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDL> OwnerList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<LeadAudit> LeadAuditList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<TaskItem> TaskItemList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<FileAttachment> AttachmentList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Account> AccountList { get; set; }

        [DataMember]
        public int TotalRecords { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public List<LeadComment> LeadComment { get; set; }

        


      
      
    }
}