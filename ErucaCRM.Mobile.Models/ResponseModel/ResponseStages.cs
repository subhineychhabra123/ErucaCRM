using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.ResponseModel
{
    [DataContract]
    public class ResponseStages : ResponseStatus
    {
        [DataMember]
        public List<Stages> StageList { get; set; }
    }
}