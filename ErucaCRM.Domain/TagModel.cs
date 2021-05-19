using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class TagModel
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public virtual CompanyModel CompanyModel { get; set; }

      


    }

}
