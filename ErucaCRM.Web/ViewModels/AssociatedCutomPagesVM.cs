using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    public class AssociatedCutomPagesVM
    {
        public string ApplicationPageId { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageUrl { get; set; }
        public bool IsApplicationPage { get; set; }
        public string Action { get; set; }
        public bool RemoveAction { get; set; }
        public string CustomPageId { get; set; }
        

    }
}