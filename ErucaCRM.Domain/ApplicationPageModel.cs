using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class ApplicationPageModel
    {
        public int ApplicationPageId { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageUrl { get; set; }
        public bool IsApplicationPage { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        public  ICollection<AssociationApplicationPageModel> AssociationApplicationPages { get; set; }
        public ICollection<AssociationApplicationPageModel> AssociationApplicationPages1 { get; set; }
        public  ContentApplicationPageModel ContentApplicationPage { get; set; }
    }
}
