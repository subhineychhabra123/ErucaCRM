using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class ResponseTimeandCulture 
    {
        public List<TimeZone> TimeZone { get; set; }
        public List<CultureInformation> CultureInformation { get; set; }
    }
}