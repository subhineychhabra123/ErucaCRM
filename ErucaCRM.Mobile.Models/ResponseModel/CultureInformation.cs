using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.ResponseModel
{
    public class CultureInformation
    {
        [DataMember]
        public int CultureInformationId { get; set; }
        [DataMember]
        public string CultureInformationDescription { get; set; }

    }
}