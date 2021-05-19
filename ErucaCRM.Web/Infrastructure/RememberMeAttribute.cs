using ErucaCRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErucaCRM.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RememberMeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //LoginModel loginModel = new LoginModel();

            //HttpCookie rememberMe =HttpContext.Current.Request.Cookies["RememberMe"];

            //if (!string.IsNullOrEmpty(rememberMe.Values["emailId"]) && !string.IsNullOrEmpty(rememberMe.Values["password"]))
            //{
            //    loginModel.EmailId = rememberMe.Values["emailId"];
            //    loginModel.Password = rememberMe.Values["password"];
            //    filterContext.Result = new RedirectToRouteResult(new
            //               RouteValueDictionary(new { controller = "site", action = "Login", login }));
            //    // return login;
            //}
        }
    }


}