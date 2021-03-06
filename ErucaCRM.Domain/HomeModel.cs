using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class HomeModel
    {
       
        public int LeadAuditId { get; set; }
        public Nullable<int> LeadId { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<int> RatingId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Icons { get; set; }
        public Nullable<bool> IsClosedWin { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<int> ActivityType { get; set; }
        public string ImageURL { get; set; }
        public string StageName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDisplay { get; set; }
        // for dashboard graphs properties 

        public Nullable<double> TotalLead { get; set; }
        public Nullable<double> WinLead { get; set; }
        public Nullable<double> LostLead { get; set; }
        public Nullable<double> ClosedLead { get; set; }
        public Nullable<double> TotalRevenue { get; set; }
        public Nullable<double> LeadsInStages { get; set; }
        public Nullable<double> TotalSale { get; set; }
        public Nullable<decimal> SalePercentage { get; set; }
        public Nullable<decimal> ClosedLeadPercentage { get; set; }
        public Nullable<double> NewClient { get; set; }
        public Nullable<decimal> NewClientPercentage { get; set; }
        public Nullable<decimal> WinLeadPercentage { get; set; }
        public Nullable<decimal> LostLeadPercentage { get; set; }



    }
}
