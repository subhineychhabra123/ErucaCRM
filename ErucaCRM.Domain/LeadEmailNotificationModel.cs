using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class LeadEmailNotificationModel
    {

        public int UserId { get; set; }
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string LeadIds { get; set; }
        public string CultureName { get; set; }
       //Proreties for saving Email Data to Table -SendEmailNotification
       //---Start---
        public int NotificationId { get; set; }
        public string LogData { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool HasError { get; set; }
        public int LogType { get; set; }
       
        //---End---
    }

}
