using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        [DataMember]
        public string Icons { get; set; }
    }
}