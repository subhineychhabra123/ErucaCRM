using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public class LeadAuditVM
    {
        public int LeadAuditId { get; set; }
        public string StageName { get; set; }
        public int ActivityType { get; set; }
        public string Title { get; set; }
        public string RatingIcon { get; set; }
        public Nullable<bool> IsClosedWin { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public Nullable<int> LeadId { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<int> RatingId { get; set; }
        public string StageId_encrypted { get; set; }
        public string LeadId_encrypted { get; set; }
        public string RatingId_encrypted { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OwnerName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
        public string CultureSpcificAmount
        {
            get
            {
                return Amount==null?"0":Amount.Value.ToString("N2");
            }
        }
        public Nullable<System.DateTime> ClosingDate { get; set; }
      
        //private string activityText;
        public string ActivityText
        {

            get
            {
                if (ActivityType == (int)Enums.ActivityType.LeadAdded)
                {
                    return Convert.ToString(Enums.ActivityType.LeadAdded);
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadAmountChanged)
                {
                    return Convert.ToString(Enums.ActivityType.LeadAmountChanged);
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadRatingChanged)
                {
                    return Convert.ToString(Enums.ActivityType.LeadRatingChanged);
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadStageChanged)
                {
                    return Convert.ToString(Enums.ActivityType.LeadStageChanged);
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadDeleted)
                {
                    return Convert.ToString(Enums.ActivityType.LeadDeleted);
                }
                else
                {
                    return "";
                }
            }
        }

        public string HistoryActivityType
        {
            get
            {
                if (ActivityType == (int)Enums.ActivityType.LeadAdded)
                {
                    return "";
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadAmountChanged)
                {
                    return CommonFunctions.GetGlobalizedLabel("Lead", "LeadAmountChanged");
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadRatingChanged)
                {
                    return CommonFunctions.GetGlobalizedLabel("Lead", "LeadRatingChanged");
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadStageChanged)
                {
                    return CommonFunctions.GetGlobalizedLabel("Lead", "LeadStageChanged");
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadDeleted)
                {
                    return CommonFunctions.GetGlobalizedLabel("Lead", "LeadDeleted");
                }
                else
                {
                    return "";
                }
            }
        }
        public string LeadClosingDate
        {
            get
            {
                return this.ClosingDate != null ? this.ClosingDate.Value.ToShortDateString() : "";
            }
        }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime FromDate { get; set; }
        public string LeadStageFromDate
        {
            get
            {
                return this.FromDate != null ? this.FromDate.ToShortDateString() : "";
            }
        }
    
        public virtual RatingVM Rating { get; set; }
      
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Duration { get; set; }
        public Nullable<decimal> Proability { get; set; }
        public Nullable<decimal> ExpectedRevenue { get; set; }
        public int TotalNumberOfMinutes { get; set; }
        public int StageDurationPercentage { get; set; }
        public int NumberOfMinutes { get; set; }
    }
}