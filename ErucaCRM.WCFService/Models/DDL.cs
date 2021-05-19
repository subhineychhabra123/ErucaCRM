using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class DDL
    {
        [DataMember]
        public int Value { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
    public class DDLEncrypt
    {
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}