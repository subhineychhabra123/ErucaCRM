using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class CompanyModel
    {
        public int CompanyId
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public string CreatedBy { get; set; }
      
        public DateTime CreatedOn { get; set; }
       
        public bool IsActive { get; set; }
    }
}
