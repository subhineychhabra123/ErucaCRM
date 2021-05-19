using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace WorkerRoleDelayLead
{
    public class LeadEmailNotificationModel : ITableEntity
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
        public DateTimeOffset Timestamp { get; set; }
        public string ETag { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        //public string ReadEntity { get; set; }
        //public IDictionary<string, EntityProperty> WriteEntity { get; set; }


        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            throw new NotImplementedException();
        }
    }
}
