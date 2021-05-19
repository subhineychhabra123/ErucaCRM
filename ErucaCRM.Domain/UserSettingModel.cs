using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
  public  class UserSettingModel
    {
        public int UserSettingId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> MaxLeadAuditId { get; set; }
        public Nullable<int> TotalNotifications { get; set; }
        public Nullable<bool> IsSendNotificationsRecentActivities { get; set; }
        public Nullable<int> WebMaxLeadAuditId { get; set; }
        public Nullable<int> SchedulerMaxLeadAuditId { get; set; }
        public virtual UserModel User { get; set; }
    }
}
