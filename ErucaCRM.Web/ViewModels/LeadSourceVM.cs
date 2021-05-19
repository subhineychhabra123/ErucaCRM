using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "LeadSource")]
    public class LeadSourceVM
    {
        public int LeadSourceId { get; set; }
        public string LeadSourceName { get; set; }
    }
}