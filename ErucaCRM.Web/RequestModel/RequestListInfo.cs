using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class RequestListInfo
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalRecords { get; set; }
        public int CompanyId
        {
            get;
            set;
        }
    }
}