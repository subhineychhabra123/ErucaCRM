using System;
using System.Collections.Generic;
using System.Linq;

namespace ErucaCRM.Mobile.Models.ResponseModel
{
    public class ResponseTimeandCulture 
    {
        public List<TimeZone> TimeZone { get; set; }
        public List<CultureInformation> CultureInformation { get; set; }
    }
}