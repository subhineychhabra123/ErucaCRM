using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public class LeadCommentVM
    {
        public string LeadCommentId { get; set; }
        public string Comment { get; set; }
        public string LeadId { get; set; }
        public string UserId { get; set; }
        public string CreatedBy { get; set; }
        public string UserImg { get; set; }
        public string UserName { get; set; }
        public string AudioFileName { get; set; }
        public string AudioPathName { get; set; }
        public string LeadCommentCreatedTime
        {
            get
            {
                return CreatedDate.ToDateTimeNow().ToLongDateString();
            }
        }
        public DateTime CreatedDate { get; set; }
    }
}