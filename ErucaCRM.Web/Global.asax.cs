using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using ErucaCRM.Business.Infrastructure;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Utility;
using ErucaCRM.Web.ViewModels;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Business;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Repository.Infrastructure;
using System.Web.Optimization;
using System.Globalization;

namespace ErucaCRM.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {



        protected void Application_Start()
        {
           
            AreaRegistration.RegisterAllAreas();
            StructureMapper.Run();

            AutoMapperWebProfile.Run();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LoadAllUserCultures();

        }

        /// <summary>
        /// Application will load all cultures on start of application
        /// </summary>
        private void LoadAllUserCultures()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();

            ICultureInformationBusiness CultureInfoBusiness = new CultureInformationBusiness(unitOfWork);

            List<ErucaCRM.Domain.CultureInformationModel> listCultureInformationModel = CultureInfoBusiness.LoadAllUserCultures();

            for (int i = 0; i < listCultureInformationModel.Count; i++)
            {
                if (listCultureInformationModel[i].IsDefault == true)
                {
                    ErucaCRM.Utility.CultureInformationManagement.ApplicationDefaultCulture = listCultureInformationModel[i].CultureName;

                }

                ErucaCRM.Utility.CultureInformationManagement.SetCultureObject(listCultureInformationModel[i].CultureName, listCultureInformationModel[i].LabelsXML);
            }

        }

        private void SetCurrentUserCulture()
        {
            System.Globalization.CultureInfo culture;
            try
            {
                culture = new System.Globalization.CultureInfo(CultureInformationManagement.CurrentUserCulture);
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;

                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

                }
            catch (Exception ex) { }
        }


        void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Session != null)
            {
                if (ErucaCRM.Utility.SessionManagement.LoggedInUser == null || ErucaCRM.Utility.SessionManagement.LoggedInUser.UserId == 0)
                {
                    string[] groups = null;
                    System.Security.Principal.GenericIdentity id = new System.Security.Principal.GenericIdentity(string.Empty, "RIAuthentication");
                    System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(id, groups);
                    Context.User = principal;
                }
                SetCurrentUserCulture();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // code to change culture
        }

        protected void Application_AuthenticateRequest(object sender, System.EventArgs e)
        {
            string cookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = Context.Request.Cookies[cookieName];

            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                System.Web.Security.FormsAuthenticationTicket authTicket = null;
                authTicket = System.Web.Security.FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    string[] groups = authTicket.UserData.Split('|');
                    System.Security.Principal.GenericIdentity id = new System.Security.Principal.GenericIdentity(authTicket.Name, "RIAuthentication");
                    System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(id, groups);
                    Context.User = principal;

                }
            }
        }
    }
}