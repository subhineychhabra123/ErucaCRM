using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace ErucaCRM.Utility
{
    public class CookieManagement
    {

        public static string ReadFromCultureCookie
        {
            get
            {
                if (HttpContext.Current.Request.Cookies["Culture"] != null && !string.IsNullOrEmpty(HttpContext.Current.Request.Cookies["Culture"].Value))
                {
                    return HttpContext.Current.Request.Cookies["Culture"].Value;

                }
                else
                    return null;
            }
        }
    }
}
