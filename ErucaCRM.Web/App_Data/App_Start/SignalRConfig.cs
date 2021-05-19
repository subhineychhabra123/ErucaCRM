using System.Web;
using System.Web.Optimization;
using ErucaCRM.Utility;
using Microsoft.Owin;
using Owin;
using System;
using Microsoft.AspNet.SignalR;
[assembly: OwinStartup(typeof(ErucaCRM.Web.AppStartup))]
namespace ErucaCRM.Web
{
  
    public class AppStartup
    {
        public static void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new MyConnectionFactory());
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
    public class MyConnectionFactory : IUserIdProvider
    {

        public string GetUserId(IRequest request)
        {
            if (request.Cookies["userid"] != null)
            {
                return request.Cookies["userid"].Value;
            }

            return request.Cookies["userid"].Value;
        }
    }
}