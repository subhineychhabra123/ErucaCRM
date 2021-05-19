using AutoMapper;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
using ErucaCRM.Utility.WebClasses;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
namespace ErucaCRM.Web.Controllers
{
    [CustomAuthorize(Roles = Constants.AUTHENTICATION_ROLE_ADMIN)]
    public class AdminController : Controller
    {
        private ICultureInformationBusiness cultureBusiness;
        private IApplicationPageBusiness applicationPageBusiness;
        private ICompanyBusiness companyBusiness;
        private IPlanBusiness planBusiness;
        IUserBusiness userBusiness;
        public AdminController(ICultureInformationBusiness _cultureBusiness, IApplicationPageBusiness _applicationPage, IUserBusiness _userBusiness, IPlanBusiness _planBusiness, ICompanyBusiness _companyBusiness)
        {
            cultureBusiness = _cultureBusiness;
            applicationPageBusiness = _applicationPage;
            userBusiness = _userBusiness;
            planBusiness = _planBusiness;
            companyBusiness = _companyBusiness;
        }

        public ActionResult Plans()
        {
            return View();

        }

        public ActionResult GetAllPlans()
        {
            List<PlanVM> listPlanVM = new List<PlanVM>();
            List<PlanModel> listPlanModel = new List<PlanModel>();

            listPlanModel = planBusiness.GetAllPlans();

            AutoMapper.Mapper.Map(listPlanModel, listPlanVM);

            return Json(new
            {

                ListPlans = listPlanVM
            }, JsonRequestBehavior.AllowGet);

        }


        [EncryptedActionParameterAttribute]
        public ActionResult Plan(string Id_encrypted)
        {
            PlanVM planVM = new PlanVM();

            if (Id_encrypted != null)
            {
                PlanModel planModel = new PlanModel();

                planModel = planBusiness.GetPlanById(Convert.ToInt32(Id_encrypted));
                AutoMapper.Mapper.Map(planModel, planVM);

                return View(planVM);

            }

            //List<ModuleListModel> listModules = planBusiness.GetAllModules();

            //for (int i = 0; i < listModules.Count; i++)
            //{
            //    PlanModuleModel planModuleModel = null;

            //    if (planVM.PlanModules != null)
            //    {
            //        planModuleModel = planVM.PlanModules.Where(x => x.ModuleId == listModules[i].ModuleId).FirstOrDefault();

            //    }
            //    else
            //    {
            //        planVM.PlanModules = new List<PlanModuleModel>();
            //    }

            //    if (planModuleModel != null)
            //    {
            //        planModuleModel.ModuleName = listModules[i].ModuleName;
            //    }
            //    else
            //    {
            //        planModuleModel = new PlanModuleModel();
            //        planModuleModel.ModuleId = listModules[i].ModuleId;
            //        planModuleModel.ModuleName = listModules[i].ModuleName;
            //        planModuleModel.HasPermission = false;
            //        planModuleModel.HasPermissionAfterTrail = false;

            //        planVM.PlanModules.Add(planModuleModel);
            //    }

            //}


            return View(planVM);
        }

        [HttpPost]
        public ActionResult Plan(PlanVM planVM)
        {
            PlanModel planModel = new PlanModel();

            planVM.CreatedBy = (int)SessionManagement.LoggedInUser.UserId;
            planVM.CreatedDate = DateTime.UtcNow;
            planVM.ModifiedBy = (int)SessionManagement.LoggedInUser.UserId;
            planVM.ModifiedDate = DateTime.UtcNow;

            AutoMapper.Mapper.Map(planVM, planModel);
            bool resultsuccess = planBusiness.AddUpdatePlan(planModel);

            return RedirectToAction("Plans", "Admin");

            //return View(planVM);
        }
        [EncryptedActionParameterAttribute]
        public ActionResult PlanDetail(string Id_encrypted)
        {
            PlanVM planVM = new PlanVM();

            if (Id_encrypted != null)
            {
                PlanModel planModel = new PlanModel();

                planModel = planBusiness.GetPlanById(Convert.ToInt32(Id_encrypted));
                AutoMapper.Mapper.Map(planModel, planVM);



            }
            return View(planVM);
        }

        [EncryptedActionParameterAttribute]
        public ActionResult PlanModule(string Id_encrypted, string pid_encrypted)
        {
            PlanModuleVM planModuleVM = new PlanModuleVM();
            PlanModuleModel planModuleModel = new PlanModuleModel();
            if (Id_encrypted != null)
            {
                planModuleModel = planBusiness.GetPlanModulePermission(Convert.ToInt32(Id_encrypted));
                AutoMapper.Mapper.Map(planModuleModel, planModuleVM);

            }

            planModuleVM.PlanId = Convert.ToInt32(pid_encrypted).Encrypt();
            planModuleVM.ModuleList = planBusiness.GetAllModules();

            return View(planModuleVM);
        }
        [HttpPost]
        public ActionResult PlanModule(PlanModuleVM planModuleVM)
        {

            if (planBusiness.IsDuplicateModuleInPlan(planModuleVM.PlanId.Decrypt(), planModuleVM.ModuleId, planModuleVM.PlanModuleId.Decrypt()))
            {
                ViewBag.ErrorMessage = "Module already exist in the plan";
                planModuleVM.ModuleList = planBusiness.GetAllModules();
                return View(planModuleVM);

            }
            PlanModuleModel planModuleModel = new PlanModuleModel();



            AutoMapper.Mapper.Map(planModuleVM, planModuleModel);
            bool resultsuccess = planBusiness.AddUpdatePlanModulePermission(planModuleModel, (int)SessionManagement.LoggedInUser.UserId);

            return RedirectToAction("PlanDetail/" + planModuleVM.PlanId, "Admin");

        }

        public ActionResult UpdateCultureDocument(CultureInformationVM cultureVM)
        {
            string FileName = string.Empty;
            DataSet dsObject = new DataSet();
            HttpPostedFileBase file = Request.Files[0];
            CultureInformationModel cultureInfoModel = new CultureInformationModel();
            string fileExtension = Path.GetExtension(file.FileName);
            if (file.ContentLength > 0)
            {
                if ((fileExtension == ".xlsx" || fileExtension == ".xls"))
                {
                    if (!String.IsNullOrEmpty(cultureVM.ExcelFilePath))
                    {
                        if (cultureVM.CultureDescription.ToLower() != Path.GetFileNameWithoutExtension(file.FileName).ToLower())
                        {
                            return Json(new { success = false, response = "You are saving a file which is for different culture." });
                        }
                    }
                    try
                    {
                        string CultureDocPath = ReadConfiguration.CultureExcelFilePath;
                        string path = Path.Combine(Server.MapPath(@"~" + CultureDocPath), cultureVM.CultureDescription + fileExtension);
                        CommonFunctions.UploadFile(file, path);
                        dsObject = CommonFunctions.ImportExceltoDataset(path);
                        string XmlContent = cultureBusiness.ProcessCultureSpecificData(dsObject, cultureVM.CultureName);
                        //Code for saving the xml for a selected culture  to the database
                        cultureVM.LabelsXML = XmlContent;
                        cultureVM.ExcelFilePath = cultureVM.CultureDescription + fileExtension;
                        Mapper.Map(cultureVM, cultureInfoModel);
                        cultureBusiness.EditDocument(cultureInfoModel);
                        if (cultureVM.IsActive == true)
                        {
                            UpdateApplicationVariable(cultureVM);
                        }
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex);
                    }
                }
                else
                {
                    return Json(new { success = false, response = "File is not in correct forrect.Please select a Excel spread sheet." });
                }
            }
            else
            {
                return Json(new { success = false, response = "No file selected.Please select a Excel spread sheet." });
            }
            return Json(new { success = true, response = "Documentment uploaded  successfully" });
        }
        public string GetJson(DataTable dt, ref int tableRowCounter)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<Dictionary<string, object>> dataRows = new List<Dictionary<string, object>>();
            int localTableRowCounter = 0;
            var row = new Dictionary<string, object>();
            List<DataRow> listDataRow = dt.Rows.Cast<DataRow>().ToList();
            //dt.Rows.Cast<DataRow>().ToList().ForEach(dtrow =>
            for (int i = tableRowCounter; i < listDataRow.Count(); i++)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(listDataRow[i][0])) && !string.IsNullOrEmpty(Convert.ToString(listDataRow[i][1])))
                {
                    if (Convert.ToString(listDataRow[i][0]).ToLower() == "labelname" && Convert.ToString(listDataRow[i][1]).ToLower() == "displaylabeltext")
                    {
                        break;
                    }
                    else
                    {
                        row.Add(listDataRow[i][0].ToString(), listDataRow[i][1].ToString());

                    }
                }
                localTableRowCounter++;
            }
            dataRows.Add(row);
            tableRowCounter = localTableRowCounter;
            return ser.Serialize(dataRows);

        }
        [EncryptedActionParameter]
        public FileResult DownloadFile(string id_encrypted)
        {
            CultureInformationModel model = cultureBusiness.GetCultureDetails(Convert.ToInt32(id_encrypted));
            CultureInformationVM vmodel = new CultureInformationVM();
            Mapper.Map(model, vmodel);
            string filePath = string.Empty;
            string returnfileName = string.Empty;
            if (!String.IsNullOrEmpty(vmodel.ExcelFilePath))
            {
                filePath = Server.MapPath(@"~" + ReadConfiguration.CultureExcelFilePath + model.ExcelFilePath);
                returnfileName = vmodel.CultureDescription + Constants.EXCEL_FILE_EXTENSION;
            }
            else
            {
                model = cultureBusiness.GetDefaultCultureDetail();
                Mapper.Map(model, vmodel);
                filePath = Server.MapPath(@"~" + ReadConfiguration.CultureExcelFilePath + vmodel.ExcelFilePath);
                returnfileName = vmodel.CultureDescription + Constants.EXCEL_FILE_EXTENSION;
            }

            if (System.IO.File.Exists(filePath))
            {
                return File(filePath, "application/xlsx", returnfileName);
            }
            else
                return null;
        }

        public List<CultureInformationVM> GetCultureList()
        {
            List<CultureInformationModel> cultureInfoList = new List<CultureInformationModel>();
            List<CultureInformationVM> cultureInfoVMList = new List<CultureInformationVM>();
            cultureInfoList = cultureBusiness.GetUserCultures();
            Mapper.Map(cultureInfoList, cultureInfoVMList);
            return cultureInfoVMList;
        }

        public ActionResult LogOut()
        {
            SessionManagement.LoggedInUser = null;
            Session.Abandon();
            Session.RemoveAll();
            CommonFunctions.RemoveCookies();
            return RedirectToAction("Index", "Site");
        }

        [CustomAuthorize(Roles = Constants.MODULE_CONTENTMANAGEMENT + Constants.PERMISSION_VIEW)]
        public ActionResult ContentManagement()
        {
            return View();
        }

        public JsonResult GetCultures(ListingParameters listingParameters)
        {
            List<CultureInformationModel> cultureInformation = cultureBusiness.GetAllCultures();
            List<CultureInformationVM> cultureInformationVM = new List<CultureInformationVM>();
            Mapper.Map(cultureInformation, cultureInformationVM);
            Response<CultureInformationVM> response = new Response<CultureInformationVM>();
            //    response.TotalRecords = totalRecords;
            response.List = cultureInformationVM;
            return Json(response, JsonRequestBehavior.AllowGet);


        }
        [EncryptedActionParameter]
        public ActionResult ManageLanguage(string id_encrypted)
        {
            CultureInformationVM cultureinfoVM = new CultureInformationVM();
            CultureInformationModel cultureInfoModel = cultureBusiness.GetCultureDetails(Convert.ToInt32(id_encrypted));
            Mapper.Map(cultureInfoModel, cultureinfoVM);
            ViewBag.Language = cultureinfoVM.Language;
            ViewBag.CultureInformationId = cultureinfoVM.CultureInformationId;
            ViewBag.ApplicationPages = GetPublicPages();
            return View(cultureinfoVM);
        }
        [EncryptedActionParameter]
        public ActionResult UpdateDefaultLanguage(string id_encrypted)
        {
            CultureInformationVM cultureinfoVM = new CultureInformationVM();
            cultureBusiness.UpdateDefaultLanguage(Convert.ToInt32(id_encrypted));
            Response response = new Response();
            response.Status = "Success";
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveLanguage(CultureInformationVM cultureInformationVM)
        {
            Response response = new Response();
            response.Status = "Success";
            CultureInformationModel cultureModel = new CultureInformationModel();
            Mapper.Map(cultureInformationVM, cultureModel);
            cultureBusiness.SaveLanguage(cultureModel);
            UpdateApplicationVariable(cultureInformationVM);
            return Json(response);
        }
        [EncryptedActionParameter]
        public ActionResult EditApplicationPage(string id_encrypted, string pageId_encrypted, string returnId = "", string redirectedFrom = "")
        {
            CultureInformationVM cultureinfoVM = new CultureInformationVM();
            CultureInformationModel cultureInfoModel = cultureBusiness.GetCultureDetails(Convert.ToInt32(id_encrypted));
            Mapper.Map(cultureInfoModel, cultureinfoVM);
            if (string.IsNullOrEmpty(cultureinfoVM.ExcelFilePath))
            {
                TempData["LanguageAdded"] = false;
                return RedirectToAction("ManageLanguage", new { id_encrypted = Convert.ToInt32(id_encrypted).Encrypt() });

            }
            else
            {
                ApplicationPageVM pageVM = new ApplicationPageVM();
                ApplicationPageModel pagemodel = new ApplicationPageModel();
                pagemodel = applicationPageBusiness.GetPageDetail(Convert.ToInt32(id_encrypted), Convert.ToInt32(pageId_encrypted));
                Mapper.Map(pagemodel, pageVM);
                ViewBag.Language = cultureinfoVM.Language;
                ViewBag.CultureInformationId = cultureinfoVM.CultureInformationId;
                ViewBag.ApplicationPages = GetPublicPages();
                ViewBag.ReturnPageId = returnId;
                ViewBag.redirectedFrom = redirectedFrom;
                return View(pageVM);
            }
        }

        public ActionResult RemoveCustomPage(ApplicationPageInfo applicationPageInfo)
        {
            Response response = new Response();
            response.Status = "Success";
            int applicationPageId = Convert.ToInt32(applicationPageInfo.ApplicationPageId.Decrypt());
            int customPageId = Convert.ToInt32(applicationPageInfo.CustomPageId.Decrypt());
            if (applicationPageBusiness.RemoveCustomPage(applicationPageId, customPageId))
                response.Message = "true";
            else
                response.Message = "false";

            return Json(response);
            //   return View();
        }
        public ActionResult AddCustomPageToApplicationPage(ApplicationPageInfo applicationPageInfo)
        {
            Response response = new Response();
            response.Status = "Success";
            int applicationPageId = Convert.ToInt32(applicationPageInfo.ApplicationPageId.Decrypt());
            int customPageId = Convert.ToInt32(applicationPageInfo.CustomPageId.Decrypt());
            if (applicationPageBusiness.AddCustomPage(applicationPageId, customPageId))
                response.Message = "true";
            else
                response.Message = "false";

            return Json(response);
        }
        public ActionResult GetCustomPageList(ApplicationPageInfo applicationPageInfo)
        {
            int applicationPageId = applicationPageInfo.ApplicationPageId.Decrypt();
            int cultureInfromationId = applicationPageInfo.CultureInformationId.Decrypt();
            List<AssociatedCutomPagesVM> listAssociatedCutomPagesVM = new List<AssociatedCutomPagesVM>();
            AssociatedCutomPagesVM objAssociatedCutomPagesVM = new AssociatedCutomPagesVM();
            List<ContentApplicationPageModel> listContentApplicationPageModel = applicationPageBusiness.GetCustomPages(applicationPageId, cultureInfromationId, applicationPageInfo.IsViewAll);


            if (applicationPageInfo.IsViewAll)
            {
                for (int i = 0; i < listContentApplicationPageModel.Count; i++)
                {
                    objAssociatedCutomPagesVM = new AssociatedCutomPagesVM();

                    objAssociatedCutomPagesVM.PageTitle = listContentApplicationPageModel[i].ApplicationPage.PageTitle;
                    objAssociatedCutomPagesVM.PageUrl = listContentApplicationPageModel[i].ApplicationPage.PageUrl;

                    if (listContentApplicationPageModel[i].ApplicationPage.AssociationApplicationPages1 != null && listContentApplicationPageModel[i].ApplicationPage.AssociationApplicationPages1.Where(x => x.ApplicationPageId == applicationPageId).Count() > 0)
                    {
                        objAssociatedCutomPagesVM.Action = "Remove From ";
                        objAssociatedCutomPagesVM.RemoveAction = true;
                    }
                    else
                    {
                        objAssociatedCutomPagesVM.Action = "Add To ";
                        objAssociatedCutomPagesVM.RemoveAction = false;
                    }
                    objAssociatedCutomPagesVM.ApplicationPageId = applicationPageId.Encrypt();
                    objAssociatedCutomPagesVM.CustomPageId = listContentApplicationPageModel[i].ApplicationPageId.Encrypt();

                    listAssociatedCutomPagesVM.Add(objAssociatedCutomPagesVM);
                }
            }
            else
            {
                for (int i = 0; i < listContentApplicationPageModel.Count; i++)
                {

                    if (listContentApplicationPageModel[i].ApplicationPage.AssociationApplicationPages1 != null && listContentApplicationPageModel[i].ApplicationPage.AssociationApplicationPages1.Where(x => x.ApplicationPageId == applicationPageId).Count() > 0)
                    {
                        objAssociatedCutomPagesVM = new AssociatedCutomPagesVM();

                        objAssociatedCutomPagesVM.PageTitle = listContentApplicationPageModel[i].ApplicationPage.PageTitle;
                        objAssociatedCutomPagesVM.PageUrl = listContentApplicationPageModel[i].ApplicationPage.PageUrl;

                        objAssociatedCutomPagesVM.Action = "Remove From ";
                        objAssociatedCutomPagesVM.RemoveAction = true;


                        objAssociatedCutomPagesVM.ApplicationPageId = applicationPageId.Encrypt();
                        objAssociatedCutomPagesVM.CustomPageId = listContentApplicationPageModel[i].ApplicationPageId.Encrypt();

                        listAssociatedCutomPagesVM.Add(objAssociatedCutomPagesVM);
                    }
                }



            }


            return Json(new
            {
                CustomPageList = listAssociatedCutomPagesVM
            }
            , JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditApplicationPage(ApplicationPageVM pageVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationPageModel pageModel = new ApplicationPageModel();
                Mapper.Map(pageVm, pageModel);
                applicationPageBusiness.UpdatePageContent(pageModel);
                ApplicationPageModel pagemodel = new ApplicationPageModel();
                TempData["DataUpdated"] = true;
            }
            else
            {
                TempData["DataUpdated"] = false;
            }
            string returnApplicationPageId = Convert.ToString(Request.Form["returnApplicationPageId"]);
            if (!string.IsNullOrEmpty(returnApplicationPageId))
            {
                pageVm.ApplicationPageId = returnApplicationPageId;

            }
            return RedirectToAction("EditApplicationPage", new { id_encrypted = pageVm.ContentApplicationPage.CultureInformationId, pageId_encrypted = pageVm.ApplicationPageId });
        }

        public List<ApplicationPageVM> GetPublicPages()
        {
            List<ApplicationPageVM> publicPageListVM = new List<ApplicationPageVM>();
            List<ApplicationPageModel> publicPageList = new List<ApplicationPageModel>();
            publicPageList = applicationPageBusiness.GetPublicPages();
            Mapper.Map(publicPageList, publicPageListVM);
            return publicPageListVM;
        }
        [EncryptedActionParameter]
        public ActionResult AddCustomPage(string cultureInformationId_encrypted, string applicationPageId_encrypted, string redirectedFrom = "")
        {
            ApplicationPageVM pageVM = new ApplicationPageVM();
            ContentApplicationPageVM contentVM = new ContentApplicationPageVM();
            contentVM.CultureInformationId = Convert.ToInt32(cultureInformationId_encrypted).Encrypt();
            pageVM.ApplicationPageId = Convert.ToInt32(applicationPageId_encrypted).Encrypt();
            pageVM.ContentApplicationPage = contentVM;
            CultureInformationVM cultureinfoVM = new CultureInformationVM();
            CultureInformationModel cultureInfoModel = cultureBusiness.GetCultureDetails(Convert.ToInt32(cultureInformationId_encrypted));
            Mapper.Map(cultureInfoModel, cultureinfoVM);
            ViewBag.Language = cultureinfoVM.Language;
            ViewBag.ApplicationPages = GetPublicPages();
            ViewBag.redirectedFrom = redirectedFrom;
            return View(pageVM);

        }
        [HttpPost]
        public ActionResult AddCustomPage(ApplicationPageVM pageVm)
        {
            ApplicationPageModel pageModel = new ApplicationPageModel();
            if (ModelState.IsValid == true)
            {
                Mapper.Map(pageVm, pageModel);
                applicationPageBusiness.AddCustomPage(pageModel);
                return RedirectToAction("EditApplicationPage", new { id_encrypted = pageVm.ContentApplicationPage.CultureInformationId, pageId_encrypted = pageVm.ApplicationPageId });
            }
            else
            {
                ApplicationPageVM pageVM = new ApplicationPageVM();
                ContentApplicationPageVM contentVM = new ContentApplicationPageVM();
                contentVM.CultureInformationId = pageVm.ContentApplicationPage.CultureInformationId;
                pageVM.ApplicationPageId = pageVm.ApplicationPageId;
                pageVM.ContentApplicationPage = contentVM;
                CultureInformationVM cultureinfoVM = new CultureInformationVM();
                CultureInformationModel cultureInfoModel = cultureBusiness.GetCultureDetails(pageVm.ContentApplicationPage.CultureInformationId.Decrypt());
                Mapper.Map(cultureInfoModel, cultureinfoVM);
                ViewBag.Language = cultureinfoVM.Language;
                ViewBag.ApplicationPages = GetPublicPages();
                return View(pageVm);
            }

        }

        [EncryptedActionParameter]
        public ActionResult DeleteCustomPage(string pageId_encrypted)
        {
            applicationPageBusiness.DeleteCustomPage(Convert.ToInt32(pageId_encrypted));
            return Json(new { result = "true", message = "Record Deleted Successfully." });



        }

        public void UpdateApplicationVariable(CultureInformationVM cultureInfoVM)
        {
            CultureInformationModel cultureModel = new CultureInformationModel();
            cultureModel = cultureBusiness.GetCultureDetails(cultureInfoVM.CultureInformationId.Decrypt());
            ErucaCRM.Utility.CultureInformationManagement.SetCultureObject(cultureModel.CultureName, cultureModel.LabelsXML, cultureModel.IsActive);

        }


        public ActionResult UserList()
        {
            ViewBag.PageSize = ErucaCRM.Utility.ReadConfiguration.PageSize;
            return View();
        }
        public ActionResult GetAdminUserList(UseListInfo userListinfo)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;
            int companyId = (int)SessionManagement.LoggedInUser.CompanyId;
            Boolean userStatus = userListinfo.UserStatus == "Active" ? true : false;
            long totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            int userTypeId = (int)Utility.Enums.UserType.Admin;
            List<UserModel> userListModel = userBusiness.GetAdminUsersByStaus(userTypeId, userListinfo.CurrentPage, userStatus, pageSize, ref totalRecords);

            List<UserVM> listUserVModel = new List<UserVM>();
            Mapper.Map(userListModel, listUserVModel);

            return Json(new
            {
                TotalRecords = totalRecords,
                listUser = listUserVModel
            }
            , JsonRequestBehavior.AllowGet);

        }

        [EncryptedActionParameterAttribute]
        public ActionResult AddUser(string id_encrypted)
        {
            UserVM objUserVModel = new UserVM();
            int UserId = Convert.ToInt32(id_encrypted);
            if (UserId > 0)
            {
                ViewBag.ModuleName = "Edit User";
                UserModel objUserModel = userBusiness.GetUserByUserId(UserId);
                AutoMapper.Mapper.Map(objUserModel, objUserVModel);
            }
            else
            {
                ViewBag.ErrorMessage = "";
                ViewBag.ModuleName = "Create User";
            }
            ViewBag.CountryList = GetCountries();
            return View(objUserVModel);
        }
        [HttpPost]
        public ActionResult AddUser(UserVM userVM)
        {
            ViewBag.ErrorMessage = "";
            if (ModelState["Profile.ProfileName"] != null)
                ModelState["Profile.ProfileName"].Errors.Clear();
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(userVM.UserId))
                {

                    if (userBusiness.IsEmailExist(userVM.EmailId, null))
                    {
                        //ViewBag.ErrorMessage ="" "Email id already exist, please enter different email id";
                        ModelState.AddModelError("EmailId", CommonFunctions.GetGlobalizedLabel("User", "EmailAlreadyExists"));//  "Email already exists, please try another one.");
                        ViewBag.CountryList = GetCountries();

                        return View(userVM);
                    }
                }
                UserModel userModel = new UserModel();
                Mapper.Map(userVM, userModel);
                CompanyModel companymodel = new CompanyModel();
                companymodel.CompanyId = (int)SessionManagement.LoggedInUser.CompanyId;
                userModel.CompanyModel = companymodel;
                userModel = userBusiness.AddUpdateAdminStaffUser(userModel);
                ServiceReference1.Service1Client mailService = new ServiceReference1.Service1Client();
                string to = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;
                to = userModel.EmailId;
                subject = "Login information Eurca CRM";
                body = "<div style='font-face:arial;'><img src='http://erucacrm.sensationsolutions.com/Content//Content/images/logo-dashbord.png'><hr/>Dear Customer" + ",<br/><br/>Please return to the site and log in using the following information" + "<br/>Your Username: " + userModel.EmailId.Trim() + "<br/>Password: " + userModel.Password + " <br/><br/>Thank you.<br/ >Customer Relations</div>";
                try
                {
                    mailService.SendEmail(to, subject, body, true);
                   
                }
                catch (Exception ex)
                {
                  
                }
                return RedirectToAction("UserList", "Admin");
            }
            ViewBag.CountryList = GetCountries();

            return View(userVM);
        }

        public List<CountryVM> GetCountries()
        {
            List<CountryModel> listCountryModel = userBusiness.GetCountries();
            List<CountryVM> listCountryVM = new List<CountryVM>();
            Mapper.Map(listCountryModel, listCountryVM);
            return listCountryVM;
        }
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


        [HttpPost]
        [CustomAuthorize(Roles = Constants.MODULE_USERMANAGEMENT + Constants.PERMISSION_EDIT)]
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


        public ActionResult GetOrganisations(CompanyListInfo companyList)
        {
            short pagesize = 0;
            long totalRecords = 0;
            int AdminCompanyId;
            AdminCompanyId = SessionManagement.LoggedInUser.CompanyId.Value;
            List<CompanyModel> companyModelList = new List<CompanyModel>();
            List<CompanyVM> companyVMList = new List<CompanyVM>();
            pagesize = (short)ErucaCRM.Utility.ReadConfiguration.PageSize;
            Boolean companyStatus = companyList.CompanyStatus == "Active" ? true : false;
            companyModelList = companyBusiness.GetOrgnaisations(companyList.CurrentPage, companyStatus, pagesize, AdminCompanyId, ref totalRecords);


            Mapper.Map(companyModelList, companyVMList);

            return Json(new
            {
                TotalRecords = totalRecords,
                listCompany = companyVMList
            }
            , JsonRequestBehavior.AllowGet);

        }
        public ActionResult Organizations()
        {
            return View();

        }

        public ActionResult OrganizationDetail(string id_encrypted)
        {
            CompanyVM companymodel = new CompanyVM();
            CompanyModel companyModel = new CompanyModel();

            companyModel = companyBusiness.GetCompanyDetail(id_encrypted.Decrypt());
            AutoMapper.Mapper.Map(companyModel, companymodel);
            // companymodel.CreatedBy=userBusiness.getuse

            return View(companymodel);

        }
        [HttpGet]

        public ActionResult GetUserList(UseListInfo userListinfo)
        {
            Response response = new Response();
            response.Status = Enums.ResponseResult.Failure.ToString();
            response.StatusCode = 500;

            Boolean userStatus = userListinfo.UserStatus == "Active" ? true : false;
            long totalRecords = 0;
            Int16 pageSize = Convert.ToInt16(ErucaCRM.Utility.ReadConfiguration.PageSize);
            List<UserModel> userListModel = userBusiness.GetAllUsersByStaus(userListinfo.CompanyId.Decrypt(), userListinfo.CurrentPage, userStatus, pageSize, ref totalRecords);
            List<UserVM> listUserVModel = new List<UserVM>();
            Mapper.Map(userListModel, listUserVModel);

            return Json(new
            {
                TotalRecords = totalRecords,
                listUser = listUserVModel
            }
            , JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateCompanyStatus(List<CompanyListInfo> listUserInfo)
        {
            bool response = false;
            if (listUserInfo.Count > 0)
            {
                int[] CompanyIds = listUserInfo.Select(x => x.CompanyId.Decrypt()).ToArray();
                Boolean currentUserStatus = listUserInfo.FirstOrDefault().CompanyStatus == "Active" ? true : false;
                Boolean newUserStatus = (!currentUserStatus);
                int adminUserID = (int)SessionManagement.LoggedInUser.UserId;
                response = companyBusiness.UpdateCompanyStatus(CompanyIds, newUserStatus, adminUserID);
                return Json(new { status = "Success" }, JsonRequestBehavior.AllowGet);

            }
            return Json(
                        new { status = "Failure" }
                     , JsonRequestBehavior.AllowGet
                     );
        }

    }
}
