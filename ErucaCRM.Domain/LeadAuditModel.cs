using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class LeadAuditModel
    {
        public int LeadAuditId { get; set; }
        public String StageName { get; set; }
        public string RatingIcon { get; set; }
        public string Title { get; set; }
        public Nullable<bool> IsClosedWin { get; set; }
        public Nullable<bool> IsClosed { get; set; }
        public Nullable<int> LeadId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> StageId { get; set; }
        public Nullable<int> RatingId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public virtual RatingModel Rating { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public System.DateTime FromDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string Duration { get; set; }
        public Nullable<decimal> Proability { get; set; }
        public Nullable<decimal> ExpectedRevenue { get; set; }
        public int TotalNumberOfMinutes { get; set; }
        public Nullable<int> ActivityType { get; set; }
        public int StageDurationPercentage
        {
            get
            {
                if (TotalNumberOfMinutes > 0)
                    return ((this.NumberOfMinutes * 100) / TotalNumberOfMinutes);
                else return 0;

            }
        }

        public int NumberOfMinutes { get; set; }
    }
}
