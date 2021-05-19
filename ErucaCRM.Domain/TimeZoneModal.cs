using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class TimeZoneModal
    {
        public int TimeZoneId { get; set; }
        public string TimeZone_Location { get; set; }
        public string GMT { get; set; }
       public string offset { get; set; }
    }
}
