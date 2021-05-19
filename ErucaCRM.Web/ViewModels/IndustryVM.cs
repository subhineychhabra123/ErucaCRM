using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Industry")]
    public class IndustryVM
    {
        public int IndustryId { get; set; }
        public string IndustryName { get; set; }
    }
}