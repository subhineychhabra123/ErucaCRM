using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
  public   class LeadContactModel
    {

        public int LeadContactId { get; set; }
        public int LeadId { get; set; }
        public int ContactId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public virtual LeadModel Lead { get; set; }
        public virtual ContactModel Contact { get; set; }
  
  }
}
