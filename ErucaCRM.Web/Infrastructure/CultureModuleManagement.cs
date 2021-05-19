using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErucaCRM.Utility;
using System.Web.Routing;

namespace ErucaCRM.Web.Infrastructure
{
    public class CultureModuleManagement : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {


            if (filterContext.Controller.ViewData.Model != null)
            {
                var objCustomModule = ((filterContext.Controller.ViewData.Model.GetType().GetCustomAttributes(false).FirstOrDefault()));


                if (objCustomModule is CultureModuleAttribute)
                {
                    filterContext.Controller.ViewBag.ModuleName = ((CultureModuleAttribute)objCustomModule).ModuleName;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}