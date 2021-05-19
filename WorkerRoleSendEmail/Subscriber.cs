using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using System.ComponentModel.DataAnnotations;
namespace WorkerRoleSendEmail
{
    public class Subscriber : TableEntity
    {
        [Required]
        [Display(Name = "List Name")]
        public string ListName
        {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }

        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress
        {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }

        public string SubscriberGUID { get; set; }

        public bool? Verified { get; set; }
    }
}