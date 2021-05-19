using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class DashboarActivitiesModel
    {
        public int TaskId { get; set; }
        public string Subject { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string AudioFileDuration { get; set; }
        public string AudioFileName { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public string PriorityName
        {
            get
            {
                return Enum.GetName(typeof(Utility.Enums.TaskPriority), this.PriorityId);
            }
        }
        public string TaskStatus
        {
            get
            {
                return Enum.GetName(typeof(Utility.Enums.TaskStaus), this.Status);
            }
        }
        public string Description { get; set; }
        public Nullable<int> AssociatedModuleId { get; set; }
        public Nullable<int> AssociatedModuleValue { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ImageURL { get; set; }
        public bool RecordDeleted { get; set; }        
        public virtual UserModel User { get; set; }
    }
}
