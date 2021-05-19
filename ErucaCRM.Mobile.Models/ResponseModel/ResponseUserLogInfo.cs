using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ErucaCRM.Mobile.Models.ResponseModel
{
    public class ResponseUserLogedInfo : ResponseStatus
    {
        public User User { get; set; }
        public string Token { get; set; }

        public string VersionCode { get; set; }
    }
}