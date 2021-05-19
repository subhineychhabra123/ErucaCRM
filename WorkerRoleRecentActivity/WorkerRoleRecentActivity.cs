using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Text;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Business;
using ErucaCRM.Utility;
using ErucaCRM.Repository;
using ErucaCRM.Domain;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using WorkerRoleRecentActivity.Infrastructure;


namespace WorkerRoleRecentActivity
{
    public class WorkerRoleRecentActivity : RoleEntryPoint
    {
        private readonly RecentActivitiesAlert recentActivitiesAlert = new RecentActivitiesAlert();
        public static string CultureName;
        public static string TimeZoneOffSet;
        public static int UserId;
        public CloudQueue sendEmailQueue;
        /// <summary>
        /// 24 hour milli seconds
        /// </summary>
        public const int milliseconds = ((1000 * 3600) * 24) - 60000 * 15;
        public override void Run()
        {
            
            try
            {
                Trace.TraceInformation("Worker Role RecentActivity start run() {0}", DateTime.Now);
                recentActivitiesAlert.GetRecentActivitiesEmailData();
                Trace.TraceInformation("Worker Role RecentActivity going to sleep {0}", DateTime.Now);
                Thread.Sleep(milliseconds);
                Trace.TraceInformation("Worker Role RecentActivity end to sleep {0}", DateTime.Now);
            }
            catch (Exception ex)
            {
                Trace.TraceInformation("WorkerRoleRecentActivity Error.");
            }

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
        public override bool OnStart()
        {
            ConfigureDiagnostics();
            AutoMapperNotificationManager.Run();
            WorkerRoleCommonFunctions.GetAllCultureObject();
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            Trace.TraceInformation("Worker Role RecentActivity has been started");
            return result;
        }
        public override void OnStop()
        {
            Trace.TraceInformation("Worker Role RecentActivity is stopping");
            base.OnStop();
            Trace.TraceInformation("Worker Role RecentActivity has stopped");
        }


    }
}
