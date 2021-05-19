using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "LeadStatus")]
    public class LeadStatusVM
    {
        public int LeadStatusId { get; set; }
        public string LeadStatusName { get; set; }
    }
}