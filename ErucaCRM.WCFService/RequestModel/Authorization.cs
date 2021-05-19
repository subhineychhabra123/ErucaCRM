using ErucaCRM.WCFService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.WCFService.RequestModel
{
    public class Authorization : ResponseStatus
    {
        public string UserId { get; set; }
        public int? CompanyId { get; set; }
    }
}