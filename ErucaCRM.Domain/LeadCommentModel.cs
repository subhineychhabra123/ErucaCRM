using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class LeadCommentModel
    {
        public int LeadCommentId { get; set; }
        public string Comment { get; set; }
        public int LeadId { get; set; }
        public int CreatedBy { get; set; }
        public string UserImg { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AudioFileName { get; set; }
        public string AudioFileDuration { get; set; }
    }
}
