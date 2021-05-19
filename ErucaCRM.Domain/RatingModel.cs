using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class RatingModel
    {
        public int RatingId { get; set; }
        public string Icons { get; set; }
        public int RatingConstant { get; set; }
        public decimal ExpectedRevenuePercentage { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public bool IsLastRating { get; set; }
    }
}
