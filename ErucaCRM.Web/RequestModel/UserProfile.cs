using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.RequestModel
{
    public class UserProfile
    {
        [DataMember]
      public  string UserId { get; set; }
         [DataMember]
      public string EmailId { get; set; }
         [DataMember]
      public int TimeZoneId { get; set; }
      [DataMember]
        public string FirstName { get; set; }
     [DataMember]
        public string LastName { get; set; }
     [DataMember]
        public string CompanyName { get; set; }
      public int CultureInformationId { get; set; }
      public string TimeZoneDescription { get; set; }
      public string CultureDescription { get; set; }
      public string Street { get; set; }
      public string ZipCode { get; set; }
      public int CountryId { get; set; }
      public string City { get; set; }
      public string ImagePath { get; set; }
   
    }
}