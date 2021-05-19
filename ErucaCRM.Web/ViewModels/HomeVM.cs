using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Utility;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "DashBoard")]
    public class HomeVM
    {
        public string RatingIcon { get; set; }
        public Nullable<bool> IsClosedWin { get; set; }
        public string LeadAuditId { get; set; }
        public string LeadId { get; set; }
        public string StageId { get; set; }
        public string RatingId { get; set; }
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
        public bool IsDisplay { get; set; }
        public int TotalNotifications { get; set; }
        public string ActivityCreatedTime
        {
            get
            {
                return CreatedDate.ToDateTimeNow().ToString("MMMM dd, yyyy H:mm:ss");
            }
        }
        //private string activityText;
        public string ActivityText
        {

            get
            {
                if (ActivityType == (int)Enums.ActivityType.LeadAdded)
                {
                    return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadAdded"), " <a class='permissionbased notificationpopup' data-showalways='True' data-permission='LeadVe' href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + "</a>", " <a class='permissionbased' data-showalways='True' data-permission='UserVe' href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>");
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadAmountChanged)
                {
                    return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadAmountChanged"), " <a class='permissionbased notificationpopup' data-showalways='True' data-permission='LeadVe' href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + " </a>", " <a class='permissionbased' data-showalways='True' data-permission='UserVe' href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this.Amount);
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadRatingChanged)
                {
                    return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadRatingChanged"), " <a class='permissionbased notificationpopup' data-showalways='True' data-permission='LeadVe' href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + " </a>", " <a class='permissionbased' data-showalways='True' data-permission='UserVe'  href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this.StageName);
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadStageChanged)
                {
                    return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadStageChanged"), " <a class='permissionbased notificationpopup' data-showalways='True' data-permission='LeadVe' href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + " </a>", this.StageName, " <a class='permissionbased' data-showalways='True' data-permission='UserVe'  href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>");
                }
                else if (ActivityType == (int)Enums.ActivityType.LeadDeleted)
                {
                    return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadDeleted"), this.Title, this.StageName, " <a href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>");
                }
                else
                {
                    return "";
                }
            }
        }
     
      
        public Nullable<double> TotalLead { get; set; }
        public Nullable<double> WinLead { get; set; }
        public Nullable<double> LostLead { get; set; }
        public Nullable<double> ClosedLead { get; set; }
        public Nullable<double> TotalRevenue { get; set; }
        public Nullable<double> LeadsInStages { get; set; }
        public Nullable<double> TotalSale { get; set; }
        public string TotalSaleRevenue
        {
            get
            {
                if (TotalSale != null)
                {
                    return TotalSale.Value.ToString("N2");
                }
                return "0" ;
            }
        }
        public Nullable<decimal> SalePercentage { get; set; }
        public Nullable<decimal> ClosedLeadPercentage { get; set; }
        public Nullable<double> NewClient { get; set; }
        public Nullable<decimal> NewClientPercentage { get; set; }
        public Nullable<decimal> WinLeadPercentage { get; set; }
        public Nullable<decimal> LostLeadPercentage { get; set; }

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }



        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }
        public Description Descriptions
        {
            get
            {
                return new Description();
            }

        }
        public class PageSubHeader
        {
            public string Dashboard { get; set; }
            public string DashboardWonLeads { get; set; }
            public string DashboardLostLeads { get; set; }
            public string MyTasks { get; set; }
            public string MyCases { get; set; }
            public string NewLeads { get; set; }
            public string LeadsClosed { get; set; }
            public string ExpectedRevenue { get; set; }
            public string RecentActivities { get; set; }
            public string ViewMoreComments { get; set; }
            public string NoRecordFound { get; set; }
            public string WinLeads { get; set; }
            public string LostLeads { get; set; }
            public string CaseNumber { get; set; }
            public string Subject { get; set; }
            public string Description { get; set; }
            public string Replies { get; set; }
            public string AddComment { get; set; }
            public string AllCases { get; set; }
            public string AllTasks { get; set; }
        }

        public class Description
        {
            public string DescriptionLeadBox { get; set; }
            public string DescriptionContactBox { get; set; }
            public string DescriptionActivityBox { get; set; }
        }

        public class PageButton
        {
            public string ButtonMonthly { get; set; }
            public string ButtonYearly { get; set; }
            public string ButtonOverAll { get; set; }
            public string ButtonWeek { get; set; }
            public string ButtonMonth { get; set; }
            public string ButtonYear { get; set; }
            public string LinkButtonAddLead { get; set; }
            public string LinkButtonAddContact { get; set; }
            public string LinkButtonAddActivity { get; set; }
            public string LinkButtonAddCase { get; set; }
            public string ButtonLoadMore { get; set; }
            public string ButtonRefreshToolTip { get; set; }
            public string ButtonPostComment { get; set; }
            public string ButtonViewAll { get; set; }
            public string ButtonViewAllRecentActivity { get; set; }
            public string ButtonPopClose { get; set; }
        }
    }

}