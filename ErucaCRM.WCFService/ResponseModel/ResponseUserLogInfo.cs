using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    public class ResponseUserLogedInfo : ResponseStatus
    {
        public User User { get; set; }
        public string Token { get; set; }
       
    }
}