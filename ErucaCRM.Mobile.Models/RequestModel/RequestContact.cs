using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ErucaCRM.Mobile.Models.RequestModel
{
    public class RequestContact : RequestListInfo
    {
        public string UserId { get; set; }
        public List<Contact> Contacts { get; set; }
        public Contact Contact { get; set; }
        public string ContactId { get; set; }
        public string FilterBy { get; set; }
        public bool IsSearchByTag { get; set; }
        public string SearchTags { get; set; }
        public string ContactCompanyName { get; set; }
        public string LeadId { get; set; }
      
    }
}