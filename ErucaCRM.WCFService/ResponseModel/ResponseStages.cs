using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.ResponseModel
{
    [DataContract]
    public class ResponseStages : ResponseStatus
    {
        [DataMember]
        public List<Stages> StageList { get; set; }
    }
}