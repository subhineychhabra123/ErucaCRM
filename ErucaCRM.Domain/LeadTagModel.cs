using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class LeadTagModel
    {
        public int LeadTagId { get; set; }
        public int LeadId { get; set; }
        public int TagId { get; set; }

        //public virtual LeadModel Lead { get; set; }
        public virtual TagModel Tag { get; set; }
    }
}
