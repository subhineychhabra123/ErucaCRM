using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.Infrastructure
{
    public class CultureModuleAttribute : Attribute
    {
        public string ModuleName { get; set; }

    } 
}