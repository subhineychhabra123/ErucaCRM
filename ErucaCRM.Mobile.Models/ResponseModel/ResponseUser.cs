using ErucaCRM.Mobile.Models.Model;
using ErucaCRM.Mobile.Models.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.ResponseModel
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