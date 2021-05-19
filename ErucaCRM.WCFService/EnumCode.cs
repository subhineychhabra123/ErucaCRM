using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ErucaCRM.WCFService
{
    public class EnumCode
    {
        public enum status
        {
            [Description("Success")]
            Success = 1,
            [Description("Email does Not Match")]
            EmailError = 2,
            [Description("Password does not match")]
            PasswordError = 3,
            [Description("Failure")]
            Failure = 4,
            [Description("OK")]
            Ok = 202,
            [Description("Created")]
            Created = 201,
            [Description("No Content")]
            NoContent = 204,
            [Description("Unauthorized")]
            Unauthorized = 401,
            [Description("Not Implemented")]
            NotImplemented = 501,
        }
    }
}