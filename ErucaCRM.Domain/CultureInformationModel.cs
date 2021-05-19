using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class CultureInformationModel
    {
        public int CultureInformationId { get; set; }
        public string CultureName { get; set; }
        public string Language { get; set; }
        public string ExcelFilePath { get; set; }
        public string LabelsXML { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
 
    }
}
