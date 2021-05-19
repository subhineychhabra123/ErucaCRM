using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    public class LeadContactVM
    {

        public int LeadContctId { get; set; }
        public int LeadId { get; set; }
        public int ContactId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public virtual AccountVM Account { get; set; }
        public virtual ContactVM Contact { get; set; }
    
    }
}