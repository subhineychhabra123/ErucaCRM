using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    public class TimeZoneVM
    {
        public string TimeZoneId { get; set; }
        public string TimeZone_Location { get; set; }
        public string GMT { get; set; }
        public string TimeZoneDescription
        {

            get
            {
                if (!String.IsNullOrEmpty(TimeZone_Location) && !String.IsNullOrEmpty(GMT))
                {
                    return TimeZone_Location + " : " + GMT;
                }
                else return String.Empty;
            }



        }
    }
}