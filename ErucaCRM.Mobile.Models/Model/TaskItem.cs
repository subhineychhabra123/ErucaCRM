using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    [DataContract]
    public class TaskItem
    {
        [DataMember]
        public string TaskId { get; set; }
        [DataMember]
        public String Subject { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string DueDate { get; set; }
        [DataMember]
        public String OwnerName { get; set; }
        [DataMember]
        public String TaskType { get; set; }
        [DataMember]
        public String TaskAssociatedPerson { get; set; }
        [DataMember]
        public string PriorityName { get; set; }
        [DataMember]
        public string OwnerId { get; set; }
        [DataMember]
        public int PriorityId { get; set; }
        [DataMember]
        public string TaskStatus { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public int AssociatedModuleId { get; set; }
        [DataMember]
        public string AssociatedModuleValue { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserImage { get; set; }
        [DataMember]
        public string AudioFileName { get; set; }
        [DataMember]
        public string AudioFileDuration { get; set; }
         [DataMember]
        public string Audiofiledata { get; set; }
         [DataMember]
         public string AudioStatus { get; set; }
         [DataMember]
         public string CultureName { get; set; }
    }
}