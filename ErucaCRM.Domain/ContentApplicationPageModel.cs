using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class ContentApplicationPageModel
    {
        public int ContentApplicationPageId { get; set; }
        public int CultureInformationId { get; set; }
        public int ApplicationPageId { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string PageContent { get; set; }
        public bool UseDefaultContent { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public  ApplicationPageModel ApplicationPage { get; set; }
        public  CultureInformationModel CultureInformation { get; set; }
    }
}
