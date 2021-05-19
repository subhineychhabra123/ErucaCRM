using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.Web.Models
{
    public class AppVersion
    {
        public Nullable<int> AppId { get; set; }
        [DataMember]
        public string VersionCode { get; set; }
        public string VersionName { get; set; }
    }
}