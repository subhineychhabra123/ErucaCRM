using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
   [Serializable]
    public class FileContent
    {

        
        public string FileName { get; set; }
      
        public string LeadId { get; set; }
      
        public string UserId { get; set; }
     
        public string CompanyId { get; set; }
       
        public Stream FileData { get; set; }

    }
}