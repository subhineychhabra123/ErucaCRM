using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.ViewModels
{
    public static class LayoutInnerFooter
    {
        public static string Module { get { return "LayoutInner"; } }
        public static string FooterLast
        {
            get { return CommonFunctions.GetGlobalizedLabel(Module, "FooterLast"); }

        }
    }
}