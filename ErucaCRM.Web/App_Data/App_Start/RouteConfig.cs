using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErucaCRM.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("elmah.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                            name: "CMSHomeRoute",
                            url: "Home/{pagename}",
                            defaults: new { controller = "Site", action = "Index", pagename = UrlParameter.Optional }
                        );
            routes.MapRoute(
                    name: "CMSLoginRoute",
                    url: "Login/{pagename}",
                    defaults: new { controller = "Site", action = "Login", pagename = UrlParameter.Optional }
                );
            routes.MapRoute(
                          name: "CMSAboutusRoute",
                          url: "Aboutus/{pagename}",
                          defaults: new { controller = "Site", action = "Aboutus", pagename = UrlParameter.Optional }
                      );

            routes.MapRoute(
                       name: "CMSContactusRoute",
                       url: "Contactus/{pagename}",
                       defaults: new { controller = "Site", action = "Contactus", pagename = UrlParameter.Optional }
                   );
            routes.MapRoute(
                      name: "CMSFeatureRoute",
                      url: "Feature/{pagename}",
                      defaults: new { controller = "Site", action = "Feature", pagename = UrlParameter.Optional }
                  );
            routes.MapRoute(
                     name: "CMSForgotPasswordRoute",
                     url: "ForgotPassword/{pagename}",
                     defaults: new { controller = "Site", action = "ForgotPassword", pagename = UrlParameter.Optional }
                 );


            routes.MapRoute(
                 name: "UserRoutes",
                 url: "User/{action}/{Id_encrypted}",
                 defaults: new { controller = "User", action = "Index", id_encrypted = UrlParameter.Optional }
             );

            routes.MapRoute(
                 name: "AdminRoutes",
                 url: "admin/{action}/{Id_encrypted}",
                 defaults: new { controller = "Admin", action = "Index", id_encrypted = UrlParameter.Optional }
             );
            routes.MapRoute(
               name: "SiteRoutes",
               url: "{action}",
               defaults: new { controller = "Site", action = "Index" }
           );
            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Site", action = "Index", id = UrlParameter.Optional }
             );
        }
    }
}