using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErucaCRM.Repository;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Web.ViewModels;
using ErucaCRM.Domain;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using System.Threading.Tasks;
using ErucaCRM.Utility;
using ErucaCRM.Utility.WebClasses;
using AutoMapper;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.Controllers
{

    [CultureModuleManagement]
    public class SiteController : Controller
    {
        private IUserBusiness userBusiness;
        private IProfileBusiness profileBusiness;
        private ITimeZoneBusiness timeZoneBusiness;
        private ICultureInformationBusiness cultureInformationBusiness;
        private IContentApplicationPageBusiness contentApplicationBusiness;
        public SiteController(IUserBusiness _userBusiness, IProfileBusiness _profileBusiness, ICultureInformationBusiness _cultureInformationBusiness, ITimeZoneBusiness _timeZoneBusiness, IContentApplicationPageBusiness _contentApplicationBusiness)
        {
            userBusiness = _userBusiness;
            profileBusiness = _profileBusiness;
            cultureInformationBusiness = _cultureInformationBusiness;
            timeZoneBusiness = _timeZoneBusiness;
            contentApplicationBusiness = _contentApplicationBusiness;
        }

        public ActionResult Index(string pagename)
        {
            string sitePageName = Constants.PUBLIC_PAGE_HOME;

            LoginModel loginModel = new LoginModel();


            HttpCookie rememberMe = Request.Cookies["RememberMe"];
            if (Request.Cookies["MaxLeadAuditId"] != null)
            {
                Response.Cookies.Remove("MaxLeadAuditId");

            }
            if (rememberMe != null && !string.IsNullOrEmpty(rememberMe.Values["emailId"]) && !string.IsNullOrEmpty(rememberMe.Values["password"]))
            {
                loginModel.EmailId = rememberMe.Values["emailId"];
                loginModel.Password = rememberMe.Values["password"];
                loginModel.isChecked = true;
                UserModel usermodel = userBusiness.ValidateUser(loginModel.EmailId, loginModel.Password, true);
                if (usermodel != null)
                {
                    int userTypeId = IsLogin(loginModel);
                    if (userTypeId == 1)
                    {
                        return RedirectToAction("ContentManagement", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Home", "User");
                    }
                }
            }
            else if (SessionManagement.LoggedInUser != null && SessionManagement.LoggedInUser.Password != null && SessionManagement.LoggedInUser.Password != null)
            {
                loginModel.EmailId = SessionManagement.LoggedInUser.EmailId;
                loginModel.Password = SessionManagement.LoggedInUser.Password;
                loginModel.isChecked = true;
                UserModel usermodel = userBusiness.ValidateUser(loginModel.EmailId, loginModel.Password, true);
                if (usermodel != null)
                {
                    int userTypeId = IsLogin(loginModel);
                    if (userTypeId == 1)
                    {
                        return RedirectToAction("ContentManagement", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Home", "User");
                    }
                }

            }
            if (pagename != null)
            {
                sitePageName = pagename;
            }
        

            PublicHomePageVM cultureSpecificSiteContentVM = new PublicHomePageVM();
            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);
            AutoMapper.Mapper.Map(cultureSpecificSiteContentModel, cultureSpecificSiteContentVM);

            return View(cultureSpecificSiteContentVM);

        }
        [ChildActionOnly]
        public ActionResult RegisterUser(string pagename)
        {
            string sitePageName = Constants.PUBLIC_PAGE_REGISTRATION;


            if (pagename != null)
            {
                sitePageName = pagename;
            }


            RegistrationVM registrationVM = new RegistrationVM();

            ViewBag.success = false;
            ViewBag.CultureList = GetCultureList();
            ViewBag.TimeZoneList = GetTimeZoneList();
            return View(registrationVM);
            //return PartialView("_Registration", registrationVM);
        }
        [HttpGet]
        public ActionResult SetProfile()
        {
         
      
            return View();
        }
        [HttpPost]
        public string SetProfile(string name)
        {
            profileBusiness.UpdateCompanyStandardProfile();

            return "Prolfe Set Success";
        }

        private int IsLogin(LoginModel login)
        {

            UserModel usermodel = userBusiness.ValidateUser(login.EmailId, login.Password, login.isChecked);
            if (usermodel != null)
            {
               
                SessionManagement.LoggedInUser.UserId = usermodel.UserId;
                SessionManagement.LoggedInUser.UserIdEncrypted = usermodel.UserId.Encrypt();
                SessionManagement.LoggedInUser.UserName = (usermodel.FirstName + " " + usermodel.LastName);
                SessionManagement.LoggedInUser.Password = usermodel.Password;
                SessionManagement.LoggedInUser.EmailId = usermodel.EmailId;
                SessionManagement.LoggedInUser.FullName = usermodel.FirstName + " " + usermodel.LastName;
                SessionManagement.LoggedInUser.Role = (Enums.UserType)usermodel.UserTypeId;
                SessionManagement.LoggedInUser.CompanyId = usermodel.CompanyModel.CompanyId;
                SessionManagement.LoggedInUser.CompanyName = usermodel.CompanyModel.CompanyName;
                SessionManagement.LoggedInUser.ProfileImageUrl = usermodel.ImageURL;
                SessionManagement.LoggedInUser.CurrentCulture = usermodel.CultureInformationModel.CultureName;
                SessionManagement.LoggedInUser.TimeZoneOffSet = usermodel.TimeZoneModel.offset;
          
                string pipeSeperatedPermissions = String.Join("|", usermodel.ProfileModel.ProfilePermissionModels.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList().ToArray());
                //string pipeSeperatedPermissions = userBusiness.CheckCompanyPlanProfile((int)usermodel.CompanyModel.CompanyId, (int)usermodel.UserId);
                CommonFunctions.SetCookie(SessionManagement.LoggedInUser, pipeSeperatedPermissions, login.isChecked);
            }
            return usermodel.UserTypeId;
        }
        public ActionResult Register(string pagename)
        {
            string sitePageName = Constants.PUBLIC_PAGE_REGISTRATION;

            TempData["View"] = "Register";
            if (pagename != null)
            {
                sitePageName = pagename;
            }
            ViewBag.success = false;
            ViewBag.CultureList = GetCultureList();
            ViewBag.TimeZoneList = GetTimeZoneList();
            RegistrationVM registrationVM = new RegistrationVM();

            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            registrationVM.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
            registrationVM.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
            registrationVM.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;

            return View(registrationVM);

        }
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                UserModel usermodel = userBusiness.ValidateUser(login.EmailId, login.Password, login.isChecked);
                if (usermodel != null)
                {
                    int userTypeId = IsLogin(login);
                    if (userTypeId == 1)
                    {
                        return RedirectToAction("ContentManagement", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Home", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("EmailId", CommonFunctions.GetGlobalizedLabel("Login", "LoginErrorMessage"));
                    //Rebind model for displaying  Content

                    login = new LoginModel();

                    string sitePageName = Constants.PUBLIC_PAGE_LOGIN;

                    CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

                    login.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
                    login.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
                    login.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;
                    ViewBag.CultureList = GetCultureList();
                    ViewBag.TimeZoneList = GetTimeZoneList();
                    return View(login);
                }
            }
            else
            {
                login = new LoginModel();

                string sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

                login.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
                login.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
                login.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;
                return View(login);
            }

        }
        //POST:
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationVM registration)
        {
            string sitePageName = Constants.PUBLIC_PAGE_REGISTRATION;

            if (ModelState.IsValid)
            {
                Boolean IsModelErrorExist = false;
                if (ModelState["Profile.ProfileName"] != null)
                    ModelState["Profile.ProfileName"].Errors.Clear();

                if (ModelState["Role.RoleName"] != null)
                    ModelState["Role.RoleName"].Errors.Clear();

                RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();

                ViewBag.CultureList = GetCultureList();
                ViewBag.TimeZoneList = GetTimeZoneList();


                if (String.IsNullOrEmpty(recaptchaHelper.Response))
                {
                    ModelState.AddModelError("", CommonFunctions.GetGlobalizedLabel("Registration", "CaptchaAnswerRequired"));//"Captcha answer cannot be empty.");
                    IsModelErrorExist = true;

                }
                if (IsModelErrorExist == false)
                {
                    Task<RecaptchaVerificationResult> recaptchaResult = recaptchaHelper.VerifyRecaptchaResponseTaskAsync();

                    if (recaptchaResult.Result != RecaptchaVerificationResult.Success)
                    {
                        ModelState.AddModelError("", CommonFunctions.GetGlobalizedLabel("Registration", "IncorrectCaptchaAnswer"));
                        IsModelErrorExist = true;

                    }
                }
                UserModel userModel;
                if (IsModelErrorExist == false)
                {
                    userModel = userBusiness.GetUserByEmailId(registration.EmailId);
                    if (userModel != null)
                    {

                        ModelState.AddModelError("", CommonFunctions.GetGlobalizedLabel("Registration", "EmailAlreadyExists"));
                        IsModelErrorExist = true;

                    }
                }

                if (IsModelErrorExist == true)
                {


                    ViewBag.success = false;


                    if (TempData["View"] != null && TempData["View"].ToString() == "Login")
                    {
                        LoginModel loginModel = new LoginModel();
                        ViewBag.OpenRegistrationPopup = true;

                        TempData["View"] = "Login";

                        sitePageName = Constants.PUBLIC_PAGE_LOGIN;
                        CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

                        loginModel.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
                        loginModel.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
                        loginModel.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;
                        //  return View(login);
                        return View("Login", loginModel);
                    }
                    else
                    {
                        TempData["View"] = "Register";
                        // RegistrationVM registrationVM = new RegistrationVM();
                        CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

                        registration.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
                        registration.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
                        registration.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;

                        return View(registration);

                    }
                }

                userModel = new UserModel();


                AutoMapper.Mapper.Map(registration, userModel);              
                userModel = userBusiness.RegisterUser(userModel);
                SessionManagement.LoggedInUser.UserId = userModel.UserId;
                SessionManagement.LoggedInUser.UserIdEncrypted = userModel.UserId.Encrypt();
                SessionManagement.LoggedInUser.UserName = (userModel.FirstName + " " + userModel.LastName);
                SessionManagement.LoggedInUser.Password = userModel.Password;
                SessionManagement.LoggedInUser.EmailId = userModel.EmailId;
                SessionManagement.LoggedInUser.FullName = userModel.FirstName + " " + userModel.LastName;
                SessionManagement.LoggedInUser.Role = (Enums.UserType)userModel.UserTypeId;
                SessionManagement.LoggedInUser.CompanyId = userModel.CompanyModel.CompanyId;
                SessionManagement.LoggedInUser.CompanyName = userModel.CompanyModel.CompanyName;
                SessionManagement.LoggedInUser.ProfileImageUrl = userModel.ImageURL;
                SessionManagement.LoggedInUser.CurrentCulture = userModel.CultureInformationModel.CultureName;
                string pipeSeperatedPermissions = String.Join("|", userModel.ProfileModel.ProfilePermissionModels.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList().ToArray());
                CommonFunctions.SetCookie(SessionManagement.LoggedInUser, pipeSeperatedPermissions, false);
                ViewBag.success = true;
                ViewBag.StatusMessage = CommonFunctions.GetGlobalizedLabel("Registration", "RegistrationSuccessMessage"); //"Registered successfully. Please login";

                ViewBag.RegistrationSuccessWelComeMessage = CommonFunctions.GetGlobalizedLabel("Registration", "RegistrationSuccessWelComeMessage");
                string emailSubject = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "RegistrationEmailSubject");
                string emailBody = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "RegistrationEmailBody");
                userBusiness.SendRegistrationEmail(userModel, emailSubject, emailBody);
                //ServiceReference1.Service1Client mailService = new ServiceReference1.Service1Client();
                //string to = string.Empty;
                //string subject = string.Empty;
                //string body = string.Empty;
                //to = userModel.EmailId;
                //subject = "Welcome to Eurca CRM";
                //body = "<div style='font-face:arial;'><img src='http://erucacrm.sensationsolutions.com/Content//Content/images/logo-dashbord.png'><hr/>Dear Customer" + ",<br/><br/>Thanks for your registration to Eruca CRM. <br/> Please find the following details to login into Eruca CRM. <br/><br/>CRM URL: <a href='http://erucacrm.sensationsolutions.com'>Click here to navigate to CRM<a/><br/><br/>" + "Your Username: " + userModel.EmailId.Trim() + "<br/>Password: " + userModel.Password + " <br/><br/>Thank you.<br/><br/>Customer Relations</div>";
                //mailService.SendEmail( to, subject, body, true);
                ModelState.Clear();


            }

            if (TempData["View"] != null && TempData["View"].ToString() == "Login")
            {
                TempData["View"] = "Login";
                sitePageName = Constants.PUBLIC_PAGE_LOGIN;

                LoginModel loginModel = new LoginModel();
                ViewBag.OpenRegistrationPopup = true;


                CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

                loginModel.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
                loginModel.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
                loginModel.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;
                //  return View(login);
                return View("Login", loginModel);
            }
            else
            {
                ViewBag.CultureList = GetCultureList();
                ViewBag.TimeZoneList = GetTimeZoneList();
                sitePageName = Constants.PUBLIC_PAGE_REGISTRATION;
                TempData["View"] = "Register";
                // RegistrationVM registrationVM = new RegistrationVM();
                CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

                registration.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
                registration.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
                registration.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;

                return View(registration);
            }



        }

        public ActionResult Login(string pagename)
        {
            LoginModel loginModel = new LoginModel();
            TempData["View"] = "Login";
            string sitePageName = Constants.PUBLIC_PAGE_LOGIN;


            if (pagename != null)
            {
                sitePageName = pagename;
            }



            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            loginModel.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
            loginModel.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
            loginModel.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;


            if (Request.Cookies["Permissions"] != null)
            {
                HttpCookie myCookie = new HttpCookie("Permissions");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }

            ViewBag.success = false;
            ViewBag.CultureList = GetCultureList();
            ViewBag.TimeZoneList = GetTimeZoneList();

            return View(loginModel);
        }

        public ActionResult ForgotPassword(string pagename)
        {


            string sitePageName = Constants.PUBLIC_PAGE_FORGOTPASSWORD;


            if (pagename != null)
            {
                sitePageName = pagename;
            }



            ForgotPasswordVM forgotPasswordVM = new ViewModels.ForgotPasswordVM();

            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            forgotPasswordVM.CultureSpecificPageContent = cultureSpecificSiteContentModel.CultureSpecificPageContent;
            forgotPasswordVM.CultureSpecificPageMetaTitle = cultureSpecificSiteContentModel.CultureSpecificPageMetaTitle;
            forgotPasswordVM.CultureSpecificPageMetaTags = cultureSpecificSiteContentModel.CultureSpecificPageMetaTags;

            return View(forgotPasswordVM);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordVM forgotPassword)
        {
            Response response = new Response();
            response.Status = "Failure";
            response.StatusCode = 500;
            if (ModelState.IsValid)
            {
                UserModel userModel;
                bool isMailSent = true;
                userModel = userBusiness.GetUserByEmailId(forgotPassword.EmailId);
                if (userModel != null)
                {

                    if (userModel.Active == true)
                    {
                        string emailSubject = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "ForgotEmailSubject");
                        string emailBody = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "ForgotEmailBody");
                        isMailSent = userBusiness.SendPasswordRecoveryMail(userModel, emailSubject, emailBody);
                        //ServiceReference1.Service1Client mailService = new ServiceReference1.Service1Client();
                        //string to = string.Empty;
                        //string subject = string.Empty;
                        //string body = string.Empty;
                        //to = userModel.EmailId;
                        //subject = "Login information Eurca CRM";
                        //body = "<div style='font-face:arial;'><img src='http://erucacrm.sensationsolutions.com/Content//Content/images/logo-dashbord.png'><hr/>Dear Customer" + ",<br/><br/>Please return to the site and log in using the following information" + "<br/>Your Username: " + userModel.EmailId.Trim() + "<br/>Password: " + userModel.Password + " <br/><br/>Thank you.<br/ >Customer Relations</div>";
                        //try
                        //{
                        //    mailService.SendEmail(to, subject, body, true);
                        //    isMailSent = true;
                        //}
                        //catch (Exception ex)
                        //{
                        //    isMailSent = false;
                        //}
                        if (isMailSent)
                        {
                            response.Message = CommonFunctions.GetGlobalizedLabel("Login", "PasswordForAccountSent");// "Password for your account is sent. ";
                            response.Status = "Success";
                        }
                        else
                        {
                            response.Message = CommonFunctions.GetGlobalizedLabel("Login", "EmailSendingError");//"Error while sending mail. Please try again.";
                        }
                    }
                    else
                    {
                        response.Message = CommonFunctions.GetGlobalizedLabel("Login", "AccountDeactivated");//"Your account is deactivated. Please contact your administrator";
                    }

                }
                else
                {
                    response.Message = CommonFunctions.GetGlobalizedLabel("Login", "UserDoesNotExist");//"No user exists with the given Email Address.";
                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }

            }
            return Json(response);
        }


        public ActionResult Feature(string pagename)
        {
            PublicFeaturePageVM cultureSpecificSiteContentVM = new PublicFeaturePageVM();

            string sitePageName = Constants.PUBLIC_PAGE_FEATURE;

            if (pagename != null)
            {
                sitePageName = pagename;
            }



            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            AutoMapper.Mapper.Map(cultureSpecificSiteContentModel, cultureSpecificSiteContentVM);

            return View(cultureSpecificSiteContentVM);
        }
        public ActionResult ContactUs(string pagename)
        {
            string sitePageName = Constants.PUBLIC_PAGE_CONTACTUS;


            if (pagename != null)
            {
                sitePageName = pagename;
            }


            CultureSpecificSiteContentVM cultureSpecificSiteContentVM = new CultureSpecificSiteContentVM();
            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            AutoMapper.Mapper.Map(cultureSpecificSiteContentModel, cultureSpecificSiteContentVM);

            return View(cultureSpecificSiteContentVM);
        }

        public ActionResult AboutUs(string pagename)
        {
            string sitePageName = Constants.PUBLIC_PAGE_ABOUTUS;

 

            if (pagename != null)
            {
                sitePageName = pagename;
            }


            PublicAboutUsPageVM cultureSpecificSiteContentVM = new PublicAboutUsPageVM();
            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            AutoMapper.Mapper.Map(cultureSpecificSiteContentModel, cultureSpecificSiteContentVM);

            return View(cultureSpecificSiteContentVM);


        }
        public List<CultureInformationVM> GetCultureList()
        {
            List<CultureInformationModel> cultureInfoList = new List<CultureInformationModel>();
            List<CultureInformationVM> cultureInfoVMList = new List<CultureInformationVM>();
            cultureInfoList = cultureInformationBusiness.GetUserCultures();
            Mapper.Map(cultureInfoList, cultureInfoVMList);
            return cultureInfoVMList;
        }

        public List<TimeZoneVM> GetTimeZoneList()
        {
            List<TimeZoneModal> timeZoneListList = new List<TimeZoneModal>();
            List<TimeZoneVM> timeZoneVMList = new List<TimeZoneVM>();
            timeZoneListList = timeZoneBusiness.GetTimeZones();
            Mapper.Map(timeZoneListList, timeZoneVMList);
            return timeZoneVMList;
        }

        public ActionResult MobileCRM(string pagename)
        {
            string sitePageName = Constants.PUBLIC_PAGE_MobileCRM;



            if (pagename != null)
            {
                sitePageName = pagename;
            }


            PublicMobileCRMPageVM cultureSpecificSiteContentVM = new PublicMobileCRMPageVM();
            CultureSpecificSiteContentModel cultureSpecificSiteContentModel = contentApplicationBusiness.GetPageCultureSpecificContent(sitePageName);

            AutoMapper.Mapper.Map(cultureSpecificSiteContentModel, cultureSpecificSiteContentVM);

            return View(cultureSpecificSiteContentVM);


        }
    }
}