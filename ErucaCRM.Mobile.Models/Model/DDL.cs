using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ErucaCRM.Mobile.Models.Model
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