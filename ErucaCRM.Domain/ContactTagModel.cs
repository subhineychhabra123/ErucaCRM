using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class ContactTagModel
    {
        public int ContactTagId { get; set; }
        public int ContactId { get; set; }
        public int TagId { get; set; }

        public virtual ContactModel ContactModel { get; set; }
        public virtual TagModel TagModel { get; set; }
    }
}
