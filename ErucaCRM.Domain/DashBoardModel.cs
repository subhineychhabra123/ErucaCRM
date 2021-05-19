using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class DashBoardModel
    {
        int TotalLead { get; set; }
        float LeadIncrementPercentage { get; set; }
        int LeadWin { get; set; }
        int LeadLost { get; set; }
        float WinLeadIncrementPer { get; set; }
        int leadClose { get; set; }
        float LeadClosePercentage { get; set;} 
    }
}
