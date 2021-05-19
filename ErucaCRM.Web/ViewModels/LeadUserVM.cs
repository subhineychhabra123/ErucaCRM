using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "LeadUser")]
    public class LeadUserVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageURL { get; set; }
        public string Name { get { return CommonFunctions.ConcatenateStrings(this.FirstName, this.LastName); } }
    }
}