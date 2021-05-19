using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class LeadHistoryChartModel
    {
        public Nullable<int> StageId { get; set; }
        public string StageName { get; set; }
        public int NumberOfMinutes { get; set; }
        public string Duration { get; set; }
        public Nullable<int> TotalNumberOfMinutes { get; set; }
        public int StageDurationPercentage
        {
            get
            {
                if (TotalNumberOfMinutes > 0)
                    return (int)((this.NumberOfMinutes * 100) / TotalNumberOfMinutes);
                else return 0;

            }
        }

        public Nullable<int> StageDays { get; set; }
        public Nullable<int> StageHours { get; set; }
        public Nullable<int> StageMinutes { get; set; }
    }
}
