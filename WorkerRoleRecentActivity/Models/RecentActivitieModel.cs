using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleRecentActivity
{
    public class RecentActivitieModel
    {

       public string LeadAuditId { get; set; }
        public string LeadId { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<int> RatingId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<int> ActivityType { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string StageName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActivityCreatedTime
        {
            get
            {
                return CreatedDate.ToDateTimeNow().ToString("MMMM dd, yyyy H:mm:ss");
            }
        }
        private string activityText;
        public string ActivityText
        {

            get
            {

               
              
                if (ActivityType == (int)ErucaCRM.Utility.Enums.ActivityType.LeadAdded)
                {
                    return string.Format(WorkerRoleCommonFunctions.GetGlobalizedLabel("DashBoard", "LeadAdded", WorkerRoleRecentActivity.CultureName), " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/Leads/#" + Convert.ToInt32(this.LeadId).Encrypt() + "\">" + this.Title + "</a>", " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/UserProfile/" + Convert.ToInt32(this.CreatedBy).Encrypt() + "\">" + this.FirstName + " " + this.LastName + " </a>");
                }
                else if (ActivityType == (int)ErucaCRM.Utility.Enums.ActivityType.LeadAmountChanged)
                {
                    return string.Format(WorkerRoleCommonFunctions.GetGlobalizedLabel("DashBoard", "LeadAmountChanged", WorkerRoleRecentActivity.CultureName), " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/Leads/#" + Convert.ToInt32(this.LeadId).Encrypt() + "\">" + this.Title + " </a>", " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/UserProfile/" + Convert.ToInt32(this.CreatedBy).Encrypt() + "\">" + this.FirstName + " " + this.LastName + " </a>", this.Amount);
                }
                else if (ActivityType == (int)ErucaCRM.Utility.Enums.ActivityType.LeadRatingChanged)
                {
                    return string.Format(WorkerRoleCommonFunctions.GetGlobalizedLabel("DashBoard", "LeadRatingChanged", WorkerRoleRecentActivity.CultureName), " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/Leads/#" + Convert.ToInt32(this.LeadId).Encrypt() + "\">" + this.Title + " </a>", " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/UserProfile/" + Convert.ToInt32(this.CreatedBy).Encrypt() + "\">" + this.FirstName + " " + this.LastName + " </a>", this.StageName);
                }
                else if (ActivityType == (int)ErucaCRM.Utility.Enums.ActivityType.LeadStageChanged)
                {
                    return string.Format(WorkerRoleCommonFunctions.GetGlobalizedLabel("DashBoard", "LeadStageChanged", WorkerRoleRecentActivity.CultureName), " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/Leads/#" + Convert.ToInt32(this.LeadId).Encrypt() + "\">" + this.Title + " </a>", this.StageName, " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/UserProfile/" + Convert.ToInt32(this.CreatedBy).Encrypt() + "\">" + this.FirstName + " " + this.LastName + " </a>");
                }
                else if (ActivityType == (int)ErucaCRM.Utility.Enums.ActivityType.LeadDeleted)
                {
                    return string.Format(WorkerRoleCommonFunctions.GetGlobalizedLabel("DashBoard", "LeadDeleted", WorkerRoleRecentActivity.CultureName), this.Title, this.StageName, " <a href=\"" + ErucaCRM.Utility.ReadConfiguration.SiteUrl + "User/UserProfile/" + Convert.ToInt32(this.CreatedBy).Encrypt() + "\">" + this.FirstName + " " + this.LastName + " </a>");
                }
                else
                {
                    return "";
                }
            }
        }
     
      
       

     



    
    }
}
