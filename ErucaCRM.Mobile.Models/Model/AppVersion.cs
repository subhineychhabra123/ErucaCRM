using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace ErucaCRM.Mobile.Models.Model
{
    public class AppVersion
    {
        public Nullable<int> AppId { get; set; }
        [DataMember]
        public string VersionCode { get; set; }
        public string VersionName { get; set; }
    }
}