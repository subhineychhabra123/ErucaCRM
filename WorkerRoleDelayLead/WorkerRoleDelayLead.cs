using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Business;
using ErucaCRM.Utility;
using ErucaCRM.Utility.WebClasses;
using System.Text;
using ErucaCRM.Domain;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;
using WorkerRoleDelayLead.Infrastructure;

namespace WorkerRoleDelayLead
{
    /// <summary>
    /// Worker Role for retriving email data from sql server and saving them to Message Table for sendEmail Worker Role
    /// </summary>
    public class WorkerRoleDelayLead : RoleEntryPoint
    {

        static IUnitOfWork unitOfWork = new UnitOfWork();
        LeadNotifcationBusiness objLeadNotifcationBusiness = new LeadNotifcationBusiness(unitOfWork);
        MailHelper objmailhelper = new MailHelper();
        MailBodyTemplate mailBodyTemplate = new MailBodyTemplate();
        StringBuilder logdata = new StringBuilder("CompanyId, UserId, Status, Message");
        DateTime startdate = DateTime.Now;
        bool hasError = false;
        public static string EmailBody;
        public static string CultureName;
        public static string TimeZoneOffSet;
        public static int UserId;
        public CloudQueue sendEmailQueue;
        public CloudTable messageTable;
        /// <summary>
        /// 24 hour milli seconds
        /// </summary>
        public const int milliseconds = ((1000 * 3600) * 24) - 60000 * 15;
        /// <summary>
        /// start Run
        /// </summary>
        public override void Run()
        {

            try
            {
                Trace.TraceInformation("Worker Role DelayLead start of Run() {0}", DateTime.Now);
                sendDelayLeadNotification();
                Trace.TraceInformation("Worker Role DelayLead is going to sleep {0}", DateTime.Now);
                Thread.Sleep(milliseconds);
                Trace.TraceInformation("Worker Role DelayLead is end to sleep {0}", DateTime.Now);

            }


            catch (Exception ex)
            {
                Trace.TraceInformation("Worker Role DelayLead Error.");
            }


        }
        /// <summary>
        /// for saing delay lead email data in message table
        /// </summary>
        public void sendDelayLeadNotification()
        {
            Trace.TraceInformation("Worker Role DelayLead sendDelayLeadNotification() called {0}", DateTime.Now);
            try
            {
                try
                {
                    List<int> objallcompany = objLeadNotifcationBusiness.GetAllActiveCompanies();
                    string culture = "";
                    string UserName = "";
                    int StageDuration = 0;
                    foreach (int companyId in objallcompany)
                    {
                        IList<ErucaCRM.Domain.LeadEmailNotificationModel> objLeadNotificationModel;
                        try
                        {
                            objLeadNotificationModel = objLeadNotifcationBusiness.GetCompanyDataForEmail(companyId);
                        }
                        catch (Exception exception)
                        {
                            hasError = true;
                            logdata.Append("\n");
                            logdata.Append("," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exception.Message + ".Error Occured while Fetching Company data for Email.");
                            continue;
                        }
                        for (int emailNotificationIndex = 0; emailNotificationIndex < objLeadNotificationModel.Count; emailNotificationIndex++)
                        {
                            culture = objLeadNotificationModel[emailNotificationIndex].CultureName;
                            UserName = objLeadNotificationModel[emailNotificationIndex].Name;
                            WorkerRoleDelayLead.UserId = objLeadNotificationModel[emailNotificationIndex].UserId;
                            string[] LeadsData = objLeadNotificationModel[emailNotificationIndex].LeadIds.Split('|');
                            string currentStageName = "";
                            string leadId = "";
                            string dayDifference = "";
                            string title = "";
                            string stageId = "";
                            string stageName = "";
                            bool loopReachedEnd = false;
                            StringBuilder objstringbuilder = new StringBuilder();
                            for (int leadDataIndex = 0; leadDataIndex < LeadsData.Count(); leadDataIndex++)
                            {
                                string[] LeadSeparated = LeadsData[leadDataIndex].Split(',');
                                if (checkStageLeadData(LeadSeparated.Count()))
                                {
                                    leadId = LeadSeparated[0];
                                    dayDifference = LeadSeparated[1];
                                    title = LeadSeparated[2];
                                    stageId = LeadSeparated[3];
                                    stageName = LeadSeparated[4];
                                    int stageIdForLead = Convert.ToInt32(stageId);
                                    LeadNotifcationBusiness leadNotificationBusiness = new LeadNotifcationBusiness(unitOfWork);
                                    try
                                    {
                                        StageDuration = leadNotificationBusiness.GetStageLeadDuration(stageIdForLead);
                                    }
                                    catch (Exception exception)
                                    {
                                        hasError = true;
                                        logdata.Append("\n");
                                        logdata.Append("," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exception.Message + "." + "Error Occured while Fetching Lead data for StageDuration.");
                                        continue;
                                    }
                                    try
                                    {
                                        if (currentStageName != stageName)
                                        {
                                            if (currentStageName != "") loopReachedEnd = true;
                                            else loopReachedEnd = false;
                                            if (currentStageName != stageName && loopReachedEnd)
                                                objstringbuilder.Append("</ul></div>");
                                            currentStageName = stageName;
                                            {
                                                objstringbuilder.Append("<div style='background-color:mintcream;border:1px solid Grey;border-radius:5px; float:left; margin-right:10px; margin-top:30px; width:800px; box-shadow:0 0 3px #666; padding:5px;'>");
                                                objstringbuilder.Append("<p><b>" + CommonFunctions.GetGlobalizedLabel("Lead", "StageName", culture) + " :</b>" + currentStageName + " <i> (" + CommonFunctions.GetGlobalizedLabel("Lead", "MaxDuration", culture) + ":" + StageDuration + CommonFunctions.GetGlobalizedLabel("Lead", "Days", culture) + ") </i></p><br/><b style='float:left'>" + CommonFunctions.GetGlobalizedLabel("Lead", "Leads", culture) + "</b>");
                                                objstringbuilder.Append("<ul style='float:left'><li><b><a href='" + ReadConfiguration.ErucaCRMURL + "#" + Convert.ToInt32(leadId).Encrypt() + "'> " + title + " </a></b><i> (" + CommonFunctions.GetGlobalizedLabel("Lead", "TotalTimeSpent", culture) + " :" + dayDifference + CommonFunctions.GetGlobalizedLabel("Lead", "Days", culture) + ") </i></li>");
                                                //  objstringbuilder.Append("<p><b>" + "Test Check" + " :</b>" + currentStageName + " <i> (" + "Test Check" + ":" + StageDuration + "Test Check" + ") </i></p><br/><b style='float:left'>" + "Test Check" + "</b>");
                                                // objstringbuilder.Append("<ul style='float:left'><li><b><a href='" + ReadConfiguration.ErucaCRMURL + "#" + Convert.ToInt32(leadId).Encrypt() + "'> " + title + " </a></b><i> (" + "Test Check" + " :" + dayDifference + "Test Check" + ") </i></li>");
                                            }
                                        }
                                        else
                                        {
                                            objstringbuilder.Append("<li><b><a href='" + ReadConfiguration.ErucaCRMURL + "#" + Convert.ToInt32(leadId).Encrypt() + "'>" + title + "</a></b><i> (" + CommonFunctions.GetGlobalizedLabel("Lead", "TotalTimeSpent", culture) + " :" + dayDifference + CommonFunctions.GetGlobalizedLabel("Lead", "Days", culture) + ") </i></li>");
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        hasError = true;
                                        logdata.Append("\n");
                                        logdata.Append(companyId + "," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exception.Message + "." + "Error Occured While Fetching Data From EXCEL File or Configuration File.");
                                    }
                                }
                            }
                            objstringbuilder.Append("</ul></div>");

                            objmailhelper.ToAddress = objLeadNotificationModel[emailNotificationIndex].EmailId;
                            objmailhelper.RecipientName = objLeadNotificationModel[emailNotificationIndex].Name;
                            objmailhelper.Subject = Constants.LEADS_TIMEEXCEED_NOTIFICATION_SUBJECT;
                            try
                            {
                                objmailhelper.Body = "<p>" + CommonFunctions.GetGlobalizedLabel("Lead", "Hi", culture) + " " + UserName + ",</p><br/><br/><p>" + CommonFunctions.GetGlobalizedLabel("Lead", "EmailTopMsg", culture) + ":</br></br>";
                                objmailhelper.Body = objmailhelper.Body + objstringbuilder.ToString() + "<div style='clear:both;'></div><div style='width:700px;margin-top:20px;'><p>Regards</p></BR><P>Administration</P></div>";
                                logdata.Append("\n");
                                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                                CloudConfigurationManager.GetSetting("StorageConnectionString"));
                                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                                CloudTable table = tableClient.GetTableReference("Message");
                                table.CreateIfNotExists();
                                Message message = new Message();
                                AutoMapper.Mapper.Map(objmailhelper, message);
                                EmailBody = message.Body;
                                var sendEmailRow = new SendEmail
                                {
                                    PartitionKey = message.RecipientName,
                                    RowKey = message.ToAddress,
                                    EmailAddress = message.ToAddress,
                                    EmailSent = false,
                                    MessageBody = message.Body,
                                    ScheduledDate = DateTime.Now,
                                    FromEmailAddress = ReadConfiguration.EmailForScheduler,
                                    SubjectLine = message.Subject,
                                };
                                try
                                {
                                    Trace.TraceInformation("Worker Role DelayLead saved data in message table {0}", DateTime.Now);
                                    TableOperation insertOperation = TableOperation.InsertOrReplace(sendEmailRow);
                                    table.Execute(insertOperation);
                                }
                                catch (Exception ex)
                                {
                                    string err = "Error creating SendEmail row:  " + ex.Message;
                                    if (ex.InnerException != null)
                                    {
                                        err += " Inner Exception: " + ex.InnerException;
                                    }
                                    Trace.TraceError(err);
                                }
                                string queueMessageString =
                                sendEmailRow.PartitionKey + "," +
                                sendEmailRow.RowKey + ",";
                                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

                                var queueMessage = new CloudQueueMessage(queueMessageString);
                                sendEmailQueue = queueClient.GetQueueReference("azuremailqueue");
                                sendEmailQueue.AddMessage(queueMessage);
                                Trace.TraceInformation("Worker Role DelayLead saved data in queue table {0}", DateTime.Now);
                                logdata.Append(companyId + "," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Success.ToString() + "," + objmailhelper.Subject);
                            }
                            catch (System.Net.Mail.SmtpException exception)
                            {
                                hasError = true;
                                logdata.Append("\n");
                                logdata.Append(companyId + "," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exception.Message + "." + "Error Occured on Sending Email to the the User");
                            }
                            catch (Exception exception)
                            {
                                hasError = true;
                                logdata.Append("\n");
                                logdata.Append(companyId + "," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exception.Message + "." + "Error Occured on Sending Email to the the User");
                            }
                        }
                    }
                    saveEmailData(hasError, logdata, startdate);
                    // await messageTable.ExecuteAsync(returnInsertOperation);
                }
                catch (Exception exception)
                {
                    hasError = true;
                    logdata.Append("\n");
                    logdata.Append("," + WorkerRoleDelayLead.UserId + "," + ErucaCRM.Utility.Enums.ResponseResult.Failure.ToString() + "," + exception.Message + "." + "Error Occured while Fetching Company.");
                    saveEmailData(hasError, logdata, startdate);
                }


            }
            catch (Exception ex)
            {
                //  Add(ex, true);
            }

        }
        /// <summary>
        /// save email data to sql database
        /// </summary>
        /// <param name="hasError"></param>
        /// <param name="logdata"></param>
        /// <param name="startdate"></param>
        public void saveEmailData(bool hasError, StringBuilder logdata, DateTime startdate)
        {
            Trace.TraceInformation("Worker Role DelayLead saved log data in sql server {0}", DateTime.Now);
            ErucaCRM.Domain.LeadEmailNotificationModel leadEmailNotificationModel = new ErucaCRM.Domain.LeadEmailNotificationModel();
            // LeadEmailNotificationModel leadEmailNotificationModel = new LeadEmailNotificationModel();
            leadEmailNotificationModel.HasError = hasError;
            leadEmailNotificationModel.LogData = logdata.ToString();
            leadEmailNotificationModel.LogType = (int)ErucaCRM.Utility.Enums.LogErrorType.DelayLead;
            leadEmailNotificationModel.EndDate = DateTime.Now;
            leadEmailNotificationModel.StartDate = startdate;
            objLeadNotifcationBusiness.SaveEmailNotificationDetail(leadEmailNotificationModel);
            //  var insertOperation = TableOperation.Insert(leadEmailNotificationModel);
            //return insertOperation;
        }
        /// <summary>
        /// calls at start
        /// </summary>
        /// <returns></returns>
        public override bool OnStart()
        {
            ConfigureDiagnostics();
            AutoMapperNotificationManager.Run();
            CommonFunctions.GetAllCultureObject();
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
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
        /// In this we check the mailing data
        /// </summary>
        /// <param name="LeadSeparated"></param>
        /// <returns></returns>
        public static bool checkStageLeadData(int LeadSeparated)
        {
            if (LeadSeparated == 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
