using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
  public  class RealTimeNotificationModel
    {
      public int RealTimeId { get; set; }
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public bool IsConnected { get; set; }
        public virtual CompanyModel Company { get; set; }
        public virtual UserModel User { get; set; }
    }
}
