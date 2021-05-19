using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class LeadByStarRatingPercentageModel
    {
        public string[] StageName { get; set; }
        public string[] RatingName { get; set; }
        public List<int[]> RatingArray { get; set; }

    }
}
