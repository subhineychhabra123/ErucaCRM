using ErucaCRM.WCFService.Models;
using ErucaCRM.WCFService.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.ResponseModel
{
    public class ResponseUser : ResponseStatus
    {

        [DataMember]
        public UserProfile User { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDL> TimeZoneList { get; set; }


        [DataMember(EmitDefaultValue = false)]
        public List<DDL> ProfileList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDL> RoleList { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<DDL> CultureInformationList { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public List<DDL> CountryList { get; set; }
    }
}