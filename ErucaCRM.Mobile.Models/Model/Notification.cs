using System;
using System.Collections.Generic;
using System.Linq;

namespace ErucaCRM.Mobile.Models.Model
{
    public class Notification
    {
        //public Nullable<long> row { get; set; }
        //public int LeadAuditId { get; set; }
        //public Nullable<int> LeadId { get; set; }
        //public Nullable<int> StageId { get; set; }
        //public Nullable<int> RatingId { get; set; }
        //public Nullable<decimal> Amount { get; set; }
        //public Nullable<System.DateTime> ClosingDate { get; set; }
        //public System.DateTime CreatedDate { get; set; }
        //public Nullable<int> CreatedBy { get; set; }
        //public Nullable<int> CompanyId { get; set; }
        //public Nullable<System.DateTime> ToDate { get; set; }
        //public Nullable<System.DateTime> FromDate { get; set; }
        //public Nullable<int> ActivityType { get; set; }
        //public string StageName { get; set; }
        //public string ImageURL { get; set; }
        //public string Title { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        public string LeadAuditId { get; set; }

        public string ToDate { get; set; }

        public string FromDate { get; set; }
        public string ImageURL { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

    }
}