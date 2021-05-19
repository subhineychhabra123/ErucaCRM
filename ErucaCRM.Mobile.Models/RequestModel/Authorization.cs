using ErucaCRM.Mobile.Models.ResponseModel;
using ErucaCRM.Mobile.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;



namespace ErucaCRM.Mobile.Models.RequestModel
{
    public class Authorization : ResponseStatus
    {
        public string UserId { get; set; }
        public int? CompanyId { get; set; }
    }
}