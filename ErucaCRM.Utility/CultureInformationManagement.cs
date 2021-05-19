using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ErucaCRM.Utility.WebClasses;
using System.Xml;

namespace ErucaCRM.Utility
{
    public class CultureInformationManagement
    {

        public static string ApplicationDefaultCulture
        {
            get
            {
                if (HttpContext.Current.Application["DefaultCulture"] == null)
                {
                    HttpContext.Current.Application["DefaultCulture"] = ErucaCRM.Utility.ReadConfiguration.DefaultUserCulture;
                }
                return Convert.ToString(HttpContext.Current.Application["DefaultCulture"]);


            }
            set { HttpContext.Current.Application["DefaultCulture"] = value; }

        }


        private static string _currentUserCulture;

        public static string CurrentUserCulture
        {
            get
            {
                //if user is logged in then get the current culture from session

                if (HttpContext.Current.User.Identity.IsAuthenticated == true && SessionManagement.LoggedInUser.Role != Enums.UserType.Admin)
                {
                    _currentUserCulture = SessionManagement.LoggedInUser.CurrentCulture;

                }
                else
                {
                    //if user is not logged in then first read culture info from culture specific cookie 
                    _currentUserCulture = CookieManagement.ReadFromCultureCookie;

                    //otherwise get application default culture
                    if (string.IsNullOrEmpty(_currentUserCulture))
                    {
                        _currentUserCulture = CultureInformationManagement.ApplicationDefaultCulture;
                    }

                }

                return _currentUserCulture;

            }

        }


        /// <summary>
        /// This methid will set the culture at application label
        /// </summary>
        /// <param name="cultureName"></param>
        /// <param name="lablesXML"></param>
        public static void SetCultureObject(string cultureName, string lablesXML, bool isActive = true)
        {
            UserCulture userCulture = new UserCulture();
            XmlDocument xmlDocCulture = new XmlDocument();

            if (isActive == false)
            {
                HttpContext.Current.Application[cultureName] = userCulture;

            }
            else
            {
                if (!string.IsNullOrEmpty(lablesXML))
                {
                    xmlDocCulture.LoadXml(lablesXML);
                }

                userCulture.CultureName = cultureName;
                userCulture.CultureXML = xmlDocCulture;
                HttpContext.Current.Application[cultureName] = userCulture;
            }
        }




        public static UserCulture GetCultureObject()
        {
            ///if sessionmanagement.loggedinuser is not null 
            ///get culture of loggedin user
            ///else if get culutre from cookie
            ///else
            ///get culdture from application var
            UserCulture userCulture = new UserCulture();
            if (HttpContext.Current.Application[CultureInformationManagement.CurrentUserCulture] != null)
                userCulture = (UserCulture)HttpContext.Current.Application[CultureInformationManagement.CurrentUserCulture];
            return userCulture;
        }



    }
}
