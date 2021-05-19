using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleDelayLead
{
   public class Message :TableEntity
    {
      
       public string ToAddress { get; set; }
       public string Subject { get; set; }
       public string Body { get; set; }
       public string RecipientName { get; set; }
    }
}
