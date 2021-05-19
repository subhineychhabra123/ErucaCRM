using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErucaCRM.Utility;

namespace ErucaCRM.Web.Controllers
{
    public class ErrorController : Controller
    {
        //public ErrorController(IUserBusiness _contactBusiness)
        public ErrorController()
        {
            //contactBusiness = _contactBusiness;
            //profileBusiness = _profileBusiness;
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult Internal()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult SessionTimeOut()
        {
            ErucaCRM.Utility.WebClasses.Response response = new Utility.WebClasses.Response();
            response.StatusCode = 401;
            response.Message = "Session Time Out";
            return Json(response, JsonRequestBehavior.AllowGet);

        }
    }
}
