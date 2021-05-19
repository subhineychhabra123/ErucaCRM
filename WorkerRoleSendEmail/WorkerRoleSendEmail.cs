using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using ErucaCRM.Utility;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;

namespace WorkerRoleSendEmail
{
    /// <summary>
    /// Worker Role for sending Email
    /// </summary>
    public class WorkerRoleSendEmail : RoleEntryPoint
    {
        private CloudQueue sendEmailQueue;       
        private CloudTable mailingListTable;
        private CloudTable messageTable;
        private CloudTable messagearchiveTable;
        private volatile bool onStopCalled = false;
        private volatile bool returnedFromRunMethod = false;
        public static string CultureName;
        public static string TimeZoneOffSet;
        public static int UserId;
        /// <summary>
        /// Check the message table if found sends to sendMail function()
        /// </summary>
        public override void Run()
        {
            CloudQueueMessage msg = null;

            Trace.TraceInformation("Worker Role SendEmail start of Run() {0}", DateTime.Now);
            while (true)
            {
                try
                {
                    bool messageFound = false;
                    if (onStopCalled == true)
                    {
                        Trace.TraceInformation("onStopCalled WorkerRoleSendEmail");
                        returnedFromRunMethod = true;
                        return;
                    }
                    msg = sendEmailQueue.GetMessage();
                    if (msg != null)
                    {
                        ProcessQueueMessage(msg);
                        messageFound = true;
                    }

                    // Retrieve and process a new message from the subscribe queue.
                    //msg = subscribeQueue.GetMessage();
                    //if (msg != null)
                    //{
                    //    ProcessSubscribeQueueMessage(msg);
                    //    messageFound = true;
                    //}

                    if (messageFound == false)
                    {
                        //System.Threading.Thread.Sleep(1000 * 60);
                    }
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    if (ex.InnerException != null)
                    {
                        err += " Inner Exception: " + ex.InnerException.Message;
                    }
                    if (msg != null)
                    {
                        err += " Last queue message retrieved: " + msg.AsString;
                    }
                    Trace.TraceError(err);
                    System.Threading.Thread.Sleep(1000 * 60);
                }
            }
        }
        /// <summary>
        /// gets Role Instances
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetRoleInstance()
        {
            string instanceId = RoleEnvironment.CurrentRoleInstance.Id;
            int instanceIndex = -3;
            int.TryParse(instanceId.Substring(instanceId.LastIndexOf("_") + 1), out instanceIndex);
            return instanceIndex;
        }
        /// <summary>
        /// Process Queue message
        /// </summary>
        /// <param name="msg"></param>
        private void ProcessQueueMessage(CloudQueueMessage msg)
        {
            if (msg.DequeueCount > 5)
            {
                Trace.TraceError("Deleting poison message:    message {0} Role Instance {1}.",
                    msg.ToString(), GetRoleInstance());
                sendEmailQueue.DeleteMessage(msg);
                return;
            }
            var messageParts = msg.AsString.Split(new char[] { ',' });
            var partitionKey = messageParts[0];
            var rowKey = messageParts[1];
            //var restartFlag = messageParts[2];
            Trace.TraceInformation("ProcessQueueMessage start:  partitionKey {0} rowKey {1} Role Instance {2}.",
                partitionKey, rowKey, GetRoleInstance());
            // Get the row in the Message table that has data we need to send the email.
            var retrieveOperation = TableOperation.Retrieve<SendEmail>(partitionKey, rowKey);
            var retrievedResult = messageTable.Execute(retrieveOperation);
            var emailRowInMessageTable = retrievedResult.Result as SendEmail;
            if (emailRowInMessageTable == null)
            {
                Trace.TraceError("SendEmail row not found:  partitionKey {0} rowKey {1} Role Instance {2}.",
                    partitionKey, rowKey, GetRoleInstance());
                return;
            }

            var htmlMessageBodyRef = emailRowInMessageTable.MessageBody;
            var textMessageBodyRef = emailRowInMessageTable.MessageBody;
            if (emailRowInMessageTable.EmailSent != true)
            {
                SendEmail(emailRowInMessageTable, htmlMessageBodyRef, textMessageBodyRef);

                var emailRowToDelete = new SendEmail { PartitionKey = partitionKey, RowKey = rowKey, ETag = "*" };
                emailRowInMessageTable.EmailSent = true;

                var upsertOperation = TableOperation.InsertOrReplace(emailRowInMessageTable);
                messagearchiveTable.Execute(upsertOperation);
                var deleteOperation = TableOperation.Delete(emailRowToDelete);
                messageTable.Execute(deleteOperation);
                Trace.TraceError("Row deleted from message Deleted found:  partitionKey {0} rowKey {1} Role Instance {2}.",
                    partitionKey, rowKey, GetRoleInstance());
            }

            // Delete the queue message.
            sendEmailQueue.DeleteMessage(msg);

            Trace.TraceInformation("ProcessQueueMessage complete:  partitionKey {0} rowKey {1} Role Instance {2}.",
               partitionKey, rowKey, GetRoleInstance());
        }

        /// <summary>
        /// Sends listed Emails  
        /// </summary>
        /// <param name="emailRowInMessageTable"></param>
        /// <param name="htmlMessageBodyRef"></param>
        /// <param name="textMessageBodyRef"></param>
        private void SendEmail(SendEmail emailRowInMessageTable, string htmlMessageBodyRef, string textMessageBodyRef)
        {
            MailHelper mailService = new MailHelper();
            string to = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            subject = emailRowInMessageTable.SubjectLine;
            body = htmlMessageBodyRef;
            to = emailRowInMessageTable.EmailAddress;
           // to = "waris.ali@sensationsolutions.com";
            try
            {
               mailService.SendMailMessage(to, subject, body);

            }
            catch (Exception ex)
            {

            }
        }
        ///// <summary>
        ///// configure the Role including the connection string
        ///// </summary>
        private void ConfigureDiagnostics()
        {
            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.Logs.BufferQuotaInMB = 500;
            config.Logs.ScheduledTransferLogLevelFilter = Microsoft.WindowsAzure.Diagnostics.LogLevel.Information;
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);

            DiagnosticMonitor.Start(
                "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString",
                config);
        }
        /// <summary>
        /// calls to stop the Role for 1000 milliseconds
        /// </summary>
        public override void OnStop()
        {
            onStopCalled = true;
            while (returnedFromRunMethod == false)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// runs at the start of the instance
        /// </summary>
        /// <returns></returns>
        public override bool OnStart()
        {

            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 12;
            ConfigureDiagnostics();
            Trace.TraceInformation("Initializing storage account in Worker Role SendEmail");
            // Modified by Xiang Gao
            // Use CloudConfigurationManager to get config settings, don't use RoleEnvironment
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            Trace.TraceInformation("Creating queue client.");
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            sendEmailQueue = queueClient.GetQueueReference("azuremailqueue");
            // Initialize table storage
            var tableClient = storageAccount.CreateCloudTableClient();
            mailingListTable = tableClient.GetTableReference("mailinglist");
            messageTable = tableClient.GetTableReference("message");
            messagearchiveTable = tableClient.GetTableReference("messagearchive");

            sendEmailQueue.CreateIfNotExists();
            this.messageTable.CreateIfNotExists();
            this.mailingListTable.CreateIfNotExists();
            this.messagearchiveTable.CreateIfNotExists();

            return base.OnStart();
        }
    }
}
