using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class LeadsInPipeLineModel
    {
        public string StageName { get; set; }
        public Nullable<int> TotalLeadsInPipeLine { get; set; }
    }
}
