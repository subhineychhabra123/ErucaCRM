using ErucaCRM.Utility.WebClasses;
using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.ResponseModel
{
    public class ResponseActivity : ResponseStatus
    {
        [DataMember]
        public TaskItem taskItem { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDL> OwnerList { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<DDL> StatusList { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<DDL> PriorityList { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<DDL> TaskTypeList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDLEncrypt> TaskAssignedToList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDL> AssociatedModules { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<HomeRecentActivites> RecentActivities { get; set; }
         [DataMember(EmitDefaultValue = false)]
        public List<TaskItem> Activities { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DashboardData> DashboardData { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Notification> GetNotification { get; set; }
        [DataMember]
        public int MaxLeadAuditId { get; set; }
        [DataMember]
        public int TotalNotificationRecords { get; set; }

    }
}