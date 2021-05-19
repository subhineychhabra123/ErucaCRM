using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    public class RatingVM
    {
        public string RatingId { get; set; }
        public string Icons { get; set; }
        public decimal ExpectedRevenuePercentage { get; set; }
        public int RatingConstant { get; set; }
    }
}