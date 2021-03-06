
using ErucaCRM.Mobile.Models.RequestModel;
using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.ResponseModel
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

        [DataMember]
        public List<LeadStagesJSON> GetLeadsByStageGroupformobile { get; set; }

        [DataMember]
        public UserProfile User { get; set; }
        
        [DataMember]
        public string APICulture
        {
            get;
            set;
        }
       
        [DataMember(EmitDefaultValue = false)]
        public List<TaskItem> AllTaskItem { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Contact> AllContactList { get; set; }

        [DataMember]
        public LeadComment leadComment { get; set; }

        //[DataMember]
        //public Dictionary<string, bool> PermissionList { get; set; }
    }
}