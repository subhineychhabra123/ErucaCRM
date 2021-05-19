using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
 public   class AccountSettingModel
    {
        public int AccountSettingId { get; set; }
        public int UserId { get; set; }
        public bool IsSendNotificationsRecentActivities { get; set; }
        public virtual UserModel User { get; set; }
    }
}
