using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ErucaCRM.Web.ViewModels;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Utility;
using ErucaCRM.Domain;
using System.Configuration;
using System.IO;
using ErucaCRM.Utility.WebClasses;
using AutoMapper;
using ErucaCRM.Web.Infrastructure;
using System.Drawing;
using System.Data;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
namespace ErucaCRM.Web.Controllers
{
    [CustomAuthorize(Roles = Constants.AUTHENTICATION_ROLE_USER)]
    [CultureModuleManagement]
    public class UserController : Controller
    {
        //
        // GET: /User/

        private IUserBusiness userBusiness;
        private IAccountCaseBusiness accountcaseBusiness;
        private IContactBusiness contactBusiness;
        private IAccountBusiness accountBussiness;
        private IRoleBusiness roleBusiness;
        private IProfileBusiness profileBusiness;
        private IProfilePermissionBusiness profilePermissionBusiness;
        private ILeadBusiness leadBusiness;
        private ILeadSourceBusiness leadSourceBusiness;
        private IIndustryBusiness industryBusiness;
        private ILeadStatusBusiness leadStatusBusiness;
        private IFileAttachmentBusiness fileAttachmentBusiness;
        private IProductBusiness productBusiness;
        private IQuoteBusiness quoteBusiness;
        private ITaskItemBusiness taskBusiness;
        private IProductLeadAssociationBusiness productLeadAssociationBusiness;
        private IProductQuoteAssociationBusiness productQuoteAssociationBusiness;
        private ISalesOrderBusiness salesOrderBusiness;
        private IInvoiceBusiness invoiceBusiness;
        private ICultureInformationBusiness cultureBusiness;
        private ITimeZoneBusiness timeZoneBusiness;
        private ITagBusiness tagBusiness;
        private IStageBusiness stageBusiness;
        private ICaseMessageBoardBusiness caseMessageBoardBusiness;
        private ILeadAuditBusiness leadAuditBusiness;
        private IRatingBusiness ratingBusiness;
        private ITaskItemBusiness taskItemBusiness;
        private IReportBusiness reportBusiness;
        private ILeadCommentBussiness leadCommentBussiness;
        private IRealTimeNotificationBusiness realTimeNotificationBusiness;
        private IHomeBusiness homeBusiness;

        public UserController(IUserBusiness _userBusiness, IRoleBusiness _roleBusiness, IProfileBusiness _profileBusiness, IProfilePermissionBusiness _profilePermissionBusiness, ILeadBusiness _leadBusiness, ILeadSourceBusiness _leadSourceBusiness, IIndustryBusiness _industryBusiness, ILeadStatusBusiness _leadStatusBusiness, IContactBusiness _contactBusiness, IFileAttachmentBusiness _fileAttachmentBusiness, IProductBusiness _productBusiness, ITaskItemBusiness _taskBusiness, IQuoteBusiness _quoteBusiness, ISalesOrderBusiness _salesOrderBusiness, IProductLeadAssociationBusiness _productLeadAssociationBusiness, IInvoiceBusiness _invoiceBusiness, IProductQuoteAssociationBusiness _productQuoteAssociationBusiness, ICultureInformationBusiness _cultureBusiness, ITimeZoneBusiness _timeZoneBusiness, IAccountBusiness _accountBussiness, IAccountCaseBusiness _accountcaseBusiness, ITagBusiness _tagBusiness, IStageBusiness _stageBusiness, ILeadAuditBusiness _leadAuditBusiness, ICaseMessageBoardBusiness _caseMessageBoardBusiness, IRatingBusiness _ratingBusiness, ITaskItemBusiness _taskItemBusiness, IReportBusiness _reportBusiness, IHomeBusiness _homeBusiness, ILeadCommentBussiness _leadCommentBussiness, IRealTimeNotificationBusiness _realTimeNotificationBusiness)
        {
            realTimeNotificationBusiness = _realTimeNotificationBusiness;
            userBusiness = _userBusiness;
            roleBusiness = _roleBusiness;
            profileBusiness = _profileBusiness;
            profilePermissionBusiness = _profilePermissionBusiness;
            leadBusiness = _leadBusiness;
            leadSourceBusiness = _leadSourceBusiness;
            industryBusiness = _industryBusiness;
            leadStatusBusiness = _leadStatusBusiness;
            contactBusiness = _contactBusiness;
            accountBussiness = _accountBussiness;
            accountcaseBusiness = _accountcaseBusiness;
            fileAttachmentBusiness = _fileAttachmentBusiness;
            productBusiness = _productBusiness;
            quoteBusiness = _quoteBusiness;
            taskBusiness = _taskBusiness;
            productLeadAssociationBusiness = _productLeadAssociationBusiness;
            salesOrderBusiness = _salesOrderBusiness;
            invoiceBusiness = _invoiceBusiness;
            productQuoteAssociationBusiness = _productQuoteAssociationBusiness;
            cultureBusiness = _cultureBusiness;
            timeZoneBusiness = _timeZoneBusiness;
            tagBusiness = _tagBusiness;
            stageBusiness = _stageBusiness;
            caseMessageBoardBusiness = _caseMessageBoardBusiness;
            leadAuditBusiness = _leadAuditBusiness;
            ratingBusiness = _ratingBusiness;
            taskItemBusiness = _taskItemBusiness;
            reportBusiness = _reportBusiness;
            homeBusiness = _homeBusiness;
            leadCommentBussiness = _leadCommentBussiness;
        }


        [HttpPost]
        public ActionResult ChangePassword(string CurrentPassword, string NewPassword)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Success.ToString();
            response.StatusCode = 500;

            if (userBusiness.ChangePassword((int)SessionManagement.LoggedInUser.UserId, CurrentPassword, NewPassword))
                response.Message = "true";
            else
                response.Message = "false";

            //  response.Status = "Success";
            return Json(response);
        }

        [System.Web.Mvc.Authorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult UserProfile(string id_encrypted)
        {
            UserProfileVM profile = new UserProfileVM();
            int userId = Convert.ToInt32(id_encrypted);// (int)SessionManagement.LoggedInUser.UserId;
            UserModel userModel = userBusiness.GetUserByUserId(userId);
            Mapper.Map(userModel, profile);
            if (profile.Address.Street != null)
            {
                profile.AddressDetails = profile.Address.Street;
            }
            if (profile.Address.City != null)
            {
                profile.AddressDetails = profile.AddressDetails + (profile.AddressDetails != null ? (",") : ("")) + profile.Address.City;
            }
            if (profile.Address.Country.CountryName != null)
            {
                profile.AddressDetails = profile.AddressDetails + (profile.AddressDetails != null ? (",") : ("")) + profile.Address.Country.CountryName;
            }
            if (profile.Address.Zipcode != null)
            {
                profile.AddressDetails = profile.AddressDetails + (profile.AddressDetails != null ? (",") : ("")) + profile.Address.Zipcode;
            }
            ViewBag.UserID = userId;
            return View(profile);
        }

        [System.Web.Mvc.Authorize()]
        [EncryptedActionParameter]
        public ActionResult MyProfile(string id_encrypted)
        {
            UserProfileVM profile = new UserProfileVM();
            int userId = Convert.ToInt32(id_encrypted);// (int)SessionManagement.LoggedInUser.UserId;
            UserModel userModel = userBusiness.GetUserByUserId(userId);
            Mapper.Map(userModel, profile);
            if (profile.Address.Street != null)
            {
                profile.AddressDetails = profile.Address.Street;
            }
            if (profile.Address.City != null)
            {
                profile.AddressDetails = profile.AddressDetails + (profile.AddressDetails != null ? (",") : ("")) + profile.Address.City;
            }
            if (profile.Address.Country.CountryName != null)
            {
                profile.AddressDetails = profile.AddressDetails + (profile.AddressDetails != null ? (",") : ("")) + profile.Address.Country.CountryName;
            }
            if (profile.Address.Zipcode != null)
            {
                profile.AddressDetails = profile.AddressDetails + (profile.AddressDetails != null ? (",") : ("")) + profile.Address.Zipcode;
            }
            ViewBag.UserID = userId;
            return View("UserProfile", profile);
        }


        [EncryptedActionParameter]
        public ActionResult EditUserSelfProfile(string id_encrypted)
        {
            ViewBag.ErrorMessage = "";
            UserProfileVM profile = new UserProfileVM();
            int userId = Convert.ToInt32(id_encrypted);// (int)SessionManagement.LoggedInUser.UserId;
            UserModel userModel = userBusiness.GetUserByUserId(userId);
            Mapper.Map(userModel, profile);
            ViewBag.CountryList = GetCountries();

            ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;
            ViewBag.CultureInformationList = GetCultureList();
            ViewBag.TimeZoneList = GetTimeZoneList();
            ViewBag.UserTimeZoneId = userModel.TimeZoneId;
            ViewBag.UserCultureInfoId = userModel.CultureInformationId;
            if (userId != (int)SessionManagement.LoggedInUser.UserId)
            {
                ViewBag.RolesList = GetRoleList();
                ViewBag.ProfileList = ProfileStaffUsersList();
            }
            return View("EditUserProfile", profile);
        }

        [HttpPost]
        public ActionResult EditUserSelfProfile(UserProfileVM profileUpdate)
        {
            UserModel userModel = new UserModel();
            if (ModelState["Profile.ProfileName"] != null)
                ModelState["Profile.ProfileName"].Errors.Clear();

            ViewBag.ErrorMessage = "";
            int userId = profileUpdate.UserId.Decrypt();
            if (ModelState.IsValid)
            {

                Mapper.Map(profileUpdate, userModel);
                if (userId == (int)SessionManagement.LoggedInUser.UserId)
                {
                    userModel = userBusiness.UpdateUser(userModel);
                    SessionManagement.LoggedInUser.UserName = profileUpdate.FirstName + " " + profileUpdate.LastName;
                    SessionManagement.LoggedInUser.FullName = profileUpdate.FirstName + " " + profileUpdate.LastName;
                    SessionManagement.LoggedInUser.EmailId = profileUpdate.EmailId;
                    SessionManagement.LoggedInUser.CurrentCulture = userModel.CultureInformationModel.CultureName;
                    SessionManagement.LoggedInUser.TimeZoneOffSet = userModel.TimeZoneModel.offset;
                }
                else
                {
                    ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;
                    ViewBag.CultureInformationList = GetCultureList();
                    ViewBag.TimeZoneList = GetTimeZoneList();
                    ViewBag.UserTimeZoneId = userModel.TimeZoneId;
                    ViewBag.UserCultureInfoId = userModel.CultureInformationId;
                    ViewBag.CountryList = GetCountries();

                    if (userBusiness.IsEmailExist(profileUpdate.EmailId, userId))
                    {
                        //ViewBag.ErrorMessage = "Email id already exist, please enter different email id";
                        ModelState.AddModelError("EmailId", CommonFunctions.GetGlobalizedLabel("User", "EmailAlreadyExists"));// "Email already exists, please try another one.");
                        ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;



                        if (userId != (int)SessionManagement.LoggedInUser.UserId)
                        {
                            ViewBag.RolesList = GetRoleList();
                            ViewBag.ProfileList = ProfileStaffUsersList();
                        }

                        return View(profileUpdate);
                    }
                    userBusiness.UpdateUserData(userModel);

                }
            }
            else
            {


                if (userId != (int)SessionManagement.LoggedInUser.UserId)
                {
                    ViewBag.RolesList = GetRoleList();
                    ViewBag.ProfileList = ProfileStaffUsersList();
                }

                return View(profileUpdate);
            }


            return RedirectToAction("MyProfile/" + profileUpdate.UserId);
        }


        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameter]
        public ActionResult EditUserProfile(string id_encrypted)
        {
            ViewBag.ErrorMessage = "";
            UserProfileVM profile = new UserProfileVM();
            int userId = Convert.ToInt32(id_encrypted);// (int)SessionManagement.LoggedInUser.UserId;
            UserModel userModel = userBusiness.GetUserByUserId(userId);
            Mapper.Map(userModel, profile);
            ViewBag.CountryList = GetCountries();

            ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;
            ViewBag.CultureInformationList = GetCultureList();
            ViewBag.TimeZoneList = GetTimeZoneList();
            ViewBag.UserTimeZoneId = userModel.TimeZoneId;
            ViewBag.UserCultureInfoId = userModel.CultureInformationId;
            if (userId != (int)SessionManagement.LoggedInUser.UserId)
            {
                ViewBag.RolesList = GetRoleList();
                ViewBag.ProfileList = ProfileStaffUsersList();
            }
            return View(profile);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult EditUserProfile(UserProfileVM profileUpdate)
        {
            UserModel userModel = new UserModel();
            if (ModelState["Profile.ProfileName"] != null)
                ModelState["Profile.ProfileName"].Errors.Clear();

            ViewBag.ErrorMessage = "";
            int userId = profileUpdate.UserId.Decrypt();
            if (ModelState.IsValid)
            {

                Mapper.Map(profileUpdate, userModel);
                if (userId == (int)SessionManagement.LoggedInUser.UserId)
                {
                    userModel = userBusiness.UpdateUser(userModel);
                    SessionManagement.LoggedInUser.UserName = profileUpdate.FirstName + " " + profileUpdate.LastName;
                    SessionManagement.LoggedInUser.CurrentCulture = userModel.CultureInformationModel.CultureName;
                }
                else
                {
                    ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;
                    ViewBag.CultureInformationList = GetCultureList();
                    ViewBag.TimeZoneList = GetTimeZoneList();
                    ViewBag.UserTimeZoneId = userModel.TimeZoneId;
                    ViewBag.UserCultureInfoId = userModel.CultureInformationId;
                    ViewBag.CountryList = GetCountries();

                    if (userBusiness.IsEmailExist(profileUpdate.EmailId, userId))
                    {
                        //ViewBag.ErrorMessage = "Email id already exist, please enter different email id";
                        ModelState.AddModelError("EmailId", CommonFunctions.GetGlobalizedLabel("User", "EmailAlreadyExists"));// "Email already exists, please try another one.");
                        ViewBag.LoggedInUser = (int)SessionManagement.LoggedInUser.UserId;



                        if (userId != (int)SessionManagement.LoggedInUser.UserId)
                        {
                            ViewBag.RolesList = GetRoleList();
                            ViewBag.ProfileList = ProfileStaffUsersList();
                        }

                        return View(profileUpdate);
                    }
                    userBusiness.UpdateUserData(userModel);

                }
            }
            else
            {


                if (userId != (int)SessionManagement.LoggedInUser.UserId)
                {
                    ViewBag.RolesList = GetRoleList();
                    ViewBag.ProfileList = ProfileStaffUsersList();
                }

                return View(profileUpdate);
            }


            return RedirectToAction("UserProfile/" + profileUpdate.UserId);
        }

        public List<CountryVM> GetCountries()
        {
            List<CountryModel> listCountryModel = userBusiness.GetCountries();
            List<CountryVM> listCountryVM = new List<CountryVM>();
            Mapper.Map(listCountryModel, listCountryVM);
            listCountryVM.Insert(0, new CountryVM
            {
                CountryId = 0,
                OtherCountryId = 0,
                CountryName = CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION)
            });
            return listCountryVM;
        }

        /// <summary>
        /// Method will return all contact Owners by companyID
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public IList<OwnerListModel> GetContactOwners(int companyID)
        {
            // Creating dropdown for Owners
            IList<OwnerListModel> ownersModelList = userBusiness.GetAllOwnerDropdownList(companyID, true, 0).ToList();

            return ownersModelList;
        }


        public List<CountryVM> GetOtherCountries()
        {
            List<CountryModel> listCountryModel = userBusiness.GetCountries();
            List<CountryVM> listCountryVM = new List<CountryVM>();
            Mapper.Map(listCountryModel, listCountryVM);
            listCountryVM.ForEach(x => x.OtherCountryId = x.CountryId);
            listCountryVM.Insert(0, new CountryVM
            {
                CountryId = 0,
                OtherCountryId = 0,
                CountryName = CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION)
            });
            return listCountryVM;
        }

        [CustomAuthorize(Roles = Constants.MODULE_ROLE + Constants.PERMISSION_VIEW)]
        public List<RoleVM> GetRoleList()
        {
            List<RoleVM> listRoleVM = new List<RoleVM>();
            List<RoleModel> listRoleModel = new List<RoleModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listRoleModel = roleBusiness.GetRoleByCompanyId(companyId).ToList();
            listRoleModel.Insert(0, new RoleModel
            {
                RoleId = 0,
                RoleName = CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION)
            });
            Mapper.Map(listRoleModel, listRoleVM);
            listRoleVM.ForEach(r => r.RoleId = (r.RoleId == "0" ? "" : r.RoleId));
            return listRoleVM;

        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public List<ProfileVM> ProfileList()
        {
            List<ProfileVM> listProfileVM = new List<ProfileVM>();
            List<ProfileModel> listProfileModel = new List<ProfileModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listProfileModel = profileBusiness.GetProfileByCompanyId(companyId);
            Mapper.Map(listProfileModel, listProfileVM);
            return listProfileVM;
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public List<ProfileVM> ProfileStaffUsersList()
        {
            return ProfileList();
        }

        public List<Title> GetTitleList()
        {
            List<Title> listTitles = new List<Title>();
            listTitles.Add(new Title() { TitleName = "Mr." });
            listTitles.Add(new Title() { TitleName = "Mrs." });
            listTitles.Add(new Title() { TitleName = "Ms." });
            listTitles.Add(new Title() { TitleName = "Dr." });
            listTitles.Add(new Title() { TitleName = "Prof." });
            return listTitles;
        }

        [HttpPost]
        public virtual ActionResult UploadImage()
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { statusCode = 500, status = "No image provided." }, "text/html");
            }
            var file = Request.Files[0];
            var userId = Request.Form["UserId"].Decrypt();

            try
            {
                int imageWidth = ReadConfiguration.ProfileImageWidth;
                int imageHeight = ReadConfiguration.ProfileImageHieght;
                var fileExtension = Path.GetExtension(file.FileName);
                string imageName = Constants.PROFILE_IMAGE_NAME_PREFIX + userId + fileExtension;
                string imagePathAndName = Constants.PROFILE_IMAGE_PATH + imageName;

                if (SessionManagement.LoggedInUser.UserId == userId)
                //SessionManagement.LoggedInUser.ProfileImageUrl = profilePath;
                //string ImageSavingPath = Server.MapPath(@"~" + imagePathAndName);
                CommonFunctions.UploadFile(file, imageName, "", Constants.PROFILE_IMAGE_BLOB);
                string profilePath = ReadConfiguration.DownLoadPath + Constants.PROFILE_IMAGE_BLOB + "/" + imageName;
                SessionManagement.LoggedInUser.ProfileImageUrl = profilePath;
                userBusiness.ChangeImage(userId, profilePath);
                // Return JSON
                return Json(new
                {
                    statusCode = 200,
                    status = "Image uploaded.",
                    file = profilePath,
                }, "text/html");
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    statusCode = 500,
                    status = "Error uploading image.",
                    file = string.Empty
                }, "text/html");
            }
        }

        public ActionResult Home()
        {



            if (SessionManagement.LoggedInUser.UserId != 0)
            {
                return View(new HomeVM());
            }
            else
            {
                return RedirectToAction("Index", "Site");
            }
        }

        public ActionResult LogOut()
        {

            SessionManagement.LoggedInUser = null;
            Session.Abandon();
            Session.RemoveAll();
            CommonFunctions.RemoveCookies();
            return RedirectToAction("Index", "Site");
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE)]

        public ActionResult AddUser()
        {
            ViewBag.ErrorMessage = "";
            //ViewBag.ModuleName = "AddUser";
            UserVM objUserVModel = new UserVM();
            ViewBag.CountryList = GetCountries();
            List<RoleVM> _roleList = GetRoleList();
            List<ProfileVM> _profileList = ProfileStaffUsersList();

            //RoleVM _objDefaultRole = _roleList.FirstOrDefault(x => (x.IsDefaultForStaffUser ?? false) == true && (x.IsDefaultForRegisterdUser ?? false) == false);
            //if (_objDefaultRole != null)
            //    objUserVModel.Role.RoleId = _objDefaultRole.RoleId;
            ProfileVM _objDefaultProfile = _profileList.FirstOrDefault(x => (x.IsDefaultForStaffUser ?? false) == true && (x.IsDefaultForRegisterdUser ?? false) == false);
            if (_objDefaultProfile != null)
            {
                objUserVModel.Profile.ProfileId = _objDefaultProfile.ProfileId;
            }
            System.TimeZone localZone = System.TimeZone.CurrentTimeZone;
            string offset = localZone.GetUtcOffset(DateTime.UtcNow).ToString();
            ViewBag.RoleList = _roleList;
            ViewBag.ProfileList = _profileList;
            ViewBag.CultureInformationList = GetCultureList();
            ViewBag.TimeZoneList = GetTimeZoneList();
            return View(objUserVModel);

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_CREATE)]
        public ActionResult AddUser(UserVM userVM)
        {
            ViewBag.ErrorMessage = "";

            if (ModelState["Profile.ProfileName"] != null)
                ModelState["Profile.ProfileName"].Errors.Clear();

            if (ModelState.IsValid)
            {

                if (userBusiness.IsEmailExist(userVM.EmailId, null))
                {
                    //ViewBag.ErrorMessage ="" "Email id already exist, please enter different email id";
                    ModelState.AddModelError("EmailId", CommonFunctions.GetGlobalizedLabel("User", "EmailAlreadyExists"));//  "Email already exists, please try another one.");
                    ViewBag.CountryList = GetCountries();
                    ViewBag.RoleList = GetRoleList();
                    ViewBag.ProfileList = ProfileStaffUsersList();
                    ViewBag.CultureInformationList = GetCultureList();
                    ViewBag.TimeZoneList = GetTimeZoneList();
                    return View(userVM);
                }

                UserModel userModel = new UserModel();
                Mapper.Map(userVM, userModel);
                CompanyModel companymodel = new CompanyModel();
                companymodel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                userModel.CompanyModel = companymodel;
                string emailSubject = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "AddStaffUserEmailSubject");
                string emailBody = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "AddStaffUserEmailBody");
                userBusiness.AddStaffUser(userModel, emailSubject, emailBody);
                return RedirectToAction("UserList", "User");
            }
            ViewBag.CountryList = GetCountries();
            ViewBag.RoleList = GetRoleList();
            ViewBag.ProfileList = ProfileStaffUsersList();
            return View(userVM);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult UpdateUserStatus(List<UseListInfo> listUserInfo)
        {
            string response = "";
            if (listUserInfo.Count > 0)
            {
                int[] userIDs = listUserInfo.Select(x => x.UserID.Decrypt()).ToArray();
                Boolean currentUserStatus = listUserInfo.FirstOrDefault().UserStatus == "Active" ? true : false;
                Boolean newUserStatus = (!currentUserStatus);
                int ownerUserID = (int)SessionManagement.LoggedInUser.UserId;
                response = userBusiness.UpdateUserStatus(userIDs, newUserStatus, ownerUserID);
            }
            return Json(
                        response
                     , JsonRequestBehavior.AllowGet
                     );
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult EditUser(int id)
        {

            UserModel objUserModel = userBusiness.GetUserByUserId(id);
            UserVM objUserVModel = new UserVM();
            Mapper.Map(objUserModel, objUserVModel);
            if (objUserVModel != null)
                objUserVModel.ImageURL = objUserVModel.ImageURL ?? "NoImage.jpg";
            ViewBag.CountryList = GetCountries();
            ViewBag.DesignationList = GetRoleList();
            ViewBag.ProfileList = ProfileList();

            return View(objUserVModel);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_EDIT)]
        public ActionResult EditUser(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                UserModel userModel = new UserModel();
                Mapper.Map(userVM, userModel);

                userBusiness.UpdateUserData(userModel);

                return RedirectToAction("UserList", "User");
            }
            else
            {
                ViewBag.CountryList = GetCountries();
                ViewBag.DesignationList = GetRoleList();
                ViewBag.ProfileList = ProfileList();
                return View(userVM);
            }
        }
        [EncryptedActionParameterAttribute]
        public ActionResult Account(string id_encrypted, string parentId_encrypted)
        {
            AccountVM accountVM = new AccountVM();
            AccountModel accountModel = new AccountModel();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;

            if (id_encrypted != null)
            {
                accountModel = accountBussiness.AccountDetail(Convert.ToInt32(id_encrypted));

                AutoMapper.Mapper.Map(accountModel, accountVM);
            }
            if (!string.IsNullOrEmpty(parentId_encrypted))
            {
                accountVM.ParentAccountId = Convert.ToInt32(parentId_encrypted);
                ViewBag.FromParentAccount = "true";
            }
            accountVM.ParentAccountList = accountBussiness.GetParentAccountListByCompanyID(companyId);
            accountVM.AccountTypeList = accountBussiness.GetAccountTypes();
            accountVM.AccountOwnerList = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            accountVM.IndustryList = industryBusiness.GetAllIndustriesDropdownList();
            accountVM.CountryList = GetCountries();

            return View(accountVM);
        }

        [HttpPost]
        public ActionResult Account(AccountVM accountVM)
        {
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;
            if (ModelState.IsValid)
            {
                accountVM.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                accountVM.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                accountVM.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                AccountModel accountModel = new AccountModel();
                AutoMapper.Mapper.Map(accountVM, accountModel);
                accountBussiness.AddUpdateAccount(accountModel);
                if (string.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"]) == false)
                {
                    string returnUrl = string.Empty;
                    returnUrl = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"];
                    return Redirect(returnUrl);
                }
                if (!string.IsNullOrEmpty(Request.Form["FromParentAccount"]))
                {
                    return RedirectToAction("AccountDetail", "User", new { id_encrypted = accountVM.ParentAccountId.Value.Encrypt() });
                }
                if (accountVM.AccountId == null)
                    return RedirectToAction("Accounts", "User");
                else
                {
                    return RedirectToAction("AccountDetail/" + accountVM.AccountId, "User");
                }

            }
            else
            {
                accountVM.ParentAccountList = accountBussiness.GetParentAccountListByCompanyID(companyId);
                accountVM.AccountTypeList = accountBussiness.GetAccountTypes();
                accountVM.AccountOwnerList = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
                accountVM.IndustryList = industryBusiness.GetAllIndustriesDropdownList();
                accountVM.CountryList = GetCountries();
            }

            return View(accountVM);
        }





        [CustomAuthorize(Roles = Constants.MODULE_ACCOUNT + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameterAttribute]
        public ActionResult AccountDetail(string Id_encrypted)
        {
            int accountId = Convert.ToInt32(Id_encrypted);
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<SalesOrderVM> lstSaleOrderVm = new List<SalesOrderVM>();
            List<AccountContactVM> listAccountContact = new List<AccountContactVM>();
            List<AccountVM> listChildAccountVM = new List<AccountVM>();
            AccountVM accountVM = new AccountVM();
            AccountModel accountModel = new AccountModel();
            accountModel = accountBussiness.AccountDetail(accountId);
            List<SalesOrderModel> listSalesOrdersModels = new List<SalesOrderModel>();
            List<AccountContactModel> listAccountContactModel = new List<AccountContactModel>();
            List<AccountModel> listchildAccount = new List<AccountModel>();
            listchildAccount = accountBussiness.GetChildAccount(companyId, accountId);
            listAccountContactModel = contactBusiness.GetContactByAccountId(SessionManagement.LoggedInUser.UserId, companyId, accountId);
            listSalesOrdersModels = salesOrderBusiness.GetSalesOrderList(companyId, accountId);
            Mapper.Map(listSalesOrdersModels, lstSaleOrderVm);
            Mapper.Map(accountModel, accountVM);
            Mapper.Map(listAccountContactModel, listAccountContact);
            Mapper.Map(listchildAccount, listChildAccountVM);
            accountVM.SalesOrders = lstSaleOrderVm;
            accountVM.AccountContact = listAccountContact;
            accountVM.ChildAccounts = listChildAccountVM;

            if (accountModel.ParentAccountId.HasValue && accountModel.ParentAccountId.Value > 0)
            {
                AccountModel parentAccountModel = new AccountModel();
                parentAccountModel = accountBussiness.AccountDetail(accountModel.ParentAccountId.Value);
                accountVM.ParentAccountName = parentAccountModel.AccountName;
            }
            ViewBag.AccountId = accountVM.AccountId;
            return View(accountVM);

            //return View(new AccountVM());
        }


        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW + "," + Constants.MODULE_ACCOUNT + Constants.PERMISSION_VIEW)]
        public ActionResult Accounts()
        {
            return View(new AccountVM());

        }

        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW + "," + Constants.MODULE_ACCOUNT + Constants.PERMISSION_VIEW)]
        public ActionResult AccountAndContact()
        {
            return View(new AccountVM());
        }


        // [CustomAuthorize(Roles = Constants.MODULE_CASE + Constants.PERMISSION_VIEW)]
        //redirectedTo is set to redirect page back to callin page if it is called from outside
        [EncryptedActionParameter]
        public ActionResult AccountCase(string accountId_encrypted = "", string caseId_encrypted = "", string redirectedTo = "")
        {

            AccountCaseVM cases = new AccountCaseVM();
            AccountCaseModel caseModel = new AccountCaseModel();
            Owner owner = null;

            if (!string.IsNullOrEmpty(redirectedTo) && redirectedTo == ErucaCRM.Utility.Constants.MODULE_ACCOUNT)
            {
                ViewBag.RedirectTo = accountId_encrypted;

            }
            // Edit Case
            if (Convert.ToInt32(caseId_encrypted) > 0)
            {
                caseModel = accountcaseBusiness.GetCase(Convert.ToInt32(Convert.ToInt32(caseId_encrypted)));
                if (caseModel != null)
                {
                    AutoMapper.Mapper.Map(caseModel, cases);
                    //cases.CaseOwnerId = Convert.ToInt32(caseModel.CaseOwnerId).ToString();
                    //cases.CaseOwnerName = accountcaseBusiness.GetCaseOwnerName(caseModel.CaseOwnerId);

                }
            }
            // Add Case
            else
            {

                cases.CaseOwnerId = SessionManagement.LoggedInUser.UserId;
                cases.CaseOwnerName = SessionManagement.LoggedInUser.FullName;

                //cases.Status =1;
                //cases.DueDate = DateTime.UtcNow;
            }

            // Creating dropdown for Owners
            IList<UserModel> ownersModelList = userBusiness.GetAllActiveUsers((int)SessionManagement.LoggedInUser.CompanyId).ToList();
            IList<Owner> ownersVMList = new List<Owner>();

            foreach (var o in ownersModelList)
            {
                owner = new Owner();
                owner.OwnerId = o.UserId;
                owner.OwnerName = o.FirstName + " " + o.LastName;
                ownersVMList.Add(owner);
            }
            ViewBag.CaseOwners = ownersVMList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            cases.AccountId = string.IsNullOrEmpty(accountId_encrypted) ? 0 : Convert.ToInt32(accountId_encrypted);
            ViewBag.AccountList = accountBussiness.GetAccountListByCompanyID(companyId, SessionManagement.LoggedInUser.UserId);
            IList<Utility.WebClasses.Priority> priorities = new List<Utility.WebClasses.Priority>();
            Priority priority = null;
            Array priorityvalues = Enum.GetValues(typeof(Utility.Enums.CasePriority));

            foreach (Utility.Enums.CasePriority item in priorityvalues)
            {
                priority = new Priority();
                priority.PriorityId = (int)item;
                priority.PriorityName = Enum.GetName(typeof(Utility.Enums.CasePriority), item);
                priorities.Add(priority);
            }

            ViewBag.Priorities = priorities;

            IList<Utility.WebClasses.Origin> origins = new List<Utility.WebClasses.Origin>();
            Origin origin = null;
            Array originvalues = Enum.GetValues(typeof(Utility.Enums.CaseOrigin));

            foreach (Utility.Enums.CaseOrigin item in originvalues)
            {
                origin = new Origin();
                origin.OriginId = (int)item;
                origin.OriginName = Enum.GetName(typeof(Utility.Enums.CaseOrigin), item);
                origins.Add(origin);
            }

            ViewBag.origins = origins;

            IList<Utility.WebClasses.CaseType> casetypes = new List<Utility.WebClasses.CaseType>();
            CaseType casetype = null;
            Array casetypevalues = Enum.GetValues(typeof(Utility.Enums.CaseType));

            foreach (Utility.Enums.CaseType item in casetypevalues)
            {
                casetype = new CaseType();
                casetype.CaseTypeId = (int)item;
                casetype.CaseTypeName = Enum.GetName(typeof(Utility.Enums.CaseType), item);
                casetypes.Add(casetype);
            }

            ViewBag.casetypes = casetypes;
            IList<Utility.WebClasses.Status> statuses = new List<Utility.WebClasses.Status>();
            Status status = null;
            Array statusvalues = Enum.GetValues(typeof(Utility.Enums.CaseStaus));

            foreach (Utility.Enums.CaseStaus item in statusvalues)
            {
                status = new Status();
                status.StatusId = (int)item;
                status.StatusName = Enum.GetName(typeof(Utility.Enums.CaseStaus), item);
                statuses.Add(status);
            }

            ViewBag.statuses = statuses;

            return View(cases);
        }


        [HttpPost]
        public ActionResult AccountCase(AccountCaseVM caseVM)
        {
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;
            if (ModelState.IsValid)
            {
                //caseVM.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                int userId = SessionManagement.LoggedInUser.UserId;
                AccountCaseModel caseModel = new AccountCaseModel();
                AutoMapper.Mapper.Map(caseVM, caseModel);
                if (string.IsNullOrEmpty(caseVM.AccountCaseId))
                {
                    caseModel.CreatedBy = userId;
                }
                else
                {
                    caseModel.ModifiedBy = userId;
                }

                accountcaseBusiness.AddUpdateCase(caseModel, companyId);
                if (string.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"]) == false)
                {
                    string returnUrl = string.Empty;
                    returnUrl = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"];
                    return Redirect(returnUrl);
                }
                if (!string.IsNullOrEmpty(Request.Form["RedirectTo"]))
                {

                    return RedirectToAction("AccountDetail", "User", new { id_encrypted = caseVM.AccountId.Encrypt() });

                }

                if (caseVM.AccountCaseId == null)
                    return RedirectToAction("AccountCases", "User");
                else
                {
                    return RedirectToAction("AccountCaseDetail", "User", new { Id_encrypted = caseVM.AccountCaseId });
                }

            }
            else
            {
                ViewBag.AccountList = accountBussiness.GetAccountListByCompanyID(companyId, SessionManagement.LoggedInUser.UserId);
                IList<Utility.WebClasses.Priority> priorities = new List<Utility.WebClasses.Priority>();
                Priority priority = null;
                Array priorityvalues = Enum.GetValues(typeof(Utility.Enums.CasePriority));

                foreach (Utility.Enums.CasePriority item in priorityvalues)
                {
                    priority = new Priority();
                    priority.PriorityId = (int)item;
                    priority.PriorityName = Enum.GetName(typeof(Utility.Enums.CasePriority), item);
                    priorities.Add(priority);
                }

                ViewBag.Priorities = priorities;
                IList<UserModel> ownersModelList = userBusiness.GetAllUsers((int)SessionManagement.LoggedInUser.CompanyId).ToList();
                IList<Owner> ownersVMList = new List<Owner>();
                Owner owner = null;
                foreach (var o in ownersModelList)
                {
                    owner = new Owner();
                    owner.OwnerId = o.UserId;
                    owner.OwnerName = o.FirstName + " " + o.LastName;
                    ownersVMList.Add(owner);
                }
                ViewBag.CaseOwners = ownersVMList;
                IList<Utility.WebClasses.Status> statuses = new List<Utility.WebClasses.Status>();
                Status status = null;
                Array statusvalues = Enum.GetValues(typeof(Utility.Enums.CaseStaus));

                foreach (Utility.Enums.CaseStaus item in statusvalues)
                {
                    status = new Status();
                    status.StatusId = (int)item;
                    status.StatusName = Enum.GetName(typeof(Utility.Enums.CaseStaus), item);
                    statuses.Add(status);
                }

                ViewBag.statuses = statuses;
                IList<Utility.WebClasses.Origin> origins = new List<Utility.WebClasses.Origin>();
                Origin origin = null;
                Array originvalues = Enum.GetValues(typeof(Utility.Enums.CaseOrigin));

                foreach (Utility.Enums.CaseOrigin item in originvalues)
                {
                    origin = new Origin();
                    origin.OriginId = (int)item;
                    origin.OriginName = Enum.GetName(typeof(Utility.Enums.CaseOrigin), item);
                    origins.Add(origin);
                }

                ViewBag.origins = origins;

                IList<Utility.WebClasses.CaseType> casetypes = new List<Utility.WebClasses.CaseType>();
                CaseType casetype = null;
                Array casetypevalues = Enum.GetValues(typeof(Utility.Enums.CaseType));

                foreach (Utility.Enums.CaseType item in casetypevalues)
                {
                    casetype = new CaseType();
                    casetype.CaseTypeId = (int)item;
                    casetype.CaseTypeName = Enum.GetName(typeof(Utility.Enums.CaseType), item);
                    casetypes.Add(casetype);
                }

                ViewBag.casetypes = casetypes;
            }

            return View(caseVM);
        }

        [EncryptedActionParameter]
        public ActionResult AccountCaseDetail(string Id_encrypted)
        {
            AccountCaseVM accountCaseVM = new AccountCaseVM();
            AccountCaseModel accountCaseModel = new AccountCaseModel();
            if (Convert.ToInt32(Id_encrypted) > 0)
            {
                accountCaseModel = accountcaseBusiness.GetCase(Convert.ToInt32(Convert.ToInt32(Id_encrypted)));
                if (accountCaseModel != null)
                {
                    AutoMapper.Mapper.Map(accountCaseModel, accountCaseVM);
                    //cases.CaseOwnerId = Convert.ToInt32(caseModel.CaseOwnerId).ToString();
                    // accountCaseVM.CaseOwnerName = accountcaseBusiness.GetCaseOwnerName(accountCaseModel.CaseOwnerId);

                }
            }
            return View(accountCaseVM);
        }
        [HttpPost]
        public JsonResult SaveMessageBoardMessage(CaseMessageBoardVM caseMessageBoardVM)
        {

            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            var userId = SessionManagement.LoggedInUser.UserId;
            var userName = SessionManagement.LoggedInUser.FullName;
            var encrtyptedUserId = userId.Encrypt();
            var caseMessageBoardId = caseMessageBoardVM.CaseMessageBoardId.Decrypt();
            CaseMessageBoardModel caseMessageBoardModel = new CaseMessageBoardModel();
            AutoMapper.Mapper.Map(caseMessageBoardVM, caseMessageBoardModel);
            caseMessageBoardModel.CreatedBy = userId;
            caseMessageBoardModel.FileAttachments = PrepareFileAttachmentList(caseMessageBoardVM.FileAttachments);
            caseMessageBoardModel = caseMessageBoardBusiness.Add(caseMessageBoardModel);
            caseMessageBoardId = caseMessageBoardModel.CaseMessageBoardId;

            // List<FileAttachmentVM> fileAttachmentVMList = UploadMessageBoardDocument(caseMessageBoardId, userId);

            response.Status = Enums.ResponseResult.Success.ToString();
            return Json(new { response = response, CaseMessageBoardId = caseMessageBoardModel.CaseMessageBoardId.Encrypt(), Description = caseMessageBoardVM.Description, CreatedByName = userName, UserId = encrtyptedUserId, CreatedbyUserImage = SessionManagement.LoggedInUser.ProfileImageUrl, CreatedDateTimeText = caseMessageBoardModel.CreatedDateTimeText, CreatedbyUserName = SessionManagement.LoggedInUser.FullName });
            //return Json(response);
        }
        [HttpPost]
        public JsonResult UploadMessageBoardDocument(CaseMessageBoardVM caseMessageBoardVM)
        {
            Response response = new Response();

            response.Status = Enums.ResponseResult.Failure.ToString();

            int caseMessageBoardId = 0;

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

                string uniqueName = Guid.NewGuid().ToShortGuid(6);//.ToString().Replace("-", "").Substring(6, 12).ToUpper();
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string newFileName = fileName + "_" + caseMessageBoardId + "_" + uniqueName + fileExtension;
                string filePathAndName = ReadConfiguration.AccountDocumntPath + newFileName;
                string fileSavingPath = Server.MapPath(@"~" + filePathAndName);
                CommonFunctions.UploadFile(file, newFileName, "", Constants.ACCOUNT_DOCS_BLOB);
                string filepath = ReadConfiguration.DownLoadPath + Constants.ACCOUNT_DOCS_BLOB + "/" + newFileName;
                response.Status = Enums.ResponseResult.Success.ToString();
                // response.Status = Enums.ResponseResult.Success.ToString();
                // response.Message = "Document saved successfully.";
                // return Json(new { response = response, AttachedBy = userName, FileName = fileName, FilePath = filePathAndName, DocId = fileAttachmentModel.DocumentId });
                return Json(new { Status = Enums.ResponseResult.Success.ToString(), DocumentPath = filepath, DocumentName = fileName, FilePathAndName = filePathAndName });
            }
            return Json(new { Status = Enums.ResponseResult.Failure });
        }



        private List<FileAttachmentModel> PrepareFileAttachmentList(ICollection<FileAttachmentVM> fileAttachmentVM)
        {
            List<FileAttachmentModel> fileAttachmentModelList = new List<FileAttachmentModel>();
            var userId = SessionManagement.LoggedInUser.UserId;
            if (fileAttachmentVM != null && fileAttachmentVM.Count > 0)
                foreach (FileAttachmentVM fileVM in fileAttachmentVM)
                {
                    FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
                    fileAttachmentModel.DocumentName = fileVM.DocumentName;
                    fileAttachmentModel.DocumentPath = fileVM.DocumentPath;
                    fileAttachmentModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                    fileAttachmentModel.AttachedBy = userId;
                    fileAttachmentModel.UserId = userId;
                    fileAttachmentModel.CreatedDate = DateTime.UtcNow;
                    fileAttachmentModelList.Add(fileAttachmentModel);
                }
            return fileAttachmentModelList;
        }
        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameterAttribute]
        public ActionResult ContactView(string id_encrypted)
        {
            ContactModel contactModel = contactBusiness.GetContactByContactId(Convert.ToInt32(id_encrypted));
            ContactVM contactVM = new ContactVM();

            Mapper.Map(contactModel, contactVM);
            ViewBag.ContactID = contactVM.ContactId;
            return View(contactVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW)]
        public ActionResult ContactList(ContactInfo contactInfo)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int userID = (int)SessionManagement.LoggedInUser.UserId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            List<ContactModel> contactListModel;
            //if (contactInfo.IsSearchByTag == false)
            //{
            contactListModel = contactBusiness.GetAllContacts(companyId, contactInfo.CurrentPageNo, pageSize, userID, "", 0, contactInfo.FilterBy, ref totalRecords);
            //}
            //else
            //{
            //    contactListModel = contactBusiness.GetAllContactsByTagSearch(companyId, contactInfo.CurrentPageNo, pageSize, userID, contactInfo.SearchTags, ref totalRecords);
            //}
            List<ContactVM> listContactVM = new List<ContactVM>();
            Mapper.Map(contactListModel, listContactVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                ListContacts = listContactVM
            }
       , JsonRequestBehavior.AllowGet);
        }




        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW)]
        public ActionResult NonAssociatedContactList(ContactInfo contactInfo)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int userID = (int)SessionManagement.LoggedInUser.UserId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            List<ContactModel> contactListModel;
            if (!String.IsNullOrEmpty(contactInfo.AccountId))
            {
                contactListModel = contactBusiness.NonAssociatedContactList(companyId, contactInfo.CurrentPageNo, pageSize, userID, contactInfo.FilterBy, contactInfo.AccountId.Decrypt(), ref totalRecords);
            }
            else
            {
                contactListModel = contactBusiness.NonAssociatedContactList(companyId, contactInfo.CurrentPageNo, pageSize, userID, contactInfo.FilterBy, contactInfo.LeadId.Decrypt(), ref totalRecords);
            }
            List<ContactVM> listContactVM = new List<ContactVM>();
            Mapper.Map(contactListModel, listContactVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                ListContacts = listContactVM
            }
       , JsonRequestBehavior.AllowGet);
        }




        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW)]
        public ActionResult Contacts()
        {
            DateTime dateFrom = DateTime.Today.AddDays(2);
            ViewBag.PageSize = ErucaCRM.Utility.ReadConfiguration.PageSize;

            //ViewBag.CurrentContactOwner = (int)SessionManagement.LoggedInUser.UserId;
            return View(new ContactVM());
        }

        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_CREATE)]

        public ActionResult AddContact(string id_encrypted, string mode = "")
        {
            ContactVM viewmodel = new ContactVM();
            if (!string.IsNullOrEmpty(id_encrypted) && mode == Constants.MODULE_LEAD)
            {
                viewmodel.LeadId = id_encrypted;
                ViewBag.RedirectAction = Constants.MODULE_LEAD;
            }
            if (!string.IsNullOrEmpty(id_encrypted) && mode == Constants.MODULE_ACCOUNT)
            {
                ViewBag.RedirectAction = Constants.MODULE_ACCOUNT;
                viewmodel.AccountId = id_encrypted;
            }

            ViewBag.TitleList = GetTitleList();
            ViewBag.CountryList = GetCountries();
            ViewBag.ContactOwnerList = GetContactOwners((int)SessionManagement.LoggedInUser.CompanyId);
            return View(viewmodel);
        }

        public ActionResult GetMostUsedContactTags()
        {
            List<TagVM> listTags = new List<TagVM>();
            List<TagModel> listTagModels = new List<TagModel>();
            listTagModels = contactBusiness.GetMostUsedContactTags((int)SessionManagement.LoggedInUser.CompanyId);
            AutoMapper.Mapper.Map(listTagModels, listTags);

            return Json(new
           {

               listTags = listTags
           }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_CREATE)]
        public ActionResult AddContact(ContactVM contactVM)
        {
            if (ModelState.IsValid)
            {
                ContactModel contactModel = new ContactModel();
                Mapper.Map(contactVM, contactModel);
                //  contactModel.OwnerId = (int)SessionManagement.LoggedInUser.UserId;
                contactModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                contactModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                contactModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                contactModel.DOB = DateTime.UtcNow;

                contactBusiness.AddContact(contactModel);

                if (string.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"]) == false)
                {
                    string returnUrl = string.Empty;
                    returnUrl = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"].Replace('@', '#');
                    return Redirect("Accounts?tabid=2");
                }
                else { return Redirect("Accounts?tabid=2"); }

            }
            ViewBag.TitleList = GetTitleList();
            ViewBag.CountryList = GetCountries();
            ViewBag.ContactOwnerList = GetContactOwners((int)SessionManagement.LoggedInUser.CompanyId);
            return View(contactVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameterAttribute]
        public ActionResult EditContact(string id_encrypted, string accountId)
        {
            ContactModel contactModel = contactBusiness.GetContactByContactId(Convert.ToInt32(id_encrypted));
            ContactVM contactVM = new ContactVM();
            Mapper.Map(contactModel, contactVM);
            contactModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
            contactVM.ContactOwner = contactVM.User.FirstName + " " + contactVM.User.LastName;
            ViewBag.ContactOwner = contactVM.ContactOwner;
            ViewBag.TitleList = GetTitleList();
            ViewBag.CountryList = GetCountries();
            // Creating dropdown for Owners
            ViewBag.ContactOwnerList = GetContactOwners((int)SessionManagement.LoggedInUser.CompanyId);
            ViewBag.ContactId = Convert.ToInt32(id_encrypted);
            ViewBag.ContactAccountId = accountId;
            return View(contactVM);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_EDIT)]
        public ActionResult EditContact(ContactVM contactVM)
        {
            if (ModelState.IsValid)
            {
                ContactModel contactModel = new ContactModel();
                Mapper.Map(contactVM.Address, contactModel.AddressModel);
                Mapper.Map(contactVM, contactModel);
                contactModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
                contactModel.CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
                contactBusiness.UpdateContact(contactModel);
                if (string.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"]) == false)
                {
                    string returnUrl = string.Empty;
                    returnUrl = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"].Replace('@', '#');
                    return Redirect(returnUrl);
                }
                if (!string.IsNullOrEmpty(Request.Form["contactAccountId"]))
                {
                    return RedirectToAction("AccountDetail", "User", new { id_encrypted = contactVM.AccountId });
                }
                return RedirectToAction("ContactView/" + contactVM.ContactId, "User");
            }

            ViewBag.TitleList = GetTitleList();
            ViewBag.CountryList = GetCountries();
            ViewBag.ContactOwnerList = GetContactOwners((int)SessionManagement.LoggedInUser.CompanyId);
            ViewBag.ContactId = contactVM.ContactId;
            return View(contactVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult UserList()
        {
            ViewBag.PageSize = ErucaCRM.Utility.ReadConfiguration.PageSize;
            return View(new UserVM());


        }

        [HttpGet]
        public ActionResult CheckEmailAvaiable(string id)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            Boolean _response = userBusiness.IsEmailExist(id, null);

            return Json(new
            {
                _response
            }
          , JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult GetUserList(UseListInfo userListinfo)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            Boolean userStatus = userListinfo.UserStatus == "Active" ? true : false;
            long totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            List<UserModel> userListModel = userBusiness.GetAllUsersByStaus(companyId, userListinfo.CurrentPage, userStatus, pageSize, ref totalRecords);

            List<UserVM> listUserVModel = new List<UserVM>();
            Mapper.Map(userListModel, listUserVModel);

            return Json(new
            {
                TotalRecords = totalRecords,
                listUser = listUserVModel
            }
            , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_USER + Constants.PERMISSION_VIEW)]
        public ActionResult UserDetail(int id)
        {

            UserVM objUserVM = new UserVM();
            UserModel objUserModel = userBusiness.GetUserByUserId(id);
            Mapper.Map(objUserModel, objUserVM);
            if (objUserVM != null)
                objUserVM.ImageURL = objUserVM.ImageURL ?? "NoImage.jpg";
            ViewBag.EditUserLink = "Edit User";
            ViewBag.EditUserAction = "EditUser";
            return View(objUserVM);

        }

        [CustomAuthorize(Roles = Constants.MODULE_ROLE + Constants.PERMISSION_VIEW)]
        public ActionResult Roles()
        {
            ViewBag.ModuleName = "Role";
            return View();
        }

        public ActionResult RolesList()
        {
            List<RoleVM> listRoleVM = new List<RoleVM>();
            List<RoleModel> listRoleModel = new List<RoleModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listRoleModel = roleBusiness.GetRoleByCompanyId(companyId);
            Mapper.Map(listRoleModel, listRoleVM);
            return Json(listRoleVM
                , JsonRequestBehavior.AllowGet);
        }




        [CustomAuthorize(Roles = Constants.MODULE_ROLE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_ROLE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateRole(RoleVM roleVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            bool _isAdded;
            if (ModelState.IsValid)
            {
                RoleModel roleModel = new RoleModel();
                Mapper.Map(roleVM, roleModel);
                roleModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                roleModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                roleModel.CreatedDate = DateTime.UtcNow.ToDateTimeNow();
                _isAdded = roleBusiness.AddRole(roleModel);
                if (_isAdded == true)
                {
                    response.Status = Enums.ResponseResult.Success.ToString();
                }
                else
                {
                    response.Status = Enums.ResponseResult.Failure.ToString();
                    response.Message = "Role already exists";

                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_ROLE + Constants.PERMISSION_EDIT)]
        public ActionResult EditRole(RoleVM roleVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            bool _isAdded;
            if (ModelState.IsValid)
            {

                RoleModel roleModel = new RoleModel();
                Mapper.Map(roleVM, roleModel);
                roleModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                roleModel.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
                roleModel.ModifiedDate = DateTime.UtcNow;
                _isAdded = roleBusiness.UpdateRole(roleModel);
                if (_isAdded == true)
                {
                    response.Status = Enums.ResponseResult.Success.ToString();
                }
                else
                {
                    response.Status = Enums.ResponseResult.Failure.ToString();
                    response.Message = "Role already exists";

                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public ActionResult ProfileType()
        {

            return View("ProfileTypeList", new ProfileVM());

        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public ActionResult ProfileTypeList()
        {
            List<ProfileVM> listProfileVM = new List<ProfileVM>();
            List<ProfileModel> listProfileModel = new List<ProfileModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listProfileModel = profileBusiness.GetProfileByCompanyId(companyId);
            Mapper.Map(listProfileModel, listProfileVM);
            return Json(listProfileVM
               , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateProfileType()
        {
            return View(new ProfileVM());
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_CREATE)]
        public ActionResult CreateProfileType(ProfileVM profileVM)
        {
            bool _isAdded;
            ProfileModel profileModel = new ProfileModel();
            Mapper.Map(profileVM, profileModel);
            profileModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            profileModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
            profileModel.CreatedDate = DateTime.UtcNow;
            _isAdded = profileBusiness.AddProfile(profileModel);
            if (_isAdded)
            {
                return RedirectToAction("ProfileType");
            }
            else
            {
                // ViewBag.StatusMessage = "Profile Already Exists";
                ModelState.AddModelError("ProfileName", "Profile Already Exists.");
                return View(new ProfileVM());

            }
        }


        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameter]
        public ActionResult EditProfileType(string Id_encrypted)
        {
            ProfileModel profileModel = new ProfileModel();
            ProfileVM profileVM = new ProfileVM();
            profileModel = profileBusiness.GetProfileTypeById(Convert.ToInt32(Id_encrypted));
            Mapper.Map(profileModel, profileVM);
            return View(profileVM);

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public ActionResult EditProfileType(ProfileVM profileVM)
        {
            bool _isAdded;
            ProfileModel profileModel = new ProfileModel();
            Mapper.Map(profileVM, profileModel);
            profileModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
            profileModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            profileModel.ModifiedDate = DateTime.UtcNow;
            _isAdded = profileBusiness.UpdateProfile(profileModel);
            if (_isAdded == true)
            {
                return RedirectToAction("ProfileType");
            }
            else
            {
                // ViewBag.StatusMessage = "Profile Already Exists";
                ModelState.AddModelError("ProfileName", "Profile Already Exists.");
                return View(new ProfileVM());

            }

        }
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult ProfileDetail(string id_encrypted)
        {
            ProfileVM profileVm = new ProfileVM();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            ProfileModel profileModel = profileBusiness.GetProfileDetail(Convert.ToInt32(id_encrypted), null);
            profileModel.ProfilePermissionModels = profileModel.ProfilePermissionModels.Where(m => m.ModulePermission.Module.RecordDeleted == false).OrderBy(x => x.ModulePermission.Module.SortOrder.GetValueOrDefault()).ToList();
            Mapper.Map(profileModel, profileVm);
            return View(profileVm);
        }

        [HttpPost]
        public ActionResult UpdateProfilePermissionAccess(List<ProfilePermissionModel> profilePermissionModel)
        {
            Response response = new Response();
            response.StatusCode = 500;
            int modifiedBy = SessionManagement.LoggedInUser.UserId;
            profilePermissionBusiness.UpdateProfilePermission(profilePermissionModel, modifiedBy);
            response.Status = Enums.ResponseResult.Success.ToString();
            return Json(response);
        }

        public ActionResult RoleAssignment()
        {
            List<UserVM> userVMList = new List<UserVM>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = SessionManagement.LoggedInUser.UserId;
            List<UserModel> listUserModel = userBusiness.GetAllUsers(companyId, true, exceptThisId);
            Mapper.Map(listUserModel, userVMList);
            ViewBag.RolesList = GetRoleList();
            return View(userVMList);
        }

        [HttpPost]
        public JsonResult RoleAssignment(List<UserVM> userVMList)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            List<UserModel> userModelList = new List<UserModel>();
            Mapper.Map(userVMList, userModelList);
            int modifiedBy = SessionManagement.LoggedInUser.UserId;
            userBusiness.AssignRoles(userModelList, modifiedBy);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = "Roles have assigned to selected users successfully.";
            return Json(response);
        }

        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public ActionResult AssignProfile()
        {
            List<UserVM> userVMList = new List<UserVM>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = SessionManagement.LoggedInUser.UserId;
            List<UserModel> listUserModel = userBusiness.GetAllUsers(companyId, true, exceptThisId);
            Mapper.Map(listUserModel, userVMList);
            ViewBag.ProfileList = ProfileList();
            return View(userVMList);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_EDIT)]
        public JsonResult AssignProfile(List<UserVM> userVMList)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            List<UserModel> userModelList = new List<UserModel>();
            Mapper.Map(userVMList, userModelList);
            int modifiedBy = SessionManagement.LoggedInUser.UserId;
            userBusiness.AssignProfiles(userModelList, modifiedBy);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = "Profiles have assigned to selected users successfully.";
            return Json(response);
        }

        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_CREATE)]
        [HttpGet]
        public ActionResult CreateLead(string accountId_encrypted)
        {
            List<ContactVM> contactvmlist = new List<ContactVM>();
            List<ContactModel> contactmodellist = new List<ContactModel>();
            ViewBag.LeadSources = leadSourceBusiness.GetLeadSourceDropdownList();
            ViewBag.Industries = industryBusiness.GetAllIndustriesDropdownList();
            ViewBag.CountryList = GetCountries();
            ViewBag.LeadStatus = leadStatusBusiness.GetLeadStatusDropdownList();
            contactmodellist = contactBusiness.GetContactsByOwnerIdAndCompanyID(Convert.ToInt32(SessionManagement.LoggedInUser.CompanyId), SessionManagement.LoggedInUser.UserId).ToList();
            AutoMapper.Mapper.Map(contactmodellist, contactvmlist);
            ViewBag.ContactList = contactvmlist;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            if (!string.IsNullOrEmpty(accountId_encrypted))
            {
                LeadVM lead = new LeadVM();
                lead.AccountId = accountId_encrypted;
                return View(lead);
            }
            else

                return PartialView("_CreateLead", new LeadVM());
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_CREATE)]
        public ActionResult CreateLead(LeadVM leadVM)
        {
            if (ModelState.IsValid)
            {
                LeadModel leadModel = new LeadModel();
                LeadModel _leadModel = new LeadModel();
                Mapper.Map(leadVM, leadModel);
                leadModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
                leadModel.CreatedDate = DateTime.UtcNow;
                leadModel.ModifiedDate = DateTime.UtcNow;
                leadModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                if (leadModel.LeadOwnerId == null || leadModel.LeadOwnerId == 0)
                {
                    leadModel.LeadOwnerId = SessionManagement.LoggedInUser.UserId;
                }
                _leadModel = leadBusiness.AddLead(leadModel);


                AutoMapper.Mapper.Map(_leadModel, leadVM);
                leadVM.LeadOwnerName = SessionManagement.LoggedInUser.FullName;
                leadVM.LeadOwnerImage = SessionManagement.LoggedInUser.ProfileImageUrl;
                leadVM.LeadCreatedTime = DateTime.Now.ToDateTimeNow().ToLongDateString();
                //leadVM.LeadOwnerName = SessionManagement.LoggedInUser.UserName;

                //List<HomeVM> homeVMList = new List<HomeVM>();
                //List<HomeModel> homeModelList = new List<HomeModel>();
                //int maxLeadAuditId = 0;
                //int companyid = SessionManagement.LoggedInUser.CompanyId.Value;
                //int curentuserid = SessionManagement.LoggedInUser.UserId;
                //List<int?> realTimeNotificationId = realTimeNotificationBusiness.GetNotifyClientByCompanyId(companyid, curentuserid, _leadModel.LeadId, ref maxLeadAuditId);
                //homeModelList = homeBusiness.GetRecentActivitiesForHome(1, 1, maxLeadAuditId, false, 0, 0);
                //AutoMapper.Mapper.Map(homeModelList, homeVMList);
                //var context = GlobalHost.ConnectionManager.GetHubContext<RealTimeNotificationHub>();
                //foreach (int userid in realTimeNotificationId)
                //{
                //    context.Clients.User(userid.ToString()).NewNotification(new { RecentActivities = homeVMList.Select(x => new { CreatedBy = x.CreatedBy, ImageURL = x.ImageURL, LeadAuditId = x.LeadAuditId, ActivityText = x.ActivityText, ActivityCreatedTime = x.ActivityCreatedTime }), MaxLeadAuditID = maxLeadAuditId });

                //}
                RealTimeNotification(_leadModel.LeadId);
                return Json(new
                {

                    Status = "Success",
                    NewLead = leadVM
                }
            , JsonRequestBehavior.AllowGet);


                //return Json(new { AccountId = leadVM.AccountId, Status = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);

            }


        }
        public ActionResult AddNewLead(string leadName, string companyName)
        {
            LeadModel leadModel = new LeadModel();
            LeadModel _leadModel = new LeadModel();
            LeadVM leadVM = new LeadVM();
            leadModel.FirstName = leadName;
            leadModel.LeadCompanyName = companyName;
            leadModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
            leadModel.LeadOwnerId = SessionManagement.LoggedInUser.UserId;
            leadModel.CreatedDate = DateTime.UtcNow;
            leadModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            _leadModel = leadBusiness.AddLead(leadModel);
            AutoMapper.Mapper.Map(_leadModel, leadVM);
            return Json(new
            {
                Response = "Success",
                NewLead = leadVM
            }
                 , JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]

        public ActionResult ViewLeadDetail(string Id_encrypted)
        {
            int leadId = Convert.ToInt32(Id_encrypted);
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<StageModel> stageModelList = new List<StageModel>();
            List<StageVM> stageVMList = new List<StageVM>();
            LeadVM leadVM = new LeadVM();
            List<QuoteModel> quoteList = new List<QuoteModel>();
            List<SalesOrderModel> salesOrderList = new List<SalesOrderModel>();
            List<InvoiceModel> invoiceList = new List<InvoiceModel>();
            LeadModel leadModel = new LeadModel();
            leadModel = leadBusiness.GetLeadDetail(leadId);
            ViewBag.LeadStageId = leadModel.StageId;
            List<LeadContactModel> leadContactList = new List<LeadContactModel>();
            List<LeadContactVM> leadContactVMList = new List<LeadContactVM>();
            leadVM.AllProducts = new List<ProductVM>();
            IList<ProductModel> allProducts = productBusiness.GetNonAssociatedProducts(companyId, leadId);
            Mapper.Map(allProducts, leadVM.AllProducts);
            leadModel.QuoteModels = quoteBusiness.GetQuoteList(companyId, leadId);
            leadModel.RatingList = ratingBusiness.GetRatings();
            leadModel.SalesOrderModels = salesOrderBusiness.GetSalesOrderList(companyId, leadId);
            leadModel.InvoiceModels = invoiceBusiness.GetInvoiceList(companyId, leadId);
            //   leadContactList = contactBusiness.GetContactByLeadId(companyId, leadId);
            Mapper.Map(leadModel, leadVM);
            Mapper.Map(leadContactList, leadContactVMList);

            leadVM.LeadContact = leadContactVMList;
            stageModelList = stageBusiness.GetStages(companyId);
            ViewBag.LeadID = leadVM.LeadId;
            Mapper.Map(stageModelList, stageVMList);
            ViewBag.Stages = stageVMList;
            //return Json(new { leadModel = leadVM }, JsonRequestBehavior.AllowGet);
            return PartialView(leadVM);
        }

        public ActionResult LeadDetail()
        {
            return View("Leads");

        }

        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        public ActionResult Leads()
        {
            List<StageModel> stageModelList = new List<StageModel>();
            int companyId;
            companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<StageVM> stageVMlist = new List<StageVM>();
            List<StageModel> stageListModellist = stageBusiness.GetStages(companyId);
            AutoMapper.Mapper.Map(stageListModellist, stageVMlist);
            ViewBag.stageModelList = stageVMlist;
            ViewBag.totalwidth = (stageModelList.Count() * ErucaCRM.Utility.ReadConfiguration.StageWidth);
            return View(new LeadVM());
        }


        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public JsonResult GetLeadsByStageId(string currentPage, string StageId_encrypted, bool IsLoadMore, string LastLeadId_encrypted, string tagName, string SearchLeadName = "")
        {
            int totalRecords = 0;
            int totalpages = 0;
            // string tagName = tagId_encrypted == null || tagId_encrypted == "" ? "" : tagId_encrypted;
            int pagesize = ErucaCRM.Utility.ReadConfiguration.PageSize;
            List<LeadModel> leadModelList = leadBusiness.GetLeadsbyStageIdWeb(SessionManagement.LoggedInUser.UserId, SessionManagement.LoggedInUser.CompanyId.Value, Convert.ToInt32(StageId_encrypted), Convert.ToInt32(currentPage), tagName, SearchLeadName, pagesize, IsLoadMore, Convert.ToInt32(LastLeadId_encrypted), ref totalRecords);

            List<LeadVM> leadVMlist = new List<LeadVM>();
            Mapper.Map(leadModelList, leadVMlist);
            Response<LeadVM> response = new Response<LeadVM>();
            response.TotalRecords = totalRecords;
            response.List = leadVMlist;
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        //[CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        //[EncryptedActionParameter]
        //public JsonResult GetLeadTaggedData(string currentPage, string StageId_encrypted, bool IsLoadMore, string LastLeadId_encrypted, string TagId_encrypted, string SearchLeadName)
        //{
        //    int totalRecords = 0;
        //    int totalpages = 0;
        //    int pagesize = ErucaCRM.Utility.ReadConfiguration.PageSize;
        //    List<LeadModel> leadModelList = leadBusiness.GetLeadsbyStageIdWeb(SessionManagement.LoggedInUser.UserId, SessionManagement.LoggedInUser.CompanyId.Value, Convert.ToInt32(StageId_encrypted), Convert.ToInt32(currentPage), Convert.ToInt32(TagId_encrypted), SearchLeadName, pagesize, IsLoadMore, Convert.ToInt32(LastLeadId_encrypted), ref totalRecords);
        //    totalpages = totalRecords / pagesize;
        //    if (totalRecords % pagesize != 0)
        //        totalpages = totalpages + 1;
        //    List<LeadVM> leadVMlist = new List<LeadVM>();
        //    Mapper.Map(leadModelList, leadVMlist);
        //    Response<LeadVM> response = new Response<LeadVM>();
        //    response.TotalPages = totalpages;
        //    response.List = leadVMlist;
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}





        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public JsonResult GetLeadsByStageGroup(ListingParameters listingParameters, string tagId_encrypted)
        {
            int totalRecords = 0;
            List<LeadStagesJSONModel> leadListModel = leadBusiness.GetLeadsByStageGroup(
                SessionManagement.LoggedInUser.UserId,
                SessionManagement.LoggedInUser.CompanyId.Value, listingParameters.CurrentPage,
                ErucaCRM.Utility.ReadConfiguration.PageSize,
                ref totalRecords);
            List<LeadStagesJSONVm> leadListVM = new List<LeadStagesJSONVm>();

            Mapper.Map(leadListModel, leadListVM);

            Response<LeadStagesJSONVm> response = new Response<LeadStagesJSONVm>();
            response.TotalRecords = totalRecords;
            response.List = leadListVM;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult RenderLeadsInStage(int CurrentPage, string stageId_encrypted)
        {
            string leadname = "";
            int totalRecord = 0;
            List<LeadVM> leadVMlist = new List<LeadVM>();
            List<LeadModel> leadListModel = leadBusiness.GetLeadsWeb(SessionManagement.LoggedInUser.UserId, SessionManagement.LoggedInUser.CompanyId.Value, Convert.ToInt32(stageId_encrypted), "", leadname, CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref  totalRecord);
            AutoMapper.Mapper.Map(leadListModel, leadVMlist);
            return Json(new { Status = "Success", list = leadVMlist, TotalRecord = totalRecord });
        }


        public ActionResult RenderTagLeadsInStage(int CurrentPage, string stageId_encrypted, string SearchLeadName, string tagName)
        {
            string leadname = "";
            int totalRecord = 0;
            List<LeadVM> leadVMlist = new List<LeadVM>();
            List<LeadModel> leadListModel = leadBusiness.GetLeadsWeb(SessionManagement.LoggedInUser.UserId, SessionManagement.LoggedInUser.CompanyId.Value, Convert.ToInt32(stageId_encrypted), tagName, SearchLeadName, CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref  totalRecord);
            AutoMapper.Mapper.Map(leadListModel, leadVMlist);
            return Json(new { Status = "Success", list = leadVMlist });
        }



        [HttpPost]
        public JsonResult UploadAccountDocument(FileAttachmentVM fileAttachmentVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();

            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

            if (Request.Files.Count == 0)
            {
                response.Message = "No document found to save.";
                return Json(response);
            }
            var file = Request.Files[0];
            var userId = SessionManagement.LoggedInUser.UserId;
            var userName = SessionManagement.LoggedInUser.FullName;
            var accountId = fileAttachmentVM.AccountId;

            try
            {
                string uniqueName = Guid.NewGuid().ToShortGuid(6);//.ToString().Replace("-", "").Substring(6, 12).ToUpper();
                var fileExtension = Path.GetExtension(file.FileName);
                //var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName) + fileExtension;
                string newFileName = fileName + "_" + accountId + "_" + uniqueName + fileExtension;
                string filePathAndName = ReadConfiguration.AccountDocumntPath + newFileName;
                string fileSavingPath = Server.MapPath(@"~" + filePathAndName);
                CommonFunctions.UploadFile(file, newFileName, "", Constants.ACCOUNT_DOCS_BLOB);
                string filepath = ReadConfiguration.DownLoadPath + Constants.ACCOUNT_DOCS_BLOB + "/" + newFileName;
                fileAttachmentVM.DocumentName = fileName;
                fileAttachmentVM.DocumentPath = filepath;
                fileAttachmentVM.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                fileAttachmentVM.AttachedBy = userId;
                fileAttachmentVM.UserId = userId;
                fileAttachmentVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentModel);

                fileAttachmentModel = fileAttachmentBusiness.AddDocument(fileAttachmentModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.Message = "Document saved successfully.";
                return Json(new { response = response, AttachedBy = userName, AttachById = userId.Encrypt(), FileName = fileName, FilePath = filepath, DocId = fileAttachmentModel.DocumentId });
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
            }
            return Json(response);
        }
        [HttpPost]
        public JsonResult UploadAccountCaseDocument(FileAttachmentVM fileAttachmentVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();

            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

            if (Request.Files.Count == 0)
            {
                response.Message = "No document found to save.";
                return Json(response);
            }
            var file = Request.Files[0];
            var userName = SessionManagement.LoggedInUser.FullName;
            var userId = SessionManagement.LoggedInUser.UserId.Encrypt();
            var accountCaseId = fileAttachmentVM.AccountCaseId.Decrypt();
            try
            {
                string constant_doc_path = ReadConfiguration.AccountDocumntPath;
                fileAttachmentVM = GetFileAttachmentObjectAndUploadFile(fileAttachmentVM, accountCaseId, constant_doc_path);
                AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentModel);
                fileAttachmentModel = fileAttachmentBusiness.AddDocument(fileAttachmentModel);
                string filePathAndName = constant_doc_path + fileAttachmentModel.DocumentPath;
                string uniqueName = Guid.NewGuid().ToShortGuid(6);
                var fileExtension = Path.GetExtension(file.FileName);
                string fileName = file.FileName;
                string filepath = ReadConfiguration.DownLoadPath + Constants.ACCOUNT_DOCS_BLOB + "/" + fileAttachmentVM.DocumentPath;
                string newFileName = fileName + "_" + accountCaseId + "_" + uniqueName + fileExtension;
                CommonFunctions.UploadFile(file, newFileName, "", Constants.ACCOUNT_DOCS_BLOB);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.Message = "Document saved successfully.";
                return Json(new { response = response, AttachedBy = userName, UserId = userId, DocumentName = fileName, FilePath = filepath, DocId = fileAttachmentModel.DocumentId });
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
            }
            return Json(response);
        }
        private FileAttachmentVM GetFileAttachmentObjectAndUploadFile(FileAttachmentVM fileAttachmentVM, int objectId, string CONSTANT_DOCS_PATH)
        {
            var userId = SessionManagement.LoggedInUser.UserId;
            var file = Request.Files[0];
            string uniqueName = Guid.NewGuid().ToShortGuid(6);//.ToString().Replace("-", "").Substring(6, 12).ToUpper();
            var fileExtension = Path.GetExtension(file.FileName);
            // var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var fileName = file.FileName;
            string newFileName = fileName + "_" + objectId + "_" + uniqueName + fileExtension;
            string filePathAndName = CONSTANT_DOCS_PATH + newFileName;
            string fileSavingPath = Server.MapPath(@"~" + filePathAndName);
            //CommonFunctions.UploadFile(file, newFileName,"",Constants.ACCOUNT_DOCS_BLOB);
            fileAttachmentVM.DocumentName = fileName;
            fileAttachmentVM.DocumentPath = newFileName;
            fileAttachmentVM.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            fileAttachmentVM.AttachedBy = userId;
            fileAttachmentVM.UserId = userId;
            fileAttachmentVM.CreatedDate = DateTime.UtcNow;
            return fileAttachmentVM;
        }
        [HttpPost]
        public JsonResult UploadLeadDocument(FileAttachmentVM fileAttachmentVM)
        {

            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();

            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

            if (Request.Files.Count == 0)
            {
                response.Message = "No document found to save.";
                return Json(response);
            }
            var file = Request.Files[0];
            var userId = SessionManagement.LoggedInUser.UserId;
            var userName = SessionManagement.LoggedInUser.FullName;
            var leadId = fileAttachmentVM.LeadId;

            try
            {
                string uniqueName = Guid.NewGuid().ToShortGuid(6);//.ToString().Replace("-", "").Substring(6, 12).ToUpper();
                var fileExtension = Path.GetExtension(file.FileName);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string newFileName = fileName + "_" + leadId + "_" + uniqueName + fileExtension;
                string filePathAndName = ReadConfiguration.LeadDocumentPath + newFileName;
                string fileSavingPath = Server.MapPath(@"~" + filePathAndName);
                string filepath = ReadConfiguration.DownLoadPath + Constants.LEAD_DOCS_BLOB +"/"+ newFileName;
                CommonFunctions.UploadFile(file, newFileName, "", Constants.LEAD_DOCS_BLOB);
                fileAttachmentVM.DocumentName = file.FileName;
                fileAttachmentVM.DocumentPath = filepath;
                fileAttachmentVM.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                fileAttachmentVM.AttachedBy = userId;
                fileAttachmentVM.UserId = userId;
                fileAttachmentVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentModel);

                fileAttachmentModel = fileAttachmentBusiness.AddDocument(fileAttachmentModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.Message = "Document saved successfully.";
               
                return Json(new { response = response, AttachedBy = userName, FileName = file.FileName, FilePath = filepath, DocId = fileAttachmentModel.DocumentId });
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
            }
            return Json(response);
        }
        [HttpPost]
        public JsonResult RemoveAccountDocument(FileAttachmentModel fileAttachmentModel)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
            CommonFunctions.RemoveBlobDocument(fileAttachmentModel.DocumentPath, "", Constants.ACCOUNT_DOCS_BLOB);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = "Document removed successfully.";


            return Json(response);
        }
        [HttpPost]
        public JsonResult RemoveAccountCaseDocument(FileAttachmentModel fileAttachmentModel)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
            CommonFunctions.RemoveBlobDocument(fileAttachmentModel.DocumentPath, "", Constants.ACCOUNT_DOCS_BLOB);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = "Document removed successfully.";
            return Json(response);
        }

        [HttpPost]
        public JsonResult RemoveLeadDocument(FileAttachmentModel fileAttachmentModel)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);           
            CommonFunctions.RemoveBlobDocument(fileAttachmentModel.DocumentPath,"",Constants.LEAD_DOCS_BLOB);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = "Document removed successfully.";
            return Json(response);
        }


        [HttpPost]
        public JsonResult UploadActivityDocument(FileAttachmentVM fileAttachmentVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();

            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

            if (Request.Files.Count == 0)
            {
                response.Message = "No document found to save.";
                return Json(response);
            }
            var file = Request.Files[0];
            var userId = SessionManagement.LoggedInUser.UserId;
            var userName = SessionManagement.LoggedInUser.FullName;
            var taskId = fileAttachmentVM.TaskId;

            try
            {
                string uniqueName = Guid.NewGuid().ToShortGuid(6);//.ToString().Replace("-", "").Substring(6, 12).ToUpper();
                var fileExtension = Path.GetExtension(file.FileName);
                // var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName) + fileExtension;
                string newFileName = fileName + "_" + taskId + "_" + uniqueName + fileExtension;
                string filePathAndName = ReadConfiguration.ActivityDocumntPath + newFileName;
                string fileSavingPath = Server.MapPath(@"~" + filePathAndName);
                CommonFunctions.UploadFile(file, newFileName, "", Constants.ACTIVITY_DOCS_BLOB);
                string filepath = ReadConfiguration.DownLoadPath + Constants.ACTIVITY_DOCS_BLOB + "/" + newFileName;
                fileAttachmentVM.DocumentName = fileName;
                fileAttachmentVM.DocumentPath = filepath;
                fileAttachmentVM.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                fileAttachmentVM.AttachedBy = userId;
                fileAttachmentVM.UserId = userId;
                fileAttachmentVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentModel);
                fileAttachmentModel = fileAttachmentBusiness.AddDocument(fileAttachmentModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.Message = "Document saved successfully.";
                return Json(new { response = response, AttachedBy = userName, FileName = fileName, FilePath =filepath, DocId = fileAttachmentModel.DocumentId });
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult RemoveActivityDocument(FileAttachmentModel fileAttachmentModel)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
            CommonFunctions.RemoveBlobDocument(fileAttachmentModel.DocumentPath, "", Constants.ACTIVITY_DOCS_BLOB);
            response.Status = Enums.ResponseResult.Success.ToString();
            response.Message = "Document removed successfully.";
            return Json(response);

        }
        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_CREATE)]
        [EncryptedActionParameter]
        public ActionResult CreateQuote(string id_encrypted)
        {
            List<LeadModel> leadModelList = new List<LeadModel>();
            List<LeadVM> leadVmList = new List<LeadVM>();

            if (!String.IsNullOrEmpty(id_encrypted)) { ViewBag.LeadId = Convert.ToInt32(id_encrypted).Encrypt(); }
            List<Enums.Carrier> carrierList = Enum.GetValues(typeof(Enums.Carrier)).Cast<Enums.Carrier>().ToList();
            ViewBag.Carrier = carrierList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            // ViewBag.CountryList = GetCountries();
            //    ViewBag.OtherCountryList = GetOtherCountries();

            List<ProductModel> productModels = productBusiness.GetProductDropDownList(companyId);
            productModels.Insert(0, new ProductModel
            {
                ProductId = 0,
                ProductName = CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION)
            });
            ViewBag.ProductList = productModels;

            leadModelList = leadBusiness.GetLeadDropDownList(companyId);
            AutoMapper.Mapper.Map(leadModelList, leadVmList);
            ViewBag.LeadList = leadVmList;
            return View(new QuoteVM());
        }


        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_CREATE)]
        public JsonResult CreateQuote(QuoteVM quoteVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            if (ModelState.IsValid)
            {
                QuoteModel quoteModel = new QuoteModel();
                int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
                quoteVM.CompanyId = companyId;
                quoteVM.CreatedBy = SessionManagement.LoggedInUser.UserId;
                quoteVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(quoteVM, quoteModel);
                quoteBusiness.AddQuote(quoteModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                return Json(response);
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);
        }

        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameter]
        public ActionResult EditQuote(string id_encrypted, string leadId = null)
        {
            List<LeadModel> leadModelList = new List<LeadModel>();
            List<LeadVM> leadVmList = new List<LeadVM>();
            QuoteVM quoteVM = new QuoteVM();
            QuoteModel quoteModel = quoteBusiness.GetQuoteDetail(Convert.ToInt32(id_encrypted));
            AutoMapper.Mapper.Map(quoteModel, quoteVM);



            // if (quoteModel != null && quoteModel.LeadId != null) { ViewBag.LeadId = quoteModel.LeadId; }
            List<Enums.Carrier> carrierList = Enum.GetValues(typeof(Enums.Carrier)).Cast<Enums.Carrier>().ToList();
            ViewBag.Carrier = carrierList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            //ViewBag.CountryList = GetCountries();
            // ViewBag.OtherCountryList = GetOtherCountries();
            ViewBag.ProductList = productBusiness.GetProductDropDownList(companyId);
            leadModelList = leadBusiness.GetLeadDropDownList(companyId);
            AutoMapper.Mapper.Map(leadModelList, leadVmList);
            ViewBag.LeadList = leadVmList;

            if (leadId != null) { ViewBag.LeadId = leadId; }

            return View(quoteVM);
            //return View();
        }

        public JsonResult ProductDetail(int Id)
        {
            ProductModel productModel = new ProductModel();
            productModel = productBusiness.GetProductDetail(Id);
            return (Json(productModel));

        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_DELETE)]
        public JsonResult DeleteLead(LeadVM leadVM)
        {
            LeadModel leadModel = new LeadModel();
            AutoMapper.Mapper.Map(leadVM, leadModel);
            leadModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
            leadModel.ModifiedDate = DateTime.UtcNow;
            leadBusiness.DeleteLead(leadModel);
            RealTimeNotification(leadModel.LeadId);
            return Json(new Response
            {
                Message = "Lead deleted successfully",
                StatusCode = 200
                ,
                Status = Enums.ResponseResult.Success.ToString()
            });

        }

        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameterAttribute]
        public ActionResult EditLeadDetail(string Id_encrypted)
        {
            List<ContactVM> contactvmlist = new List<ContactVM>();
            List<ContactModel> contactmodellist = new List<ContactModel>();
            int leadId = Convert.ToInt32(Id_encrypted);
            LeadVM leadVM = new LeadVM();
            //LeadModel leadModel = new LeadModel();
            //leadModel = leadBusiness.GetLeadDetail(leadId);
            //leadModel.RatingList = ratingBusiness.GetRatings();
            //Mapper.Map(leadModel, leadVM);
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(SessionManagement.LoggedInUser.CompanyId.Value, true);
            ViewBag.LeadSources = leadSourceBusiness.GetLeadSourceDropdownList();
            ViewBag.Industries = industryBusiness.GetAllIndustriesDropdownList();
            //ViewBag.CountryList = GetCountries();
            ViewBag.LeadStatus = leadStatusBusiness.GetLeadStatusDropdownList();
            //contactmodellist = contactBusiness.GetContactsByOwnerIdAndCompanyID(Convert.ToInt32(SessionManagement.LoggedInUser.CompanyId), SessionManagement.LoggedInUser.UserId).ToList();
            //AutoMapper.Mapper.Map(contactmodellist, contactvmlist);
            // ViewBag.ContactList = contactvmlist;
            ViewBag.LeadId = leadVM.LeadId;
            return PartialView("_CreateLead", leadVM);
        }
        //[CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameterAttribute]
        public ActionResult GetLeadDetail(string Id_encrypted)
        {
            int leadId = Convert.ToInt32(Id_encrypted);
            LeadVM leadVM = new LeadVM();
            LeadModel leadModel = new LeadModel();
            leadModel = leadBusiness.GetLeadDetail(leadId);
            leadModel.RatingList = ratingBusiness.GetRatings();
            Mapper.Map(leadModel, leadVM);
            return Json(new { lead = leadVM }, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameterAttribute]
        public ActionResult _ViewLeadDetail(string Id_encrypted)
        {
            List<ContactVM> contactvmlist = new List<ContactVM>();
            List<ContactModel> contactmodellist = new List<ContactModel>();
            int leadId = Convert.ToInt32(Id_encrypted);
            LeadVM leadVM = new LeadVM();
            LeadModel leadModel = new LeadModel();
            leadModel = leadBusiness.GetLeadDetail(leadId);
            leadModel.RatingList = ratingBusiness.GetRatings();
            Mapper.Map(leadModel, leadVM);
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(SessionManagement.LoggedInUser.CompanyId.Value, true);
            ViewBag.LeadSources = leadSourceBusiness.GetLeadSourceDropdownList();
            ViewBag.Industries = industryBusiness.GetAllIndustriesDropdownList();
            ViewBag.LeadStatus = leadStatusBusiness.GetLeadStatusDropdownList();
            ViewBag.LeadId = leadVM.LeadId;
            return PartialView("_ViewLeadDetail", leadVM);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_EDIT)]
        public ActionResult EditLead(LeadVM leadVM)
        {
            if (ModelState.IsValid)
            {
                LeadModel leadModel = new LeadModel();
                Mapper.Map(leadVM, leadModel);

                leadModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
                leadModel.ModifiedDate = DateTime.UtcNow;
                leadModel.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                leadBusiness.UpdateLead(leadModel);
                Mapper.Map(leadModel, leadVM);
                if (leadModel.LeadOwnerId != SessionManagement.LoggedInUser.UserId)
                {
                    string emailSubject = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "LeadAssociationEmailSubject");
                    string emailBody = CommonFunctions.GetGlobalizedLabel("EmailTemplates", "LeadAssociationEmailBody");
                    bool result = userBusiness.SendLeadAssociationMail(leadModel, emailSubject, emailBody);

                }

                RealTimeNotification(leadModel.LeadId);
                return Json(new { Message = string.Empty, Status = Enums.ResponseResult.Success.ToString(), StatusCode = 200, NewLead = leadVM }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new Response { Message = string.Empty, Status = Enums.ResponseResult.Failure.ToString(), StatusCode = 200 }, JsonRequestBehavior.AllowGet);
            //{
            //    ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(SessionManagement.LoggedInUser.CompanyId.Value, true);
            //    ViewBag.LeadSources = leadSourceBusiness.GetLeadSourceDropdownList();
            //    ViewBag.Industries = industryBusiness.GetAllIndustriesDropdownList();
            //    ViewBag.CountryList = GetCountries();
            //    ViewBag.LeadStatus = leadStatusBusiness.GetLeadStatusDropdownList();
            //    ViewBag.ContactList = contactBusiness.GetContactsByOwnerIdAndCompanyID(Convert.ToInt32(SessionManagement.LoggedInUser.CompanyId), SessionManagement.LoggedInUser.UserId).ToList();

        }





        [HttpPost]
        public JsonResult AddProductToLead(ProductLeadAssociationVM productLeadAssociationVM)
        {
            ProductLeadAssociationModel productLeadAssociationModel = new ProductLeadAssociationModel();
            Mapper.Map(productLeadAssociationVM, productLeadAssociationModel);
            productLeadAssociationModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
            productLeadAssociationBusiness.AddProductToLead(productLeadAssociationModel);
            return Json(new Response { Message = string.Empty, Status = Enums.ResponseResult.Success.ToString(), StatusCode = 200 });
        }

        [HttpPost]
        public JsonResult RemoveProductFromLead(ProductLeadAssociationVM productLeadAssociationVM)
        {
            productLeadAssociationBusiness.RemoveProductToLead(productLeadAssociationVM.LeadId.Decrypt(), productLeadAssociationVM.ProductId);
            return Json(new Response { Message = string.Empty, Status = Enums.ResponseResult.Success.ToString(), StatusCode = 200 });
        }
        [HttpPost]
        public JsonResult UploadContactDocument(FileAttachmentVM fileAttachmentVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();

            if (Request.Files.Count == 0)
            {
                response.Message = "No document found to save.";
                return Json(response);
            }
            var file = Request.Files[0];
            int userId = (int)SessionManagement.LoggedInUser.UserId;
            string userName = SessionManagement.LoggedInUser.FullName;
            string contactId = fileAttachmentVM.ContactId ?? "";
            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

            try
            {
                string uniqueName = Guid.NewGuid().ToShortGuid(6);//.ToString().Replace("-", "").Substring(6, 12).ToUpper();
                var fileExtension = Path.GetExtension(file.FileName);
                // var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName) + fileExtension;
                string newFileName = fileName + "_" + contactId + "_" + uniqueName + fileExtension;
                string filePathAndName = ReadConfiguration.ContactDocumntPath + newFileName;
                string fileSavingPath = Server.MapPath(@"~" + filePathAndName);
                string filepath = ReadConfiguration.DownLoadPath + Constants.CONTACT_DOCS_BLOB + "/" + newFileName;
                CommonFunctions.UploadFile(file, newFileName, "", Constants.CONTACT_DOCS_BLOB);
                fileAttachmentVM.DocumentName = fileName;
                fileAttachmentVM.DocumentPath = filepath;
                fileAttachmentVM.CompanyId = SessionManagement.LoggedInUser.CompanyId;
                fileAttachmentVM.AttachedBy = userId;
                fileAttachmentVM.UserId = userId;
                fileAttachmentVM.ContactId = contactId;
                fileAttachmentVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(fileAttachmentVM, fileAttachmentModel);
                fileAttachmentModel = fileAttachmentBusiness.AddDocument(fileAttachmentModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                response.Message = "Document saved successfully.";
                return Json(new { response = response, AttachedBy = userName, FileName = fileName, FilePath = filepath, DocName = newFileName, DocId = fileAttachmentModel.DocumentId });
            }
            catch (Exception ex)
            {
                response.Message = ex.ToString();
            }
            return Json(response);
        }
        [HttpPost]
        public JsonResult RemoveContactDocument(FileAttachmentModel fileAttachmentModel)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
            CommonFunctions.RemoveBlobDocument(fileAttachmentModel.DocumentPath, "", Constants.CONTACT_DOCS_BLOB);
            response.Status = Enums.ResponseResult.Success.ToString();
            //FileInfo objFile = new FileInfo(Server.MapPath(@"~" + ReadConfiguration.ContactDocumntPath + fileAttachmentModel.DocumentPath));
            //if (objFile != null)
            //{
            //    objFile.Delete();
            //}
            response.Message = "document removed successfully.";
            return Json(response);

        }


        [HttpPost]
        public JsonResult AddNewProduct(ProductVM productVM)
        {
            int _productId;
            productVM.CompanyId = SessionManagement.LoggedInUser.CompanyId;
            productVM.CreatedBy = SessionManagement.LoggedInUser.UserId;
            productVM.CreatedDate = DateTime.UtcNow;
            ProductModel productModel = new ProductModel();
            Mapper.Map(productVM, productModel);
            _productId = productBusiness.AddNewProduct(productModel);
            return Json(new { Message = string.Empty, ProductId = _productId, Status = Enums.ResponseResult.Success.ToString(), StatusCode = 200 });
        }

        #region "Task Management"
        /// <summary>
        /// Load the Default Task In Add New Task and Edit Task Mode
        /// </summary>
        /// <param name="taskId">Edit Mode</param>
        /// <param name="mod">Associated Module to Populate data for passed in querystring. i.e. mod=Lead or mod=Contact</param>
        /// <param name="val">Associated Module Value passed in querystring. i.e. val=1</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        public TaskItemVM LoadTask(int? taskId, string mod, int? val)
        {
            TaskItemVM task = new TaskItemVM();
            TaskItemModel taskModel = new TaskItemModel();
            Owner owner = null;
            task.AccountCaseNumber = null;
            // Edit Task
            if (taskId > 0)
            {
                taskModel = taskBusiness.GetTask((int)taskId);
                if (taskModel != null)
                {
                    task.TaskId = taskId.Value.Encrypt();
                    task.OwnerId = taskModel.OwnerId;
                    task.OwnerName = taskBusiness.GetTaskOwnerName(task.OwnerId);
                    task.Status = taskModel.Status;
                    task.Subject = taskModel.Subject;
                    task.DueDate = taskModel.DueDate;
                    task.PriorityId = taskModel.PriorityId;
                    task.AssociatedModuleId = taskModel.AssociatedModuleId;
                    task.AssociatedModuleValue = taskModel.AssociatedModuleValue;
                    task.Description = taskModel.Description;
                    task.CreatedBy = taskModel.CreatedBy;
                    task.CreatedDate = taskModel.CreatedDate;
                    val = taskModel.AssociatedModuleValue;
                }
            }
            // Add Task
            else
            {
                task.OwnerId = task.OwnerId = SessionManagement.LoggedInUser.UserId;
                task.OwnerName = SessionManagement.LoggedInUser.FullName;
                task.Status = 1;
                task.DueDate = DateTime.UtcNow;
                task.AssociatedModuleValue = 0;
            }

            // Creating dropdown for Owners
            IList<UserModel> ownersModelList = userBusiness.GetAllActiveUsers((int)SessionManagement.LoggedInUser.CompanyId).ToList();
            IList<Owner> ownersVMList = new List<Owner>();

            foreach (var o in ownersModelList)
            {
                owner = new Owner();
                owner.OwnerId = o.UserId;
                owner.OwnerName = o.FirstName + " " + o.LastName;
                ownersVMList.Add(owner);
            }
            task.Owners = ownersVMList;

            // Load modules dropdown based upon modulename in querystring. Also populates Associated Module value based upon val.
            if (mod == Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Lead).ToString())
            {
                task.AssociatedModuleId = (int)Utility.Enums.Module.Lead;
                if (val != null)
                    task.AssociatedModuleValue = (int)val;
            }
            else if (mod == Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Contact).ToString())
            {
                task.AssociatedModuleId = (int)Utility.Enums.Module.Contact;
                if (val != null)
                    task.AssociatedModuleValue = (int)val;
            }
            else if (mod == Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Account).ToString())
            {
                task.AssociatedModuleId = (int)Utility.Enums.Module.Account;
                if (val != null)
                    task.AssociatedModuleValue = (int)val;
            }
            else if (task.AssociatedModuleId == (int)Utility.Enums.Module.AccountCase || mod == Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.AccountCase).ToString())
            {
                if (task.AssociatedModuleId == 0)
                    task.AssociatedModuleId = (int)Utility.Enums.Module.AccountCase;

                if (val != null)
                {
                    //if (val > 0)
                    //    task.AssociatedModuleValue = (int)val;
                    mod = "AccountCase";
                    AccountCaseModel accountCaseModel = accountcaseBusiness.GetAccountCaseInfo(val ?? 0);
                    if (accountCaseModel != null)
                    {
                        task.AssociatedModuleValue = accountCaseModel.AccountCaseId;

                        task.AccountCaseNumber = accountCaseModel.CaseNumber;
                        task.AccountName = accountCaseModel.AccountName;



                    }

                }
            }

            if (mod != Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.AccountCase).ToString())
            {

                task.AssociatedModules = new List<ErucaCRM.Repository.Module>();
                ErucaCRM.Repository.Module module = new ErucaCRM.Repository.Module();
                module.ModuleId = (int)Utility.Enums.Module.Account;
                module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Account);
                task.AssociatedModules.Add(module);
                module = new ErucaCRM.Repository.Module();
                module.ModuleId = (int)Utility.Enums.Module.Lead;
                module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Lead);
                task.AssociatedModules.Add(module);
                module = new ErucaCRM.Repository.Module();
                module.ModuleId = (int)Utility.Enums.Module.Contact;
                module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Contact);
                task.AssociatedModules.Add(module);

            }


            ViewBag.TaskStatus = taskBusiness.GetTaskStatus().ToList();
            IList<Utility.WebClasses.Priority> priorities = new List<Utility.WebClasses.Priority>();
            Priority priority = null;
            Array values = Enum.GetValues(typeof(Utility.Enums.TaskPriority));

            foreach (Utility.Enums.TaskPriority item in values)
            {
                priority = new Priority();
                priority.PriorityId = (int)item;
                priority.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item);
                priorities.Add(priority);
            }

            ViewBag.Priorities = priorities;
            return task;
        }
        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_VIEW)]
        public TaskItemVM LoadTaskDetail(int taskId)
        {
            TaskItemVM task = new TaskItemVM();
            TaskItemModel taskModel = new TaskItemModel();



            taskModel = taskBusiness.GetTask((int)taskId);
            if (taskModel != null)
            {
                task.OwnerId = taskModel.OwnerId;
                task.OwnerName = taskBusiness.GetTaskOwnerName(task.OwnerId);
                task.Status = taskModel.Status;
                task.Subject = taskModel.Subject;
                task.DueDate = taskModel.DueDate;
                task.PriorityId = taskModel.PriorityId;
                task.AssociatedModuleId = taskModel.AssociatedModuleId;
                task.AssociatedModuleValue = taskModel.AssociatedModuleValue;
                task.TaskAssociatedPerson = taskBusiness.GetTaskAssociatedPersonName(task.AssociatedModuleId, task.AssociatedModuleValue);
                task.Description = taskModel.Description;
                task.CreatedBy = taskModel.CreatedBy;
                task.CreatedDate = taskModel.CreatedDate;
            }
            return task;
        }




        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_CREATE)]
        [EncryptedActionParameter]

        public ActionResult TaskItem(string taskID_encrypted, string mod, string val_encrypted)
        {
            int moduleValue = Convert.ToInt32(val_encrypted);
            TaskItemVM task = new TaskItemVM();
            task = LoadTask(Convert.ToInt32(taskID_encrypted), mod, moduleValue);
            ViewBag.RedirectAction = "Tasks";

            //if (mod != null && moduleValue > 0)
            //{
            //    if (mod.ToLower() == "contact")
            //    {
            //        ViewBag.RedirectAction = "/user/ContactView/" + moduleValue.Encrypt();
            //    }
            //    else if (mod.ToLower() == "account")
            //    {
            //        ViewBag.RedirectAction = "/user/AccountDetail/" + moduleValue.Encrypt();
            //    }
            //    else if (mod.ToLower() == "accountcase")
            //    {
            //        ViewBag.RedirectAction = "/user/AccountCaseDetail/" + moduleValue.Encrypt();
            //    }
            //    else if (mod.ToLower() == Constants.MODULE_LEAD.ToLower())
            //    {
            //        ViewBag.RedirectAction = "/user/Leads/#" + moduleValue.Encrypt();//# is appended so that the lead detail is opened 
            //    }
            //}
            return View(task);


        }


        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_CREATE)]
        [HttpPost]
        public ActionResult TaskItem(TaskItemVM task)
        {
            if (ModelState.IsValid)
            {
                TaskItemModel taskModel = new TaskItemModel();
                taskModel.TaskId = task.TaskId.Decrypt();
                taskModel.Subject = task.Subject;
                taskModel.Status = task.Status;
                taskModel.DueDate = task.DueDate;
                taskModel.OwnerId = task.OwnerId;
                taskModel.PriorityId = task.PriorityId;
                taskModel.AssociatedModuleId = task.AssociatedModuleId;
                taskModel.AssociatedModuleValue = task.AssociatedModuleValue;
                taskModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId; ;
                taskModel.Description = task.Description;

                if (taskModel.TaskId == 0)
                {
                    taskModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
                    taskModel.CreatedDate = DateTime.UtcNow;
                }
                else
                {
                    taskModel.CreatedBy = task.CreatedBy;
                    taskModel.CreatedDate = task.CreatedDate;
                }

                taskModel.EndDate = DateTime.UtcNow;
                taskModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
                taskModel.ModifiedDate = DateTime.UtcNow;

                taskBusiness.AddTask(taskModel);
                //  TempData["ShowMessage"] = "Task Successfully Saved!";
                string strAction = "Tasks";
                if (string.IsNullOrWhiteSpace(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"]) == false)
                {
                    string returnUrl = string.Empty;
                    string hashvalue = "#" + HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["val_encrypted"];
                        returnUrl = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnurl"].Replace('@', '#');
                        if (returnUrl.Contains(Constants.AND) || returnUrl.Contains(Constants.EQUALSTO))
                        {
                            returnUrl = returnUrl.Replace("|and|", "&").Replace("|equalsto|", "=");
                        }
                    if (!returnUrl.Contains("Leads"))
                    {
                        hashvalue = "";
                    }
                    return Redirect(returnUrl + hashvalue);
                }

                //if (task.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Account))
                //{
                //    if (task.AssociatedModuleValue > 0)
                //        strAction = "AccountDetail/" + task.AssociatedModuleValue.Encrypt();
                //    else
                //        strAction = "Accounts";
                //}
                //else if (task.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Contact))
                //{
                //    if (task.AssociatedModuleValue > 0)
                //        strAction = "ContactView/" + task.AssociatedModuleValue.Encrypt();
                //    else
                //        strAction = "Contacts";
                //}
                //else if (task.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Account))
                //{
                //    if (task.AssociatedModuleValue > 0)
                //        strAction = "AccountDetail/" + task.AssociatedModuleValue.Encrypt();
                //    else
                //        strAction = "Accounts";
                //}
                //else if (task.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.AccountCase))
                //{
                //    if (task.AssociatedModuleValue > 0)

                //        strAction = "AccountCaseDetail/" + task.AssociatedModuleValue.Encrypt();
                //    else
                //        strAction = "Accounts";
                //}
                //else if (task.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Lead))
                //{
                //    if (task.AssociatedModuleValue > 0)

                //        return Redirect(Url.Action("Leads", "User") + "#" + task.AssociatedModuleValue.Encrypt());  //# is appended for opening LeadDetail(Code is implemented in such way)

                //}

                else
                    strAction = "Tasks";
                return RedirectToAction(strAction);
            }
            else
            {
                ViewBag.RedirectAction = "Tasks";
                return View(task);
            }
        }


        /// <summary>
        /// Ajax based method to populate AssociatedModuleValue dropdown based upon Selected Module.
        /// </summary>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public JsonResult GetAssociatedModuleValues(int ModuleId)
        {
            List<AssociatedModuleResponses> response = new List<AssociatedModuleResponses>();

            if (ModuleId == Convert.ToInt32(Utility.Enums.Module.Lead))
            {
                AssociatedModuleResponses module;
                List<LeadModel> leads = leadBusiness.GetAllLeadByOwnerId(Convert.ToInt32(SessionManagement.LoggedInUser.CompanyId), SessionManagement.LoggedInUser.UserId).ToList();
                foreach (var lead in leads)
                {
                    module = new AssociatedModuleResponses();
                    module.Id = lead.LeadId;
                    module.value = lead.Title;
                    response.Add(module);
                }
            }
            else if (ModuleId == Convert.ToInt32(Utility.Enums.Module.Contact))
            {
                AssociatedModuleResponses module;
                List<ContactModel> contacts = contactBusiness.GetContactsByOwnerIdAndCompanyID(Convert.ToInt32(SessionManagement.LoggedInUser.CompanyId), SessionManagement.LoggedInUser.UserId).ToList();
                foreach (var contact in contacts)
                {
                    module = new AssociatedModuleResponses();
                    module.Id = contact.ContactId;
                    module.value = contact.FirstName + " " + contact.LastName;
                    response.Add(module);
                }
            }
            else if (ModuleId == Convert.ToInt32(Utility.Enums.Module.Account))
            {
                AssociatedModuleResponses module;
                List<AccountModel> accounts = accountBussiness.GetAccountsByOwnerIdAndCompanyID(Convert.ToInt32(SessionManagement.LoggedInUser.CompanyId), SessionManagement.LoggedInUser.UserId).ToList();
                foreach (var account in accounts)
                {
                    module = new AssociatedModuleResponses();
                    module.Id = account.AccountId;
                    module.value = account.AccountName;
                    response.Add(module);
                }
            }

            return Json(response);
        }

        //[HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_VIEW)]
        public JsonResult GetTasks(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            IList<TaskItemModel> userListModel = taskBusiness.GetTasks(SessionManagement.LoggedInUser.UserId,
                SessionManagement.LoggedInUser.CompanyId.Value,
                listingParameters.CurrentPage,
                ErucaCRM.Utility.ReadConfiguration.PageSize,
                ref totalRecords, listingParameters.sortColumnName, listingParameters.sortdir);
            List<TaskItemVM> listTaskVModels = new List<TaskItemVM>();
            Mapper.Map(userListModel, listTaskVModels);

            foreach (TaskItemVM item in listTaskVModels)
            {
                item.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item.PriorityId);
                item.PriorityName = CommonFunctions.GetGlobalizedLabel("DropDowns", item.PriorityName);

                item.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), item.Status);
                item.TaskStatus = CommonFunctions.GetGlobalizedLabel("DropDowns", item.TaskStatus);
                item.TaskDueDate = item.DueDate.ToShortDateString();
            }
            Response<TaskItemVM> response = new Response<TaskItemVM>();
            response.TotalRecords = totalRecords;
            response.List = listTaskVModels;
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_VIEW)]
        public JsonResult GetDashBoardTasks()
        {

            IList<TaskItemModel> userListModel = taskBusiness.GetDashBoardTasks(
                SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);


            List<TaskItemVM> listTaskVModels = new List<TaskItemVM>();
            Mapper.Map(userListModel, listTaskVModels);

            foreach (TaskItemVM item in listTaskVModels)
            {
                item.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item.PriorityId);
                item.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), item.Status);
                item.TaskDueDate = item.DueDate.ToDateTimeNow().ToString("d-MMM-yyyy");
            }
            Response<TaskItemVM> response = new Response<TaskItemVM>();
            //  response.TotalRecords = totalRecords;
            response.List = listTaskVModels;
            return Json(response, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_VIEW)]
        public JsonResult GetContactTasks(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            IList<TaskItemModel> userListModel = taskBusiness.GetContactTasks(
                SessionManagement.LoggedInUser.CompanyId.Value, listingParameters.ContactID.Decrypt(),
                listingParameters.CurrentPage,
                ErucaCRM.Utility.ReadConfiguration.PageSize,
                ref totalRecords);
            List<TaskItemVM> listTaskVModels = new List<TaskItemVM>();
            AutoMapper.Mapper.Map(userListModel, listTaskVModels);

            foreach (TaskItemVM item in listTaskVModels)
            {
                item.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item.PriorityId);
                item.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), item.Status);
                item.TaskDueDate = item.DueDate.ToShortDateString();
            }
            Response<TaskItemVM> response = new Response<TaskItemVM>();
            response.TotalRecords = totalRecords;
            response.List = listTaskVModels;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        public JsonResult GetAccountTasks(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            IList<TaskItemModel> userListModel = taskBusiness.GetAccountTasks(
                SessionManagement.LoggedInUser.CompanyId.Value, listingParameters.AccountId.Decrypt(),
                listingParameters.CurrentPage,
                ErucaCRM.Utility.ReadConfiguration.PageSize,
                ref totalRecords);
            List<TaskItemVM> listTaskVModels = new List<TaskItemVM>();
            AutoMapper.Mapper.Map(userListModel, listTaskVModels);

            foreach (TaskItemVM item in listTaskVModels)
            {
                item.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item.PriorityId);
                item.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), item.Status);
                item.TaskDueDate = item.DueDate.ToShortDateString();
            }
            Response<TaskItemVM> response = new Response<TaskItemVM>();
            response.TotalRecords = totalRecords;
            response.List = listTaskVModels;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_VIEW)]

        [EncryptedActionParameter]
        public JsonResult GetLeadTasks(string leadId_encrypted, int CurrentPageNo)
        {
            int totalRecords = 0;
            IList<TaskItemModel> userListModel = taskBusiness.GetLeadTasks(
                SessionManagement.LoggedInUser.CompanyId.Value, Convert.ToInt32(leadId_encrypted),
                CurrentPageNo,
                ErucaCRM.Utility.ReadConfiguration.PageSize,
                ref totalRecords);
            List<TaskItemVM> listTaskVModels = new List<TaskItemVM>();
            AutoMapper.Mapper.Map(userListModel, listTaskVModels);

            foreach (TaskItemVM item in listTaskVModels)
            {
                item.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item.PriorityId);
                item.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), item.Status);
                item.TaskDueDate = item.DueDate.ToShortDateString();
            }
            Response<TaskItemVM> response = new Response<TaskItemVM>();
            response.TotalRecords = totalRecords;
            response.List = listTaskVModels;
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Tasks()
        {
            return View(new TaskItemVM());
        }

        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult ViewTaskItemDetail(string taskID_encrypted, string mod)
        {
            TaskItemVM task = new TaskItemVM();
            TaskItemModel taskModel = new TaskItemModel();
            taskModel = taskBusiness.GetTask(Convert.ToInt32(taskID_encrypted));
            if (taskModel != null)
            {
                task.TaskId = taskModel.TaskId.ToString();
                task.TaskIdEncrypted = taskModel.TaskId.Encrypt();
                task.OwnerId = taskModel.OwnerId;
                task.OwnerName = taskBusiness.GetTaskOwnerName(task.OwnerId);
                task.Status = taskModel.Status;
                task.Subject = taskModel.Subject;
                task.DueDate = taskModel.DueDate;
                task.PriorityId = taskModel.PriorityId;
                task.AudioFileName = taskModel.AudioFileName;
                task.AssociatedModuleId = taskModel.AssociatedModuleId;
                task.AssociatedModuleValue = taskModel.AssociatedModuleValue;
                task.TaskType = CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.Module), task.AssociatedModuleId));
                task.TaskAssociatedPerson = taskBusiness.GetTaskAssociatedPersonName(task.AssociatedModuleId, task.AssociatedModuleValue);
                task.PriorityName = CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.TaskPriority), task.PriorityId));
                task.TaskStatus = CommonFunctions.GetGlobalizedLabel("DropDowns", Enum.GetName(typeof(Utility.Enums.TaskStaus), task.Status));
                task.TaskDueDate = task.DueDate.ToShortDateString();
                task.Description = taskModel.Description;
                task.CreatedBy = taskModel.CreatedBy;
                task.CreatedDate = taskModel.CreatedDate;
                AutoMapper.Mapper.Map(taskModel.FileAttachmentModels, task.FileAttachments);

            }
            return View(task);
        }

        #endregion "Task Management"


        public ActionResult GetProduct(int Id)
        {

            ProductModel productModel = productBusiness.GetProductDetail(Id);
            return Json(productModel, JsonRequestBehavior.AllowGet);

        }
        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_CREATE)]
        [EncryptedActionParameter]
        public ActionResult CreateSalesOrder(string id_encrypted)
        {
            List<QuoteVM> quoteVmList = new List<QuoteVM>();
            List<AccountVM> accountVmList = new List<AccountVM>();
            List<AccountModel> accountModelList = new List<AccountModel>();

            if (id_encrypted != null)
            {
                ViewBag.AccountId = Convert.ToInt32(id_encrypted).Encrypt();
                //ViewBag.SaleOrderId = id;
            }
            List<Enums.Carrier> carrierList = Enum.GetValues(typeof(Enums.Carrier)).Cast<Enums.Carrier>().ToList();
            ViewBag.Carrier = carrierList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            ViewBag.CountryList = GetCountries();
            ViewBag.OtherCountryList = GetOtherCountries();
            ViewBag.ProductList = productBusiness.GetProductDropDownList(companyId);
            ViewBag.QuoteList = quoteVmList;
            accountModelList = accountBussiness.GetAccountsByOwnerIdAndCompanyID(companyId, SessionManagement.LoggedInUser.UserId);
            AutoMapper.Mapper.Map(accountModelList, accountVmList);
            ViewBag.AccountList = accountVmList;

            return View(new SalesOrderVM());
        }

        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameter]
        public ActionResult EditSalesOrder(string id_encrypted, string accountId)
        {
            // ViewBag.LeadId = id;
            List<QuoteVM> quoteVmList = new List<QuoteVM>();
            List<QuoteModel> quoteModelList = new List<QuoteModel>();
            List<AccountVM> accountVmList = new List<AccountVM>();
            List<AccountModel> accountModelList = new List<AccountModel>();

            SalesOrderVM salesOrderVM = new SalesOrderVM();

            SalesOrderModel salesOrderModel = salesOrderBusiness.GetSalesOrderDetail(Convert.ToInt32(id_encrypted));
            AutoMapper.Mapper.Map(salesOrderModel, salesOrderVM);
            //  return View(salesOrderVM);

            List<Enums.Carrier> carrierList = Enum.GetValues(typeof(Enums.Carrier)).Cast<Enums.Carrier>().ToList();
            ViewBag.Carrier = carrierList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            ViewBag.Owners = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            ViewBag.CountryList = GetCountries();
            ViewBag.OtherCountryList = GetOtherCountries();
            ViewBag.ProductList = productBusiness.GetProductDropDownList(companyId);
            quoteModelList = quoteBusiness.GetQuoteDropDownList(companyId);
            AutoMapper.Mapper.Map(quoteModelList, quoteVmList);
            ViewBag.QuoteList = quoteVmList;
            accountModelList = accountBussiness.GetAccountsByOwnerIdAndCompanyID(companyId, SessionManagement.LoggedInUser.UserId);
            AutoMapper.Mapper.Map(accountModelList, accountVmList);
            ViewBag.AccountList = accountVmList;
            ViewBag.SaleOrderId = salesOrderVM.SalesOrderId;
            if (accountId != null) { ViewBag.AccountId = accountId; }

            return View(salesOrderVM);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_CREATE)]
        public JsonResult CreateSalesOrder(SalesOrderVM salesOrderVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            if (ModelState.IsValid)
            {
                SalesOrderModel salesOrderModel = new SalesOrderModel();
                int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
                salesOrderVM.CompanyId = companyId;
                salesOrderVM.CreatedBy = SessionManagement.LoggedInUser.UserId;
                salesOrderVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(salesOrderVM, salesOrderModel);
                salesOrderBusiness.AddSalesOrder(salesOrderModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                return Json(response);
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);
        }

        [HttpDelete]
        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public JsonResult DeleteSaleOrder(string id_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (salesOrderBusiness.DeleteSaleOrder(Convert.ToInt32(id_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                meassage = "Sale order deleted successfully";
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {
                meassage = "Sale order deletion failed";
                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }


        [HttpDelete]
        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public JsonResult DeleteAccountContact(string contactId_encrypted, string accountId_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (contactBusiness.DeleteAccountContact(Convert.ToInt32(contactId_encrypted), Convert.ToInt32(accountId_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                meassage = "Contact deleted successfully";
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {
                meassage = "Contact deletion failed";
                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }


        [HttpDelete]
        [CustomAuthorize(Roles = Constants.MODULE_CONTACT + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public JsonResult DeleteLeadContact(string contactId_encrypted, string leadId_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (contactBusiness.DeleteLeadContact(Convert.ToInt32(contactId_encrypted), Convert.ToInt32(leadId_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                meassage = "Contact deleted successfully";
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {
                meassage = "Contact deletion failed";
                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }




        //[HttpDelete]
        //[CustomAuthorize(Roles = Constants.MODULE_CASE + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public JsonResult DeleteAccountCase(string caseId_encrypted, string accountId_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (accountcaseBusiness.DeleteAccountCase(Convert.ToInt32(caseId_encrypted), Convert.ToInt32(accountId_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                meassage = "Account Case deleted successfully";
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {
                meassage = "Account Case deletion failed";
                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }

        [HttpDelete]
        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public JsonResult DeleteAccountActivity(string taskId_encrypted, string accountId_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (taskBusiness.DeleteAccountTaskItem(Convert.ToInt32(taskId_encrypted), Convert.ToInt32(accountId_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                meassage = "Account Activity deleted successfully";
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {
                meassage = "Account Activity deletion failed";
                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }

        [HttpDelete]
        [CustomAuthorize(Roles = Constants.MODULE_TASK + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public JsonResult DeleteLeadActivity(string taskId_encrypted, string leadId_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (taskBusiness.DeleteTaskItem(Convert.ToInt32(taskId_encrypted), Convert.ToInt32(leadId_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                meassage = "Account Activity deleted successfully";
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {
                meassage = "Account Activity deletion failed";
                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }


        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public JsonResult GetQuoteDetail(string Id_encrypted)
        {
            QuoteModel quoteModel = quoteBusiness.GetQuoteDetail(Convert.ToInt32(Id_encrypted));
            QuoteVM quoteVM = new QuoteVM();
            AutoMapper.Mapper.Map(quoteModel, quoteVM);

            return Json(quoteVM, JsonRequestBehavior.AllowGet);

        }

        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_CREATE)]
        [EncryptedActionParameter]
        public ActionResult CreateInvoice(string id_encrypted)
        {
            List<SalesOrderVM> salesOrderVMList = new List<SalesOrderVM>();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();
            List<LeadVM> leadVMList = new List<LeadVM>();
            List<LeadModel> leadModelList = new List<LeadModel>();
            if (id_encrypted != null) { ViewBag.LeadId = Convert.ToInt32(id_encrypted).Encrypt(); }
            List<Enums.Carrier> carrierList = Enum.GetValues(typeof(Enums.Carrier)).Cast<Enums.Carrier>().ToList();
            ViewBag.Carrier = carrierList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            InvoiceVM invoiceVM = new InvoiceVM();
            invoiceVM.OwnerList = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            invoiceVM.CountryList = GetCountries();
            invoiceVM.OtherCountryList = GetOtherCountries();
            invoiceVM.ProductList = productBusiness.GetProductDropDownList(companyId);
            salesOrderModelList = salesOrderBusiness.GetSalesOrderDropDownList(companyId);
            AutoMapper.Mapper.Map(salesOrderModelList, salesOrderVMList);
            invoiceVM.SalesOrderList = salesOrderVMList;
            leadModelList = leadBusiness.GetLeadDropDownList(companyId);
            AutoMapper.Mapper.Map(leadModelList, leadVMList);
            invoiceVM.LeadList = leadVMList;

            invoiceVM.StatusList = GetStatus();
            return View(invoiceVM);
        }
        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_EDIT)]
        [EncryptedActionParameter]
        public ActionResult EditInvoice(string id_encrypted, string leadId)
        {
            List<SalesOrderVM> salesOrderVMList = new List<SalesOrderVM>();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();
            List<LeadVM> leadVMList = new List<LeadVM>();
            List<LeadModel> leadModelList = new List<LeadModel>();
            List<Enums.Carrier> carrierList = Enum.GetValues(typeof(Enums.Carrier)).Cast<Enums.Carrier>().ToList();
            ViewBag.Carrier = carrierList;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int exceptThisId = 0;//SessionManagement.LoggedInUser.UserId;
            InvoiceVM invoiceVM = new InvoiceVM();

            InvoiceModel invoiceModel = invoiceBusiness.GetInvoiceDetail(Convert.ToInt32(id_encrypted));
            AutoMapper.Mapper.Map(invoiceModel, invoiceVM);


            invoiceVM.OwnerList = userBusiness.GetAllOwnerDropdownList(companyId, true, exceptThisId);
            invoiceVM.CountryList = GetCountries();
            invoiceVM.OtherCountryList = GetOtherCountries();
            invoiceVM.ProductList = productBusiness.GetProductDropDownList(companyId);
            salesOrderModelList = salesOrderBusiness.GetSalesOrderDropDownList(companyId);
            AutoMapper.Mapper.Map(salesOrderModelList, salesOrderVMList);
            invoiceVM.SalesOrderList = salesOrderVMList;
            leadModelList = leadBusiness.GetLeadDropDownList(companyId);
            AutoMapper.Mapper.Map(leadModelList, leadVMList);
            invoiceVM.LeadList = leadVMList;
            invoiceVM.StatusList = GetStatus();
            ViewBag.InvoiceId = invoiceVM.InvoiceId;
            if (leadId != null) { ViewBag.LeadId = leadId; }
            return View(invoiceVM);
        }

        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_CREATE)]
        public JsonResult CreateInvoice(InvoiceVM invoiceVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            if (ModelState.IsValid)
            {
                InvoiceModel invoiceModel = new InvoiceModel();
                int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
                invoiceVM.CompanyId = companyId;
                invoiceVM.CreatedBy = SessionManagement.LoggedInUser.UserId;
                invoiceVM.CreatedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(invoiceVM, invoiceModel);
                invoiceBusiness.AddInvoice(invoiceModel);
                response.Status = Enums.ResponseResult.Success.ToString();
                return Json(response);
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);
        }

        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public JsonResult GetSalesOrderDetail(string Id_encrypted)
        {
            SalesOrderModel salesOrderModel = salesOrderBusiness.GetSalesOrderDetail(Convert.ToInt32(Id_encrypted));
            SalesOrderVM salesOrderVM = new SalesOrderVM();
            AutoMapper.Mapper.Map(salesOrderModel, salesOrderVM);
            return Json(salesOrderVM, JsonRequestBehavior.AllowGet);

        }

        private List<StatusModel> GetStatus()
        {
            List<StatusModel> list = Enum.GetValues(typeof(Enums.Status)).Cast<Enums.Status>().Select(x => new StatusModel()
            {
                Status = EnumHelper.GetDescription(x),
                StatusId = ((int)x)
            }).ToList();
            list.Insert(0, new StatusModel { StatusId = 0, Status = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return list;
        }
        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult ViewQuoteDetail(string Id_encrypted)
        {
            QuoteVM quoteVM = new QuoteVM();
            QuoteModel quoteModel = quoteBusiness.GetQuoteDetail(Convert.ToInt32(Id_encrypted));
            AutoMapper.Mapper.Map(quoteModel, quoteVM);
            return View(quoteVM);

        }
        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult ViewSalesOrderDetail(string Id_encrypted)
        {
            SalesOrderVM salesOrderVM = new SalesOrderVM();
            SalesOrderModel salesOrderModel = salesOrderBusiness.GetSalesOrderDetail(Convert.ToInt32(Id_encrypted));
            AutoMapper.Mapper.Map(salesOrderModel, salesOrderVM);
            return View(salesOrderVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult ViewInvoiceDetail(string Id_encrypted)
        {
            InvoiceVM salesOrderVM = new InvoiceVM();
            InvoiceModel invoiceModel = invoiceBusiness.GetInvoiceDetail(Convert.ToInt32(Id_encrypted));
            AutoMapper.Mapper.Map(invoiceModel, salesOrderVM);
            return View(salesOrderVM);
        }

        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_VIEW)]
        public ActionResult Quotes()
        {
            return View(new QuotesVM());
        }

        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_VIEW)]
        public ActionResult GetQuotes(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            List<QuotesVM> listQuoteVM = new List<QuotesVM>();
            List<QuoteModel> listQuoteModel = new List<QuoteModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listQuoteModel = quoteBusiness.GetQuotesByCompanyId(companyId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords);

            AutoMapper.Mapper.Map(listQuoteModel, listQuoteVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                List = listQuoteVM
            }
          , JsonRequestBehavior.AllowGet);

        }

        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_VIEW)]
        public ActionResult SaleOrders()
        {
            return View(new SaleOrdersVM());
        }


        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_SALESORDER + Constants.PERMISSION_VIEW)]
        public ActionResult GetSaleOrders(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            List<SaleOrdersVM> listSalesOrderVM = new List<SaleOrdersVM>();
            List<SalesOrderModel> listSalesOrderModel = new List<SalesOrderModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listSalesOrderModel = salesOrderBusiness.GetSalesOrdersByCompanyId(SessionManagement.LoggedInUser.UserId, companyId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords, listingParameters.sortColumnName, listingParameters.sortdir);

            AutoMapper.Mapper.Map(listSalesOrderModel, listSalesOrderVM);


            return Json(new
            {
                TotalRecords = totalRecords,
                List = listSalesOrderVM
            }
          , JsonRequestBehavior.AllowGet);


        }
        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_VIEW)]
        public ActionResult Invoices()
        {
            return View(new InvoicesVM());
        }

        [HttpGet]
        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_VIEW)]
        public ActionResult GetInvoices(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            List<InvoicesVM> listInvoiceVM = new List<InvoicesVM>();
            List<InvoiceModel> listInvoiceModel = new List<InvoiceModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            listInvoiceModel = invoiceBusiness.GetInvoicesByCompanyId(companyId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords);

            AutoMapper.Mapper.Map(listInvoiceModel, listInvoiceVM);


            return Json(new
            {
                TotalRecords = totalRecords,
                List = listInvoiceVM
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAccounts(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            List<AccountModel> listAccountModel = new List<AccountModel>();
            List<AccountVM> listAccountVM = new List<AccountVM>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int userId = SessionManagement.LoggedInUser.UserId;
            //listAccountModel = accountBussiness.GetAccounts(companyId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords);
            listAccountModel = accountBussiness.GetAccountsByUserId(companyId, userId, listingParameters.TagId.Decrypt(), listingParameters.TagSearchName, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords);
            AutoMapper.Mapper.Map(listAccountModel, listAccountVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                ListAccount = listAccountVM
            }
          , JsonRequestBehavior.AllowGet);


        }
        public ActionResult GetCaseMessageBoardMessages(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            List<CaseMessageBoardModel> caseMessageBoardModelList = new List<CaseMessageBoardModel>();
            List<CaseMessageBoardVM> caseMessageBoardVMList = new List<CaseMessageBoardVM>();
            int accountCaseId = listingParameters.AccountCaseId.Decrypt();
            caseMessageBoardModelList = caseMessageBoardBusiness.GetCaseMessageBoardMessages(accountCaseId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords);
            AutoMapper.Mapper.Map(caseMessageBoardModelList, caseMessageBoardVMList);
            return Json(new
            {
                TotalRecords = totalRecords,
                List = caseMessageBoardVMList
            }
          , JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccountCases()
        {

            return View(new AccountCaseVM());
        }
        public ActionResult GetAccountCases(ListingParameters listingParameters)
        {
            List<AccountCaseVM> accountCaseVMList = new List<AccountCaseVM>();
            List<AccountCaseModel> accountCaseModelList = new List<AccountCaseModel>();
            int totalRecords = 0;
            int userId = SessionManagement.LoggedInUser.UserId;
            accountCaseModelList = accountcaseBusiness.GetAccountCasesByUserId(userId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords, listingParameters.sortColumnName, listingParameters.sortdir);
            AutoMapper.Mapper.Map(accountCaseModelList, accountCaseVMList);
            return Json(new
            {
                TotalRecords = totalRecords,
                ListAccountCases = accountCaseVMList
            }
             , JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetDashBoardAccountCases()
        {
            List<AccountCaseVM> accountCaseVMList = new List<AccountCaseVM>();
            List<AccountCaseModel> accountCaseModelList = new List<AccountCaseModel>();

            int userId = SessionManagement.LoggedInUser.UserId;
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            accountCaseModelList = accountcaseBusiness.GetDashBoardAccountCases(companyId, userId);
            AutoMapper.Mapper.Map(accountCaseModelList, accountCaseVMList);
            return Json(new
            {

                List = accountCaseVMList
            }
             , JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAccountCaseAttachments(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            IList<FileAttachmentModel> fileAttachmentModelList = new List<FileAttachmentModel>();
            IList<FileAttachmentVM> fileAttachmentVMList = new List<FileAttachmentVM>();
            int accountCaseId = listingParameters.AccountCaseId.Decrypt();
            fileAttachmentModelList = fileAttachmentBusiness.GetAttachmentsByAccountCaseId(accountCaseId, listingParameters.CurrentPage, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalRecords);
            AutoMapper.Mapper.Map(fileAttachmentModelList, fileAttachmentVMList);
            return Json(new
            {
                TotalRecords = totalRecords,
                List = fileAttachmentVMList
            }
          , JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountCaseTasks(ListingParameters listingParameters)
        {
            int totalRecords = 0;
            IList<TaskItemModel> userListModel = taskBusiness.GetAccountCaseTasks(
                SessionManagement.LoggedInUser.CompanyId.Value, listingParameters.AccountCaseId.Decrypt(),
                listingParameters.CurrentPage,
                ErucaCRM.Utility.ReadConfiguration.PageSize,
                ref totalRecords);
            List<TaskItemVM> listTaskVModels = new List<TaskItemVM>();
            AutoMapper.Mapper.Map(userListModel, listTaskVModels);

            foreach (TaskItemVM item in listTaskVModels)
            {
                item.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item.PriorityId);
                item.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), item.Status);
                item.TaskDueDate = item.DueDate.ToShortDateString();
            }
            Response<TaskItemVM> response = new Response<TaskItemVM>();
            response.TotalRecords = totalRecords;
            response.List = listTaskVModels;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorize(Roles = Constants.MODULE_ROLE + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public ActionResult DeleteRole(string roleId_encrypted, string reassignedId_encrypted)
        {
            bool result;
            result = roleBusiness.DeleteRole(Convert.ToInt32(roleId_encrypted), Convert.ToInt32(reassignedId_encrypted)
, SessionManagement.LoggedInUser.CompanyId.Value);
            return Json(new
            {
                Status = result,
                JsonRequestBehavior.AllowGet
            });

        }
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public ActionResult DeleteProfile(string profileId_encrypted, string reassignProfileId_encrypted)
        {
            bool result;
            result = userBusiness.DeleteProfile(Convert.ToInt32(profileId_encrypted), Convert.ToInt32(reassignProfileId_encrypted), SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);
            return Json(new
            {
                Status = result,
                JsonRequestBehavior.AllowGet
            });
        }


        [CustomAuthorize(Roles = Constants.MODULE_QUOTE + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public ActionResult DeleteQuote(string Id_encrypted)
        {
            bool result;
            result = quoteBusiness.DeleteQuote(Convert.ToInt32(Id_encrypted), SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);
            return Json(new Response() { Status = Convert.ToString(result), Message = "", StatusCode = 200 });

        }

        [CustomAuthorize(Roles = Constants.MODULE_INVOICE + Constants.PERMISSION_DELETE)]
        [EncryptedActionParameter]
        public ActionResult DeleteInvoice(string Id_encrypted)
        {
            bool result;
            result = invoiceBusiness.DeleteInvoice(Convert.ToInt32(Id_encrypted), SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);
            return Json(new Response() { Status = Convert.ToString(result), Message = "", StatusCode = 200 });

        }
        [EncryptedActionParameter]
        public ActionResult DeleteTaskItem(string Id_encrypted)
        {
            bool result;
            result = taskBusiness.DeleteTaskItem(Convert.ToInt32(Id_encrypted), SessionManagement.LoggedInUser.UserId);
            return Json(new Response() { Status = Convert.ToString(result), Message = "", StatusCode = 200 });
        }
        [ChildActionOnly]
        public ActionResult AddProduct()
        {
            return PartialView("_AddProduct", new ProductVM());

        }
        public List<CultureInformationVM> GetCultureList()
        {
            List<CultureInformationModel> cultureInfoList = new List<CultureInformationModel>();
            List<CultureInformationVM> cultureInfoVMList = new List<CultureInformationVM>();
            cultureInfoList = cultureBusiness.GetUserCultures();
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

        public ActionResult Tags()
        {
            return View(new TagVM());
        }

        public ActionResult GetTagList(ListingParameters listingParameters)
        {

            List<TagVM> listTagVM = new List<TagVM>();
            List<TagModel> listTagModel = new List<TagModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;

            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            listTagModel = tagBusiness.GetAllTags(companyId, listingParameters.CurrentPage, pageSize, ref totalRecords);


            AutoMapper.Mapper.Map(listTagModel, listTagVM);

            return Json(new
            {
                TotalRecords = totalRecords,
                ListTags = listTagVM
            }
          , JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTaggedAccountList(ListingParameters listingParameters)
        {

            List<AccountVM> listAccountVM = new List<AccountVM>();
            List<AccountModel> listAccountModel = new List<AccountModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            int tagId = listingParameters.TagId.Decrypt();
            listAccountModel = accountBussiness.GetTaggedAccounts(tagId, companyId, listingParameters.CurrentPage, pageSize, ref totalRecords);

            AutoMapper.Mapper.Map(listAccountModel, listAccountVM);

            return Json(new
            {
                TotalRecords = totalRecords,
                ListAccount = listAccountVM
            }
          , JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTaggedContactList(ListingParameters listingParameters)
        {

            List<ContactVM> listContactVM = new List<ContactVM>();
            List<ContactModel> listContactModel = new List<ContactModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            int tagId = listingParameters.TagId.Decrypt();
            //  listContactModel = contactBusiness.GetTaggedContacts(tagId, companyId, listingParameters.CurrentPage, pageSize, ref totalRecords);
            listContactModel = contactBusiness.GetAllContacts(companyId, listingParameters.CurrentPage, pageSize, SessionManagement.LoggedInUser.UserId, "", tagId, "", ref totalRecords);
            AutoMapper.Mapper.Map(listContactModel, listContactVM);

            return Json(new
            {
                TotalRecords = totalRecords,
                ListContacts = listContactVM
            }
          , JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTaggedLeadList(ListingParameters listingParameters)
        {

            List<LeadVM> listLeadVM = new List<LeadVM>();
            List<LeadModel> listLeadModel = new List<LeadModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            int tagId = listingParameters.TagId.Decrypt();
            listLeadModel = leadBusiness.GetTaggedLead(tagId, companyId, listingParameters.CurrentPage, pageSize, ref totalRecords);
            AutoMapper.Mapper.Map(listLeadModel, listLeadVM);
            return Json(new
            {
                TotalRecords = totalRecords,
                ListLead = listLeadVM
            }
          , JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetTaggedContactSearchByName(ListingParameters listingParameters)
        {

            List<ContactVM> listContactVM = new List<ContactVM>();
            List<ContactModel> listContactModel = new List<ContactModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            int tagId = listingParameters.TagId.Decrypt();
            //listContactModel = contactBusiness.GetTaggedContactSearchByName(listingParameters.TagSearchName, companyId, listingParameters.CurrentPage, pageSize, ref totalRecords);
            listContactModel = contactBusiness.GetAllContacts(companyId, listingParameters.CurrentPage, pageSize, SessionManagement.LoggedInUser.UserId, listingParameters.TagSearchName, tagId, "", ref totalRecords);
            AutoMapper.Mapper.Map(listContactModel, listContactVM);

            return Json(new
            {
                TotalRecords = totalRecords,
                ListContacts = listContactVM
            }
          , JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetSearchTagList(string searchText)
        {


            List<AutoCompleteModel> listTagAutoCompleteModel = new List<AutoCompleteModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;

            listTagAutoCompleteModel = tagBusiness.GetSearchTags(companyId, searchText);



            return Json(new
            {
                ListTags = listTagAutoCompleteModel
            }
          , JsonRequestBehavior.AllowGet);

        }




        [HttpPost]
        public ActionResult AddTag(TagVM tagVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            bool _isAdded;
            if (ModelState.IsValid)
            {
                TagModel tagModel = new TagModel();
                Mapper.Map(tagVM, tagModel);
                tagModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                tagModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                tagModel.CreatedDate = DateTime.UtcNow.ToDateTimeNow();
                _isAdded = tagBusiness.AddTag(tagModel);
                if (_isAdded == true)
                {
                    response.Status = Enums.ResponseResult.Success.ToString();
                }
                else
                {
                    response.Status = Enums.ResponseResult.Failure.ToString();
                    response.Message = "TagAlreadyExists";

                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);
            //   return View();
        }
        [HttpPost]
        public ActionResult UpdateTag(TagVM tagVM)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            bool _isAdded;
            if (ModelState.IsValid)
            {
                TagModel tagModel = new TagModel();
                Mapper.Map(tagVM, tagModel);
                tagModel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                tagModel.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
                tagModel.CreatedDate = DateTime.UtcNow.ToDateTimeNow();
                _isAdded = tagBusiness.UpdateTag(tagModel);
                if (_isAdded == true)
                {
                    response.Status = Enums.ResponseResult.Success.ToString();
                }
                else
                {
                    response.Status = Enums.ResponseResult.Failure.ToString();
                    response.Message = "TagAlreadyExists";

                }
            }
            else
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                        response.Message += error.ErrorMessage;
                }                //response.Message = ModelState.err;

            }
            return Json(response);

        }
        [CustomAuthorize(Roles = Constants.MODULE_TAG + Constants.PERMISSION_DELETE)]
        [HttpDelete]
        public ActionResult DeleteTag(string Id_encrypted)
        {
            bool result;
            result = tagBusiness.DeleteTag(Id_encrypted.Decrypt(), SessionManagement.LoggedInUser.UserId);
            return Json(new Response() { Status = Convert.ToString(result), Message = "", StatusCode = 200 });

        }
        [CustomAuthorize(Roles = Constants.MODULE_TAG + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult TagDetail(string Id_encrypted)
        {
            TagModel tagModel = tagBusiness.GetTagDetails(Convert.ToInt32(Id_encrypted));
            TagVM tagVM = new TagVM();

            AutoMapper.Mapper.Map(tagModel, tagVM);
            return View(tagVM);
        }


        [EncryptedActionParameter]
        public JsonResult DeleteAccount(string id_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (accountBussiness.DeleteAccount(Convert.ToInt32(id_encrypted), SessionManagement.LoggedInUser.UserId))
            {

                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {

                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }


        [EncryptedActionParameter]
        public JsonResult DeleteContact(string id_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (contactBusiness.DeleteContact(Convert.ToInt32(id_encrypted), SessionManagement.LoggedInUser.UserId))
            {
                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {

                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }

        [EncryptedActionParameter]
        [HttpDelete]
        public JsonResult DeleteMemberAccount(string memeberAcctId_encrypted, string parentAcctId_encrypted)
        {
            string meassage = "";
            string status = "";
            int code = 200;
            if (accountBussiness.DeleteMemberAccount(Convert.ToInt32(memeberAcctId_encrypted), Convert.ToInt32(parentAcctId_encrypted), SessionManagement.LoggedInUser.UserId))
            {

                status = Enums.ResponseResult.Success.ToString();
                code = 200;
            }
            else
            {

                status = Enums.ResponseResult.Failure.ToString();
                code = 400;
            }

            return Json(new Response
            {
                Message = meassage,
                StatusCode = code
                ,
                Status = status
            });

        }
        public ActionResult GetMostUsedAccountTags()
        {
            List<TagVM> listTags = new List<TagVM>();
            List<TagModel> listTagModels = new List<TagModel>();
            listTagModels = accountBussiness.GetMostUsedAccountTags((int)SessionManagement.LoggedInUser.CompanyId);
            AutoMapper.Mapper.Map(listTagModels, listTags);

            return Json(new
            {

                listTags = listTags
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetLeadTags()
        {
            List<TagVM> listTags = new List<TagVM>();
            List<TagModel> listTagModels = new List<TagModel>();
            listTagModels = leadBusiness.GetLeadTags((int)SessionManagement.LoggedInUser.CompanyId);
            AutoMapper.Mapper.Map(listTagModels, listTags);
            return Json(new
            {

                listTags = listTags
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetTaggedAccountSearchByName(ListingParameters listingParameters)
        {

            List<AccountVM> listAccountVM = new List<AccountVM>();
            List<AccountModel> listAccountModel = new List<AccountModel>();
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            int tagId = listingParameters.TagId.Decrypt();
            listAccountModel = accountBussiness.GetTaggedAccountSearchByName(listingParameters.TagSearchName, companyId, listingParameters.CurrentPage, pageSize, ref totalRecords);

            AutoMapper.Mapper.Map(listAccountModel, listAccountVM);

            return Json(new
            {
                TotalRecords = totalRecords,
                ListAccount = listAccountVM
            }
          , JsonRequestBehavior.AllowGet);

        }


        private void RealTimeNotification(int LeadId)
        {

            List<HomeModel> homeModelList = new List<HomeModel>();
            int maxLeadAuditId = 0;
            int companyid = SessionManagement.LoggedInUser.CompanyId.Value;
            int curentuserid = SessionManagement.LoggedInUser.UserId;
            List<int?> realTimeNotificationId = realTimeNotificationBusiness.GetNotifyClientByCompanyId(companyid, curentuserid, LeadId, ref maxLeadAuditId);
            homeModelList = homeBusiness.GetRecentActivitiesForHome(1, 1, maxLeadAuditId, false, 0, 0);

            var context = GlobalHost.ConnectionManager.GetHubContext<RealTimeNotificationHub>();
            foreach (int userid in realTimeNotificationId)
            {
                List<HomeVM> homeVMList = new List<HomeVM>();
                SessionManagement.CurrentUserID = userid; 
                foreach (HomeModel home in homeModelList)
                {
                    HomeVM homevm = new HomeVM();
                    homevm.ActivityType = home.ActivityType;
                    homevm.Amount = home.Amount;
                    homevm.ClosedLead = home.ClosedLead;
                    homevm.ClosingDate = home.ClosingDate;
                    homevm.CompanyId = home.CompanyId;
                    homevm.CreatedBy = home.CreatedBy.Value.Encrypt();
                    homevm.CreatedDate = home.CreatedDate;
                    homevm.FirstName = home.FirstName;
                    homevm.FromDate = home.FromDate;
                    homevm.ImageURL = home.ImageURL;
                    homevm.IsDisplay = home.IsDisplay;
                    homevm.LastName = home.LastName;
                    homevm.LeadAuditId = home.LeadAuditId.Encrypt();
                    homevm.LeadId = home.LeadId.Value.Encrypt();
                    homevm.LeadsInStages = home.LeadsInStages;
                    homevm.Title = home.Title;
                    homevm.ToDate = home.ToDate;
                    homevm.StageId = home.StageId.Value.Encrypt();
                    if (home.RatingId != null)
                    {
                        homevm.RatingId = home.RatingId.Value.Encrypt();
                    }
                    homevm.WinLead = home.WinLead;
                    homevm.ActivityType = home.ActivityType;
                    homevm.RatingIcon = home.Icons;
                    homevm.IsClosedWin = home.IsClosedWin;
                  
                    homeVMList.Add(homevm);


                }

                context.Clients.User(userid.Encrypt()).NewNotification(new { RecentActivities = homeVMList.Select(x => new { CreatedBy = x.CreatedBy, ImageURL = x.ImageURL, LeadAuditId = x.LeadAuditId, ActivityText = x.ActivityText, ActivityCreatedTime = x.ActivityCreatedTime, StageId = x.StageId, RatingId = x.RatingId, IsClosedWin = x.IsClosedWin, LeadId = x.LeadId, ActivityType = x.ActivityType, RatingIcon = x.RatingIcon, Title = x.Title, OwnerName = x.FirstName+" "+x.LastName }), MaxLeadAuditID = maxLeadAuditId.Encrypt() });
            }
            SessionManagement.CurrentUserID = null; 
        }
        [EncryptedActionParameter]
        public ActionResult ChangeLeadStage(string leadId_encrypted, string fromStage_encrypted, string toStage_encrypted, string LeadName = "", bool IsClosedWin = false)
        {
            LeadModel leadModel = new LeadModel();

            leadModel.LeadId = Convert.ToInt32(leadId_encrypted);
            leadModel.StageId = Convert.ToInt32(toStage_encrypted);
            leadModel.IsClosedWin = IsClosedWin;
            leadModel.ModifiedBy = SessionManagement.LoggedInUser.UserId;
            leadModel = leadBusiness.UpdateLeadStage(leadModel);
            Response response = new Response();
            RealTimeNotification(leadModel.LeadId);
            if (leadModel != null)
            {
                response.Status = Enums.ResponseResult.Success.ToString();
            }
            else response.Status = Enums.ResponseResult.Failure.ToString();
            return Json(new
          {
              response,
              LeadRating = leadModel.Rating
          }
                , JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult GetLeadHistory(string leadId_encrypted, int CurrentPageNo)
        {
            List<LeadAuditVM> leadAuditVM = new List<LeadAuditVM>();
            int totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            List<LeadAuditModel> leadAuditModel = leadAuditBusiness.GetLeadHistorybyLeadId(Convert.ToInt32(leadId_encrypted), CurrentPageNo, pageSize, ref totalRecords);
            AutoMapper.Mapper.Map(leadAuditModel, leadAuditVM);
            return Json(new
            {
                Response = "Success",
                TotalRecords = totalRecords,
                LeadHistory = leadAuditVM.Select(x => new { StageName = x.StageName, Proability = x.Proability, StageId = x.StageId, CultureSpcificAmount = x.CultureSpcificAmount, ExpectedRevenue = x.ExpectedRevenue, LeadClosingDate = x.LeadClosingDate, Duration = x.Duration, HistoryActivityType = x.HistoryActivityType, LeadStageFromDate = x.LeadStageFromDate })
            }
                  , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_LEAD + Constants.PERMISSION_VIEW)]
        [EncryptedActionParameter]
        public ActionResult GetLeadHistoryChartDetails(string leadId_encrypted)
        {

            List<LeadHistoryChartModel> leadHistoryChartModel = leadAuditBusiness.GetLeadHistoryChartDetails(Convert.ToInt32(leadId_encrypted));

            return Json(new
            {
                Response = "Success",
                LeadHistoryChartDetail = leadHistoryChartModel
            }
           , JsonRequestBehavior.AllowGet);
        }


        [EncryptedActionParameter]
        public ActionResult ChangeLeadRating(string leadId_encrypted, string ratingId_encrypted)
        {
            LeadModel leadModel = new LeadModel();
            LeadVM leadVM = new LeadVM();
            leadModel = leadBusiness.UpdateLeadRating(Convert.ToInt32(leadId_encrypted), Convert.ToInt32(ratingId_encrypted), SessionManagement.LoggedInUser.UserId);
            Response response = new Response();
            if (leadModel != null)
            {
                response.Status = Enums.ResponseResult.Success.ToString();
            }
            else response.Status = Enums.ResponseResult.Failure.ToString();
            AutoMapper.Mapper.Map(leadModel, leadVM);

            return Json(new { Status = response.Status, Lead = leadVM }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStagesByCompanyId()
        {
            List<StageModel> stageModelList = new List<StageModel>();
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            stageModelList = stageBusiness.GetStages(companyId);
            return Json(new
            {
                Response = "Success",
                CompanyLeadStages = stageModelList
            }
                  , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = Constants.MODULE_STAGE + Constants.PERMISSION_VIEW)]
        public ActionResult Stages()
        {
            List<RatingVM> ratingVMList = new List<RatingVM>();
            List<RatingModel> ratingModellist = ratingBusiness.GetRatings();
            AutoMapper.Mapper.Map(ratingModellist, ratingVMList);
            ViewBag.Ratings = ratingVMList;

            return View(new StageVM());
        }

        public ActionResult StagesList()
        {
            List<StageModel> stageModelList = new List<StageModel>();
            List<StageVM> stageVMList = new List<StageVM>();
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            stageModelList = stageBusiness.GetStages(companyId);
            AutoMapper.Mapper.Map(stageModelList, stageVMList);
            return Json(stageVMList
                , JsonRequestBehavior.AllowGet);
        }


        [EncryptedActionParameter]
        public ActionResult CreateNewStage(StageModel stageModel, string DefaultRatingId_encrypted, string StageId_encrypted, string StageLeadduration)
        {
            string isSaved = string.Empty;
            stageModel.StageId = Convert.ToInt32(StageId_encrypted);
            stageModel.DefaultRatingId = Convert.ToInt32(DefaultRatingId_encrypted);
            stageModel.CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            stageModel.CreatedBy = SessionManagement.LoggedInUser.UserId;

            if (StageLeadduration == "")
            {
                StageLeadduration = null;
            }

            stageModel.StageLeadDuration = Convert.ToInt32(StageLeadduration);
            isSaved = stageBusiness.SaveStage(stageModel);
            Response response = new Response();
            response.Status = isSaved;
            return Json(response
              , JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult UpdateStageOrder(List<StageSort> SortArray)
        {
            var isSaved = false;
            Response response = new Response();
            isSaved = stageBusiness.UpdateStageOrder(SortArray);
            if (isSaved)
                response.Status = Enums.ResponseResult.Success.ToString();
            else
                response.Status = Enums.ResponseResult.Failure.ToString();
            return Json(response
              , JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        [CustomAuthorize(Roles = Constants.MODULE_PROFILE + Constants.PERMISSION_VIEW)]
        public ActionResult GetStageDetail(string StageId_encrypted)
        {
            StageModel stageModel = new StageModel();
            StageVM stageVM = new StageVM();
            stageModel = stageBusiness.GetStage(Convert.ToInt32(StageId_encrypted));
            Response response = new Response();
            AutoMapper.Mapper.Map(stageModel, stageVM);
            ViewBag.RatingId = stageVM.DefaultRatingId;
            return Json(stageVM
              , JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult GetLeadCompanyAndContactStatus(string leadId_encrypted)
        {
            LeadModel leadModel = new LeadModel();
            leadModel = leadBusiness.GetLeadDetail(Convert.ToInt32(leadId_encrypted));
            List<ContactModel> contactModel = new List<ContactModel>();
            if (leadModel.LeadContactModel.Count > 0)
            {
                return Json(new { CompanyName = leadModel.LeadCompanyName, LeadOwnerName = leadModel.UserModel.FullName, Amount = leadModel.Amount, HasContact = leadModel.LeadContactModel.Count > 0 ? true : false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                contactModel = contactBusiness.GetContactList(SessionManagement.LoggedInUser.UserId, SessionManagement.LoggedInUser.CompanyId.Value);
                List<DropDownHelper> contactList = new List<DropDownHelper>();
                AutoMapper.Mapper.Map(contactModel, contactList);
                return Json(new { CompanyName = leadModel.LeadCompanyName, LeadOwnerName = leadModel.UserModel.FullName, Amount = leadModel.Amount, HasContact = leadModel.LeadContactModel.Count > 0 ? true : false, ContactList = contactList }, JsonRequestBehavior.AllowGet);

            }
        }

        [EncryptedActionParameter]
        public ActionResult UpdateLeadCompanyAndContactStatus(string leadId_encrypted, string CompanyName, string Amount, string ContactName, string ContactEmail, string id_encrypted)
        {
            LeadModel leadModel = new LeadModel();
            leadModel = leadBusiness.GetLeadDetail(Convert.ToInt32(leadId_encrypted));
            leadModel.LeadCompanyName = CompanyName;
            leadModel.Amount = Convert.ToDecimal(Amount);
            if (Convert.ToInt32(id_encrypted) > 0)
            {
                contactBusiness.AssociateLeadToContact(Convert.ToInt32(leadId_encrypted), Convert.ToInt32(id_encrypted), SessionManagement.LoggedInUser.UserId);

            }
            else if ((!String.IsNullOrEmpty(ContactEmail) && !String.IsNullOrEmpty(ContactName)))
            {
                ContactModel contactModel = new ContactModel();
                contactModel.FirstName = ContactName;
                contactModel.EmailAddress = ContactEmail;
                contactModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
                contactModel.CreatedDate = DateTime.Now;
                contactModel.OwnerId = leadModel.LeadOwnerId.Value;//SessionManagement.LoggedInUser.UserId;
                contactModel.CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
                contactModel.LeadId = leadModel.LeadId;
                contactBusiness.AddContact(contactModel);
            }
            leadBusiness.UpdateLead(leadModel);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [EncryptedActionParameter]
        public ActionResult GetLeadProducts(string leadId_encrypted, int CurrentPageNo)
        {
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            int totalrecord = 0;
            List<ProductModel> lstProductModel = new List<ProductModel>();
            List<ProductVM> productVMList = new List<ProductVM>();

            lstProductModel = productBusiness.GetLeadProducts(companyId, Convert.ToInt32(leadId_encrypted), CurrentPageNo, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalrecord);
            AutoMapper.Mapper.Map(lstProductModel, productVMList);
            return Json(new
            {
                TotalRecords = totalrecord,
                LeadProducts = productVMList
            }
          , JsonRequestBehavior.AllowGet);

        }

        [EncryptedActionParameter]
        public ActionResult GetLeadDocuments(string leadId_encrypted, int CurrentPageNo)
        {
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            int totalrecord = 0;
            List<FileAttachmentModel> fileAttachmentModel = new List<FileAttachmentModel>();
            List<FileAttachmentVM> fileAttachmentVMList = new List<FileAttachmentVM>();

            fileAttachmentModel = fileAttachmentBusiness.GetLeadDocuments(companyId, Convert.ToInt32(leadId_encrypted), CurrentPageNo, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalrecord);
            AutoMapper.Mapper.Map(fileAttachmentModel, fileAttachmentVMList);
            return Json(new
            {
                TotalRecords = totalrecord,
                LeadDocuments = fileAttachmentVMList
            }
          , JsonRequestBehavior.AllowGet);

        }
        [EncryptedActionParameter]
        public ActionResult DeleteStageAfterMoveLeads(string OldStageId_encrypted, string NewStageId_encrypted)
        {
            bool result = false;
            Response response = new Response();
            result = stageBusiness.DeleteStage(Convert.ToInt32(OldStageId_encrypted), Convert.ToInt32(NewStageId_encrypted));
            if (result)
                response.Status = Enums.ResponseResult.Success.ToString();
            else
                response.Status = Enums.ResponseResult.Failure.ToString();
            return Json(response
              , JsonRequestBehavior.AllowGet);
        }



        [EncryptedActionParameter]
        public ActionResult GetAllProducts(string leadId_encrypted, int CurrentPageNo)
        {
            int companyId = SessionManagement.LoggedInUser.CompanyId.Value;
            int totalrecord = 0;
            List<ProductModel> productList = new List<ProductModel>();
            List<ProductVM> productVMList = new List<ProductVM>();

            productList = productBusiness.GetProductsByCompanyId(companyId, Convert.ToInt32(leadId_encrypted), CurrentPageNo, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalrecord);
            AutoMapper.Mapper.Map(productList, productVMList);
            return Json(new
            {
                TotalRecords = totalrecord,
                Products = productVMList
            }
          , JsonRequestBehavior.AllowGet);

        }
        [EncryptedActionParameter]
        public ActionResult GetLeadContacts(string leadId_encrypted, int CurrentPageNo)
        {
            int CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<ContactVM> contactVMList = new List<ContactVM>();
            int totalrecord = 0;
            List<ContactModel> contactModelList = contactBusiness.GetAssociatedContactByLeadId(SessionManagement.LoggedInUser.UserId, Convert.ToInt32(leadId_encrypted), CompanyId, CurrentPageNo, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalrecord);
            AutoMapper.Mapper.Map(contactModelList, contactVMList);
            return Json(new
            {
                TotalRecords = totalrecord,
                LeadContacts = contactVMList
            }
                     , JsonRequestBehavior.AllowGet);

        }

        [EncryptedActionParameter]
        public ActionResult GetLeadComments(string leadId_encrypted, int CurrentPageNo)
        {
            List<LeadCommentVM> LeadCommentsVMList = new List<LeadCommentVM>();
            int totalrecord = 0;
            List<LeadCommentModel> LeadCommentsModelList = leadCommentBussiness.GetCommentsByLeadId(Convert.ToInt32(leadId_encrypted), CurrentPageNo, ErucaCRM.Utility.ReadConfiguration.PageSize, ref totalrecord);
            AutoMapper.Mapper.Map(LeadCommentsModelList, LeadCommentsVMList);
            return Json(new
            {
                TotalRecords = totalrecord,
                LeadComment = LeadCommentsVMList
            }
                               , JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddLeadComment(LeadCommentVM leadCommentVM)
        {
            LeadCommentModel leadCommentModel = new LeadCommentModel();
            AutoMapper.Mapper.Map(leadCommentVM, leadCommentModel);
            leadCommentModel.CreatedBy = SessionManagement.LoggedInUser.UserId;
            leadCommentModel.UserId = SessionManagement.LoggedInUser.UserId;
            leadCommentModel = leadCommentBussiness.AddCommentInLead(leadCommentModel);
            leadCommentModel.UserName = SessionManagement.LoggedInUser.FullName;
            leadCommentModel.UserImg = SessionManagement.LoggedInUser.ProfileImageUrl;
            leadCommentVM = new LeadCommentVM();
            AutoMapper.Mapper.Map(leadCommentModel, leadCommentVM);
            return Json(new
            {
                LeadComment = leadCommentVM
            }
                               , JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveNewContact(string ContactName, string EmailAddress)
        {
            ContactModel model = new ContactModel();
            model.FirstName = ContactName;
            model.EmailAddress = EmailAddress;
            model.CreatedBy = SessionManagement.LoggedInUser.UserId;
            model.OwnerId = SessionManagement.LoggedInUser.UserId;
            model.CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            int contactId = contactBusiness.AddContact(model);

            return Json(new { status = "Success", ContactId = contactId.Encrypt() }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult AssociateAccountContact(List<AccountLeadContactInfo> accountContactList)
        {
            List<AccountContactModel> acccontactList = new List<AccountContactModel>();
            AutoMapper.Mapper.Map(accountContactList, acccontactList);
            contactBusiness.AssociateAccountToContact(acccontactList, SessionManagement.LoggedInUser.UserId);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssociateLeadContact(List<AccountLeadContactInfo> accountContactList)
        {
            List<LeadContactModel> acccontactList = new List<LeadContactModel>();
            AutoMapper.Mapper.Map(accountContactList, acccontactList);
            contactBusiness.AssociateLeadToContact(acccontactList, SessionManagement.LoggedInUser.UserId);
            return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLeadAnalyticData(string Interval)
        {
            List<GetLeadAnalyticData_Result> data = leadBusiness.GetLeadAnalyticData(Interval, SessionManagement.LoggedInUser.CompanyId.Value);

            return Json(new { result = "success", data = data });

        }

        //Reports Section

        public ActionResult GetYearWiseLeadCount()
        {
            int CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<YearWiseLeadModel> data = leadBusiness.GetYearWiseLeadCount(CompanyId);
            return Json(new { result = "success", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMonthWiseLeadCount()
        {
            int CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<YearWiseLeadModel> data = leadBusiness.GetMonthWiseLeadCount(CompanyId);
            return Json(new { result = "success", data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWeekWiseLeadCount()
        {
            int CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<YearWiseLeadModel> data = leadBusiness.GetWeekWiseLeadCount(CompanyId);
            return Json(new { result = "success", data = data }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult DashBoardAndReports()
        {
            return View();
        }
        [CustomAuthorize(Roles = Constants.MODULE_REPORTSANDDASHBOARDS + Constants.PERMISSION_VIEW)]
        public ActionResult DashBoard(string leadId_encrypted)
        {
            return View(new HomeVM());
        }


        public ActionResult GetReportLeadsInPipleLine(string dateFilterOption)
        {

            List<LeadsInPipeLineModel> leadsInPipeLineModel = reportBusiness.GetLeadsInPiplineByStages(SessionManagement.LoggedInUser.CompanyId.Value, dateFilterOption);

            return Json(new
            {
                Response = "Success",
                ListLeadsInPipeLine = leadsInPipeLineModel
            }
           , JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMonthWiseAccountSaleRevenue()
        {

            List<AccountSaleRevenueModel> accountSaleRevenueModel = reportBusiness.GetMonthWiseAccountSaleRevenue(SessionManagement.LoggedInUser.CompanyId.Value);

            return Json(new
            {
                Response = "Success",
                ListMonthWiseAccountSaleRevenue = accountSaleRevenueModel
            }
           , JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetLeadsByStarRatingPercentage()
        {
            int CompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            LeadByStarRatingPercentageModel obj = new LeadByStarRatingPercentageModel();
            obj = leadBusiness.GetLeadsByStarRatingPercentage(CompanyId);
            return Json(new { result = "success", data = obj }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetAccountByTopHighestSaleRevenue()
        {

            List<AccountSaleRevenueModel> accountByTopHighestSaleRevenueModel = reportBusiness.GetAccountByTopHighestSaleRevenue(SessionManagement.LoggedInUser.CompanyId.Value);

            return Json(new
            {
                Response = "Success",
                ListAccountByTopHighestSaleRevenue = accountByTopHighestSaleRevenueModel
            }
           , JsonRequestBehavior.AllowGet);
        }
        [EncryptedActionParameter]
        public ActionResult GetRecentActivitiesForHome(int currentPage, string LeadAuditId_encrypted, bool IsLoadMore)
        {
            List<HomeVM> homeVMList = new List<HomeVM>();
            List<HomeModel> homeModelList = new List<HomeModel>();
            int pageSize = Convert.ToInt32(ErucaCRM.Utility.ReadConfiguration.PageSize);
            homeModelList = homeBusiness.GetRecentActivitiesForHome(pageSize, currentPage, Convert.ToInt32(LeadAuditId_encrypted), IsLoadMore, SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);

            AutoMapper.Mapper.Map(homeModelList, homeVMList);
            return Json(new { RecentActivities = homeVMList.Select(x => new { CreatedBy = x.CreatedBy, ImageURL = x.ImageURL, LeadAuditId = x.LeadAuditId, ActivityText = x.ActivityText, ActivityCreatedTime = x.ActivityCreatedTime }), Response = "Success", }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDashboardData(string Interval)
        {

            HomeVM homeVMlistobj = new HomeVM();
            HomeModel homemodellistobj = new HomeModel();
            homemodellistobj = homeBusiness.GetDashboardData(SessionManagement.LoggedInUser.CompanyId.Value, Interval);
            AutoMapper.Mapper.Map(homemodellistobj, homeVMlistobj);
            return Json(new
       {
           RecentActivities = homeVMlistobj,
           Response = "Success",
       }
      , JsonRequestBehavior.AllowGet);
        }



       // [EncryptedActionParameter]
       // public ActionResult AutoRefreshLeads(string tagName, string SearchLeadName = "")
       // {
       //     List<LeadAuditVM> LeadListVm = new List<LeadAuditVM>();
       //     List<LeadAuditModel> LeadListModel = leadAuditBusiness.GetAutoLeadsAudits(SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId, tagName, SearchLeadName);
       //     AutoMapper.Mapper.Map(LeadListModel, LeadListVm);
       //     return Json(new
       // {
       //     LeadList = LeadListVm.Select(x => new { StageId_encrypted = x.StageId_encrypted, RatingId_encrypted = x.RatingId_encrypted, IsClosedWin = x.IsClosedWin, RatingIcon = x.RatingIcon, OwnerName = x.OwnerName, ActivityText = x.ActivityText, LeadId_encrypted = x.LeadId_encrypted, Title = x.Title }),
       //     Status = "Success",
       // }
       //, JsonRequestBehavior.AllowGet);
       // }

        public ActionResult AccountSettings()
        {
            AccountSettingVM accounSettingVM = new AccountSettingVM();
            UserSettingModel accountSettingModel = userBusiness.GetUserAccountSetting(SessionManagement.LoggedInUser.UserId);
            if (accountSettingModel != null)
            {
                accounSettingVM.UserSettingId = accountSettingModel.UserSettingId;
                accounSettingVM.IsSendNotificationsRecentActivities = accountSettingModel.IsSendNotificationsRecentActivities ?? false;

            }

            return View(accounSettingVM);
        }
        [HttpPost]
        public ActionResult AccountSettings(AccountSettingVM accountSetting)
        {
            if (accountSetting != null)
            {
                accountSetting.UserId = SessionManagement.LoggedInUser.UserId;
                UserSettingModel accountSettingModel = new UserSettingModel();
                AutoMapper.Mapper.Map(accountSetting, accountSettingModel);
                userBusiness.UpdateUserAccountSetting(accountSettingModel);

            }
            return RedirectToAction("AccountSettings");
        }

        public ActionResult BulkUploadContacts(FileAttachmentVM cultureVM)
        {
            int RecordInserted = 0;
            string FileName = string.Empty;
            string filePathAndName = string.Empty;
            string newFileName = string.Empty;
            DataSet dsObject = new DataSet();
            HttpPostedFileBase file = Request.Files[0];
            CultureInformationModel cultureInfoModel = new CultureInformationModel();
            string fileSavingPath = string.Empty;
            string fileExtension = Path.GetExtension(file.FileName);
            List<ContactBulkUploadModel> contactModel = new List<ContactBulkUploadModel>();
            if (file.ContentLength > 0)
            {
                if ((fileExtension == ".xlsx" || fileExtension == ".xls"))
                {

                    try
                    {

                        string DocPath = ReadConfiguration.BulkUploadContactFilePath;
                        string uniqueName = Guid.NewGuid().ToShortGuid(6);
                        FileName = Path.GetFileNameWithoutExtension(file.FileName);
                        newFileName = FileName + "_" + uniqueName + fileExtension;
                        filePathAndName = DocPath + newFileName;
                        fileSavingPath = Server.MapPath(@"~" + filePathAndName);
                        CommonFunctions.UploadFile(file, newFileName, "", Constants.CONTACT_BULKUPLOAD_DOCS_BLOB);
                        dsObject = CommonFunctions.ImportExceltoDatasetContact(newFileName,Constants.CONTACT_BULKUPLOAD_DOCS_BLOB);
                        DataTable ContactDataTable = dsObject.Tables[0];
                        if (ContactDataTable.Rows.Count > 0)
                        {

                            contactModel = contactBusiness.BulkInsertContact(ContactDataTable, SessionManagement.LoggedInUser.UserId, SessionManagement.LoggedInUser.CompanyId.Value);
                            RecordInserted = ContactDataTable.Rows.Count - contactModel.Count;
                            contactBusiness.DeleteFileFromServerAfterUpload(fileSavingPath);

                        }
                        else
                        {
                            contactBusiness.DeleteFileFromServerAfterUpload(fileSavingPath);
                            return Json(new { success = false, response = "NoRecordFound" });
                        }

                    }
                    catch (Exception ex)
                    {
                        contactBusiness.DeleteFileFromServerAfterUpload(fileSavingPath);
                        return Json(new { success = false, response = "Documentment not uploaded ." });

                    }
                    return Json(new { success = true, ContactsWithErrror = contactModel, recordInserted = RecordInserted, response = "Documentment uploaded  successfully" });
                }

            }
            return Json(new { success = false, response = "Documentment not uploaded ." });


        }

        [EncryptedActionParameter()]
        public JsonResult GetNotification(string maxLeadAuditId_encrypted, bool updateNotification)
        {
            int maxLeadAuditId = 0;
            int totalNotification = 0;
            int currentPage = 1;
            bool IsLoadMore = false;
            List<HomeVM> homeVMList = new List<HomeVM>();
            List<HomeModel> homeModelList = new List<HomeModel>();
            if (maxLeadAuditId_encrypted != "0")
            {

                int.TryParse(maxLeadAuditId_encrypted, out maxLeadAuditId);

            }
            leadAuditBusiness.GetNotification(ReadConfiguration.PageSize, SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId, ref maxLeadAuditId, updateNotification, ref totalNotification);
            int pageSize = Convert.ToInt32(ErucaCRM.Utility.ReadConfiguration.NotificationListPageSize);
            homeModelList = homeBusiness.GetRecentActivitiesForHome(pageSize, currentPage, Convert.ToInt32(maxLeadAuditId_encrypted), IsLoadMore, SessionManagement.LoggedInUser.CompanyId.Value, SessionManagement.LoggedInUser.UserId);

            AutoMapper.Mapper.Map(homeModelList, homeVMList);
            if (Request.Cookies["MaxLeadAuditId"] != null)
            {
                Response.Cookies.Remove("MaxLeadAuditId");

            }
            return Json(new { Response = "Success", TotalNotification = totalNotification, RecentActivities = homeVMList.Select(x => new { CreatedBy = x.CreatedBy, ImageURL = x.ImageURL, LeadAuditId = x.LeadAuditId, ActivityText = x.ActivityText, ActivityCreatedTime = x.ActivityCreatedTime }), MaxLeadAuditID = maxLeadAuditId.Encrypt() }, JsonRequestBehavior.AllowGet);
        }

       
    }

}




