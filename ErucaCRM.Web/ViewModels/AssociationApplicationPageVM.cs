using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    public class AssociationApplicationPageVM
    {
        public int AssociationApplicationPageId { get; set; }
        public string ApplicationPageId { get; set; }
        public string CustomPageId { get; set; }
        public string Action
        {

            get
            {
                if (this.ApplicationPage.ApplicationPageId == this.ApplicationPageId)
                    return "Remove From ";
                else
                    return "Add To ";
            }
        }

        public ApplicationPageVM ApplicationPage { get; set; }
        public ApplicationPageVM ApplicationPage1 { get; set; }
    }
}