using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public  class YearWiseLeadModel
    {
        public string Month { get; set; }
        public string Week { get; set; }
        public int LeadCount { get; set; }
        public int CreatedYear { get; set; }
     }
}
