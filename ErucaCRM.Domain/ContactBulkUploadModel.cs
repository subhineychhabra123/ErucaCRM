using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public  class ContactBulkUploadModel
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
       
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string ContactCompanyName { get; set; }
        public string ErrorDescription { get; set; }
   }
}
