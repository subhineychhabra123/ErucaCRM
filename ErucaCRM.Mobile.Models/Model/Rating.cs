using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
{
    public class Rating
    {
        public int RatingId { get; set; }
        [DataMember]
        public string Icons { get; set; }
    }
}