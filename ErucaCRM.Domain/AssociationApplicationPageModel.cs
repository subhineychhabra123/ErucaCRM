using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class AssociationApplicationPageModel
    {
        public int AssociationApplicationPageId { get; set; }
        public int ApplicationPageId { get; set; }
        public int CustomPageId { get; set; }

        public  ApplicationPageModel ApplicationPage { get; set; }
        public ApplicationPageModel ApplicationPage1 { get; set; }
    }
}
