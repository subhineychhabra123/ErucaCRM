using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.ResponseModel
{
    [DataContract]
    public class ResponseContact : ResponseStatus
    {
        [DataMember(EmitDefaultValue = false)]
        public List<Contact> ContactList { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Contact Contact { get; set; }
        [DataMember]
        public int TotalRecords { get; set; }
    }
}