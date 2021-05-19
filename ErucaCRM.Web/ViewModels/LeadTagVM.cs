using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;
namespace ErucaCRM.Web.ViewModels
{
    public class LeadTagVM
    {
        public int LeadTagId { get; set; }
        public int LeadId { get; set; }
        public int TagId { get; set; }

        public virtual TagVM TagVM { get; set; }
    }
}