using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class DasshboardLeadModel
    {
       
        int TotalLead { get; set; }
        int NewClient { get; set; }
        int WinLead { get; set; }
        int LostLead { get; set; }
        int ClosedLead { get; set; }
        int TotalRevenue { get; set; }
    }
}
