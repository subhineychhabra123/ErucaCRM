using ErucaCRM.Business;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Utility;
using ErucaCRM.WCFService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using ErucaCRM.Domain;
using System.Web.Script.Serialization;
using System.IO;
using ErucaCRM.WCFService.Models;
using ErucaCRM.WCFService.RequestModel;
using ErucaCRM.WCFService.ResponseModel;
using ErucaCRM.Utility.WebClasses;
using AutoMapper;
using System.Configuration;
using System.Net.Http;
using System.Web.Hosting;
using ErucaCRM.Utility;
using System.Web;
using ErucaCRM.Web.Models;

namespace ErucaCRM.WCFService
{

    [AutomapServiceBehavior]
    //[TokenValidation]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ErucaCRMService : IErucaCRMService
    {
        //private readonly IUserBusiness userBusiness;
        //public ErucaCRMService(IUserBusiness _userBusiness) {
        //    userBusiness = _userBusiness;
        //}
        IUnitOfWork unitOfWork;
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        private string InvalideParameterMessage = "Invalid request values";
        public ErucaCRMService()
        {
            unitOfWork = new UnitOfWork();
        }
        public ResponseUserLogedInfo ValidateUser(User user)
        {
            var a = user.UserId;
            ResponseUserLogedInfo responseUserInfo = new ResponseUserLogedInfo();
            string token = String.Empty;
            User serviceUser = new User();
            AppVersion appVersionModel = new AppVersion();
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            AppVersionBusiness appVersionBusiness = new AppVersionBusiness(unitOfWork);
            Domain.UserModel userObj = userBusiness.ValidateUser(Convert.ToString(user.EmailId), user.Password, user.IsChecked);
            AppVersionModel appVersion = appVersionBusiness.GetVersion();

            if (userObj != null)
            {
                if (string.IsNullOrEmpty(userObj.tokenId))
                {
                    token = CommonFunctions.GenerateToken() + "_" + userObj.UserId.Encrypt();
                    userObj.tokenId = token;
                    //Code for Saving Token to Database
                }
                else
                {
                    token = userObj.tokenId;
                }
                userBusiness.SaveUserToken(userObj);
                AutoMapper.Mapper.Map(appVersion, appVersionModel);
                AutoMapper.Mapper.Map(userObj, serviceUser);
                serviceUser.UserIdEncrypted = userObj.UserId.Encrypt();
                responseUserInfo.Token = token;
                responseUserInfo.User = serviceUser;
                responseUserInfo.Status = (int)EnumCode.status.Success;
                responseUserInfo.VersionCode = appVersionModel.VersionCode;
                return responseUserInfo;
            }
            else
            {

                string sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Failure, Message = "User Not Found" });
                byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
                responseUserInfo.Status = (int)EnumCode.status.Failure;
                responseUserInfo.Message = "Incorrect username or password!";
                return responseUserInfo;
            }

        }

        public Stream GetLeads(RequestLead listInfo)
        {
            int count = 0;
            List<LeadModel> leadlist = new List<LeadModel>();
            IList<LeadModel> leaddomainlist = new List<LeadModel>();
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            if (listInfo.CurrentPage == null || listInfo.CurrentPage == 0)
                listInfo.CurrentPage = 1;
            //leaddomainlist = leadBusiness.GetLeads(listInfo.CompanyId, listInfo.CurrentPage, listInfo.PageSize, ref count);
            if (leaddomainlist == null)
            {
                string sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Failure, Message = "No Data Found." });
                byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
                return new MemoryStream(byResponse);
            }
            else
            {
                AutoMapper.Mapper.Map(leaddomainlist, leadlist);
                string sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Success, Leads = leadlist });
                byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
                return new MemoryStream(byResponse);
            }
        }

        //public Stream GetDropdownsForlead(User user)
        //{

        //    IList<Users> Users=new List<Users>();
        //    IList<UserModel> Userdomainlist = new List<UserModel>();
        //    UserBusiness leadBusiness = new UserBusiness(unitOfWork);
        //    Userdomainlist = leadBusiness.GetAllUsers((int)user.CompanyId);
        //    AutoMapper.Mapper.Map(Userdomainlist, Users);
        //    return new MemoryStream();
        //}

        public Stream GetUserDetail(User user)
        {
            IList<Users> Users = new List<Users>();
            UserModel Userdomain = new UserModel();
            UserBusiness UserBusiness = new UserBusiness(unitOfWork);
            Userdomain = UserBusiness.GetUserByUserId(user.UserIdEncrypted.Decrypt());
            return new MemoryStream();
        }

        //public Stream AddLead(ListInfo listInfo)
        //{
        //    int count = 0;
        //    Lead leadlist = new Lead();
        //    LeadModel leaddomainlist = new LeadModel();
        //    LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
        //    leadBusiness.AddLead(leaddomainlist);
        //    AutoMapper.Mapper.Map(leaddomainlist, leadlist);

        //    return new MemoryStream();
        //}

        public User RegisterUser(User user)
        {
            UserModel domainUser = new UserModel();
            IUnitOfWork unitOfWork = new UnitOfWork();
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            AutoMapper.Mapper.Map(user, domainUser);
            domainUser.CompanyModel.CompanyName = user.CompanyName;
            // domainUser.CultureInformationModel.CultureInformationId = user.CultureInformationId;
            //  domainUser.TimeZoneModel.TimeZoneId = user.TimeZoneId;
            domainUser.CompanyModel.CompanyName = user.CompanyName;
            var userExist = userBusiness.GetUserByEmailId(user.EmailId);
            if (userExist == null)
            {
                domainUser = userBusiness.RegisterUser(domainUser);
                AutoMapper.Mapper.Map(domainUser, user);
                userBusiness.SendRegistrationEmail(domainUser);
                user.UserIdEncrypted = user.UserId.Encrypt();
                user.Status = (int)EnumCode.status.Success;
            }
            else
            {
                user.Status = (int)EnumCode.status.EmailError;
            }
            return user;
        }
        public Stream ForgetPassword(User user)
        {
            string sResponse = "";
            IUnitOfWork unitOfWork = new UnitOfWork();
            UserModel userModel = new UserModel();
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            userModel = userBusiness.GetUserByEmailId(user.EmailId);
            if (userModel != null)
            {
                bool Result = userBusiness.SendPasswordRecoveryMail(userModel);
                if (Result)
                {
                    sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Success });
                }
                else
                {
                    sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Failure });
                }
            }
            else { sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.EmailError }); }
            byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
            return new MemoryStream(byResponse);
        }

        public Stream GetAutherizedModuleName(Authorization authorization)
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            Dictionary<string, bool> PermissionList = userBusiness.GetAutherizedModuleNameForMobile(authorization.UserId.Decrypt());
            string sResponse = jsSerializer.Serialize(new { status = EnumCode.status.Success, PermissionList = PermissionList });
            byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
            return new MemoryStream(byResponse);
        }

        public Stream GetTimeZones()
        {
            ResponseTimeandCulture ResponseTimeandCulture = new ResponseTimeandCulture();
            List<TimeZone> timeZoneList = new List<TimeZone>();
            List<TimeZoneModal> timeZoneModalList = new List<TimeZoneModal>();
            unitOfWork = new UnitOfWork();
            TimeZoneBusiness timeZoneBusiness = new TimeZoneBusiness(unitOfWork);
            timeZoneModalList = timeZoneBusiness.GetTimeZones();
            AutoMapper.Mapper.Map(timeZoneModalList, timeZoneList);
            ResponseTimeandCulture.TimeZone = timeZoneList;
            List<CultureInformationModel> cultureInformationModalList = new List<CultureInformationModel>();
            List<CultureInformation> cultureInfoList = new List<CultureInformation>();
            unitOfWork = new UnitOfWork();
            CultureInformationBusiness cultureBusiness = new CultureInformationBusiness(unitOfWork);
            cultureInformationModalList = cultureBusiness.GetUserCultures();
            AutoMapper.Mapper.Map(cultureInformationModalList, cultureInfoList);
            ResponseTimeandCulture.CultureInformation = cultureInfoList;

            string sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Success, TimeandCulture = ResponseTimeandCulture });
            byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
            return new MemoryStream(byResponse);
        }


        public List<TimeZone> GetTimeZonesList()
        {

            List<TimeZone> timeZoneList = new List<TimeZone>();
            List<TimeZoneModal> timeZoneModalList = new List<TimeZoneModal>();
            unitOfWork = new UnitOfWork();
            TimeZoneBusiness timeZoneBusiness = new TimeZoneBusiness(unitOfWork);
            timeZoneModalList = timeZoneBusiness.GetTimeZones();
            AutoMapper.Mapper.Map(timeZoneModalList, timeZoneList);

            return timeZoneList;
        }
        public ResponseLead GetLeadsAndStages(RequestLead requestLead)
        {
            throw new NotImplementedException();
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }

            int totalRecords = 0;
            List<LeadStagesJSONModel> leadJsonModelList = new List<LeadStagesJSONModel>();
            List<LeadStagesJSON> leadJsonList = new List<LeadStagesJSON>();
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);

            //leadJsonModelList = leadBusiness.GetLeadsByStageGroup(userId, requestLead.CompanyId, requestLead.CurrentPage, requestLead.PageSize, ref totalRecords);
            //leadBusiness.GetLeadsByStageGroup((
            AutoMapper.Mapper.Map(leadJsonModelList, leadJsonList);
            // responseLeads.LeadLists = leadJsonList;
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;
        }
        public ResponseLead GetLeadsByStageId(RequestLead requestLead)
        {
            int tagId = 0;
            string leadName = string.Empty;
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int stageId = requestLead.StageId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0 || stageId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            int totalRecords = 0;
            List<LeadModel> leadModelList = new List<LeadModel>();
            List<Lead> leadList = new List<Lead>();
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            //leadModelList = leadBusiness.GetLeads(userId, requestLead.CompanyId, stageId, requestLead.CurrentPage, requestLead.PageSize, ref totalRecords);
            leadModelList = leadBusiness.GetLeads(userId, requestLead.CompanyId, stageId, tagId, leadName, requestLead.CurrentPage, requestLead.PageSize, ref totalRecords);
            //GetLeadTasks
            AutoMapper.Mapper.Map(leadModelList, leadList);
            responseLeads.LeadList = leadList;
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;
        }
        public ResponseLead MoveLeadToStage(RequestLead requestLead)
        {
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0 || leadId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            LeadModel leadModel = new LeadModel();
            AutoMapper.Mapper.Map(requestLead, leadModel);
            leadModel.ModifiedBy = requestLead.UserId.Decrypt();
            leadBusiness.UpdateLeadStage(leadModel);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;
        }

        public ResponseStages Stages(RequestStages requestStages)
        {
            ResponseStages responseStages = new ResponseStages();
            if (requestStages.CompanyId <= 0)
            {
                responseStages.Status = (int)EnumCode.status.Failure;
                responseStages.Message = InvalideParameterMessage;
                return responseStages;
            }
            StageBusiness stageBusiness = new StageBusiness(unitOfWork);
            List<Stages> stagesList = new List<Stages>();
            List<StageModel> stageModelList = new List<StageModel>();
            stageModelList = stageBusiness.GetStages(requestStages.CompanyId);
            AutoMapper.Mapper.Map(stageModelList, stagesList);
            responseStages.StageList = stagesList;
            responseStages.Status = (int)EnumCode.status.Success;
            return responseStages;
        }
        ResponseLead IErucaCRMService.AddLead(RequestLead requestLead)
        {
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            LeadModel leadModel = new LeadModel();
            responseLeads.lead = new Lead();
            AutoMapper.Mapper.Map(requestLead, leadModel);
            leadModel.CreatedBy = userId;
            leadModel.LeadOwnerId = userId;
            leadModel = leadBusiness.AddLead(leadModel);
            AutoMapper.Mapper.Map(leadModel, responseLeads.lead);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;
        }
        public ResponseContact GetLeadContacts(RequestLead requestContact)
        {
            ResponseContact responseContacts = new ResponseContact();
            int leadId = requestContact.LeadId.Decrypt();
            int userid = requestContact.UserId.Decrypt();
            if (requestContact.CompanyId <= 0 || leadId <= 0)
            {
                responseContacts.Status = (int)EnumCode.status.Failure;
                responseContacts.Message = InvalideParameterMessage;
                return responseContacts;
            }
            int totalRecords = 0;
            List<ContactModel> contactModelList = new List<ContactModel>();
            List<Contact> contactList = new List<Contact>();
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            contactModelList = contactBusiness.GetContactByLeadId(requestContact.UserId.Decrypt(), requestContact.CompanyId, leadId, requestContact.CurrentPage, requestContact.PageSize, ref totalRecords);
            AutoMapper.Mapper.Map(contactModelList, contactList);
            responseContacts.ContactList = contactList;
            responseContacts.TotalRecords = totalRecords;
            responseContacts.Status = (int)EnumCode.status.Success;
            return responseContacts;
        }
        public ResponseContact GetContacts(RequestContact requestContact)
        {
            ResponseContact responseContacts = new ResponseContact();
            int userId = requestContact.UserId.Decrypt();
            if (requestContact.CompanyId <= 0 || userId <= 0)
            {
                responseContacts.Status = (int)EnumCode.status.Failure;
                responseContacts.Message = InvalideParameterMessage;
                return responseContacts;
            }
            int totalRecords = 0;
            List<ContactModel> contactModelList = new List<ContactModel>();
            List<Contact> contactList = new List<Contact>();
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            short pageSize = (short)requestContact.PageSize;


            if (requestContact.IsSearchByTag == true)
            {
                contactModelList = contactBusiness.GetAllContactsByTagSearch(requestContact.CompanyId, requestContact.CurrentPage, pageSize, userId, requestContact.SearchTags, ref totalRecords);

            }
            else
            {
                contactModelList = contactBusiness.GetAllContacts(requestContact.CompanyId, requestContact.CurrentPage, pageSize, userId, "", 0, requestContact.FilterBy, ref totalRecords);
            }
            AutoMapper.Mapper.Map(contactModelList, contactList);

            responseContacts.ContactList = contactList;
            responseContacts.TotalRecords = totalRecords;
            responseContacts.Status = (int)EnumCode.status.Success;
            return responseContacts;
        }

        public ResponseContact AddContact(RequestContact requestContact)
        {
            ResponseContact responseContacts = new ResponseContact();
            int userId = requestContact.UserId.Decrypt();
            if (requestContact.CompanyId <= 0 || userId <= 0)
            {
                responseContacts.Status = (int)EnumCode.status.Failure;
                responseContacts.Message = InvalideParameterMessage;
                return responseContacts;
            }
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            ContactModel contactModel = new ContactModel();
            foreach (Contact contact in requestContact.Contacts)
            {

                AutoMapper.Mapper.Map(contact, contactModel);
                contactModel.CreatedBy = userId;
                contactModel.OwnerId = userId;
                contactModel.CompanyId = requestContact.CompanyId;
                contactModel.ContactCompanyName = contact.ContactCompanyName;
                contactBusiness.AddContact(contactModel);
            }
            responseContacts.Status = (int)EnumCode.status.Success;
            return responseContacts;
        }
        public ResponseContact UpdateContact(RequestContact requestContact)
        {
            ResponseContact responseContacts = new ResponseContact();
            int userId = requestContact.UserId.Decrypt();
            if (requestContact.CompanyId <= 0 || userId <= 0)
            {
                responseContacts.Status = (int)EnumCode.status.Failure;
                responseContacts.Message = InvalideParameterMessage;
                return responseContacts;
            }
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            ContactModel contactModel = new ContactModel();
            AutoMapper.Mapper.Map(requestContact.Contact, contactModel);
            contactModel.ModifiedBy = userId;
            contactModel.OwnerId = contactModel.OwnerId == 0 ? userId : contactModel.OwnerId;
            contactModel.CompanyId = requestContact.CompanyId;
            contactBusiness.UpdateContact(contactModel);
            responseContacts.Status = (int)EnumCode.status.Success;
            return responseContacts;
        }
        /// <summary>
        /// Deletes Contact from Lead Table and Account Table and sets isDeleted True in Contact Table
        /// </summary>
        /// <param name="requestContact"></param>
        /// <returns></returns>
        /// Manoj Singh
        public ResponseContact DeleteContact(RequestContact requestContact)
        {
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            ResponseContact responseContact = new ResponseContact();

            if (requestContact.LeadId == null)
            {
                if (contactBusiness.DeleteContact(requestContact.ContactId.Decrypt(), requestContact.UserId.Decrypt()))
                {
                    responseContact.Status = (int)EnumCode.status.Success;
                    responseContact.Message = "success";
                }
                else
                {
                    responseContact.Status = (int)EnumCode.status.Failure;
                    responseContact.Message = "failure";
                }
            }
            else
            {
                DeleteLeadContact(requestContact);
            }
            return responseContact;

        }
        /// <summary>
        /// Deletes the contact from lead table  but not from main Contact Table(Used by DeleteContact Method)
        /// </summary>
        /// <param name="requestContact"></param>
        /// <returns></returns>
        /// Manoj Singh
        public ResponseContact DeleteLeadContact(RequestContact requestContact)
        {
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            ResponseContact responseContact = new ResponseContact();

            if (contactBusiness.DeleteLeadContact(requestContact.ContactId.Decrypt(), requestContact.LeadId.Decrypt(), requestContact.UserId.Decrypt()))
            {
                responseContact.Message = "Contact deleted successfully";
                responseContact.Status = (int)EnumCode.status.Success;

            }
            else
            {

                responseContact.Message = "Contact deletion failed";
                responseContact.Status = (int)EnumCode.status.Failure;

            }

            return responseContact;

        }
        public ResponseContact GetContactDetail(RequestContact requestContact)
        {
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            ResponseContact responseContact = new ResponseContact();
            ContactModel contactModel = new ContactModel();
            responseContact.Contact = new Contact();
            contactModel = contactBusiness.GetContactByContactId(requestContact.ContactId.Decrypt());
            AutoMapper.Mapper.Map(contactModel, responseContact.Contact);
            responseContact.Status = (int)EnumCode.status.Success;
            return responseContact;
        }
        public ResponseContact AddLeadContacts(RequestContact requestContact)
        {
            ResponseContact responseContacts = new ResponseContact();
            int userId = requestContact.UserId.Decrypt();
            if (requestContact.CompanyId <= 0 || userId <= 0)
            {
                responseContacts.Status = (int)EnumCode.status.Failure;
                responseContacts.Message = InvalideParameterMessage;
                return responseContacts;
            }
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            List<LeadContactModel> leadContactModelList = new List<LeadContactModel>();
            ContactModel contactModel = new ContactModel();
            AutoMapper.Mapper.Map(requestContact.Contacts, leadContactModelList);
            contactBusiness.AssociateLeadToContact(leadContactModelList, requestContact.UserId.Decrypt());
            responseContacts.Status = (int)EnumCode.status.Success;
            return responseContacts;
        }
        public ResponseLead DeleteLead(RequestLead requestLead)
        {
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0 || leadId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            LeadModel leadModel = new LeadModel();
            AutoMapper.Mapper.Map(requestLead, leadModel);
            leadModel.ModifiedBy = userId;
            leadBusiness.DeleteLead(leadModel);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;
        }
        public ResponseLead GetLeadDetail(RequestLead requestLead)
        {
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || leadId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            return GetLeadById(requestLead);

        }
        private ResponseLead GetLeadById(RequestLead requestLead)
        {

            //StageModel stageModel = new StageModel();
            //StageBusiness stageBusiness = new StageBusiness(unitOfWork);
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            ResponseLead responseLead = new ResponseLead();
            LeadModel leadModel = new LeadModel();
            responseLead.lead = new Lead();
            leadModel = leadBusiness.GetLeadDetail(requestLead.LeadId.Decrypt());
            AutoMapper.Mapper.Map(leadModel, responseLead.lead);

            responseLead.lead.ContactExists = contactBusiness.IsLeadContactExists(requestLead.CompanyId, requestLead.LeadId.Decrypt());

            responseLead.lead.FileName = leadModel.FileName;
            responseLead.lead.FileDuration = leadModel.FileDuration;
            responseLead.lead.DocumentPath = Utility.Constants.LEAD_AUDIO_PATH;
            //stageModel = stageBusiness.GetFinalStage(requestLead.CompanyId);
            //responseLead.lead.FinalStageId = stageModel.StageId.Encrypt();
            responseLead.Status = (int)EnumCode.status.Success;


            return responseLead;

        }
        public ResponseLead GetEditLeadDetail(RequestLead requestLead)
        {
            ResponseLead responseLead = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || leadId <= 0)
            {
                responseLead.Status = (int)EnumCode.status.Failure;
                responseLead.Message = InvalideParameterMessage;
                return responseLead;
            }
            int totalRecords = 0;
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            //  FileAttachmentBusiness fileAttachmentBusiness = new FileAttachmentBusiness(unitOfWork);
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            FileAttachment fileAttachment = new FileAttachment();
            responseLead = GetLeadById(requestLead);
            responseLead.OwnerList = new List<DDL>();
            List<OwnerListModel> OwnerListModel = userBusiness.GetAllOwnerDropdownList(requestLead.CompanyId, true);
            // fileAttachment = fileAttachmentBusiness.GetDocument(requestLead.LeadId);
            AutoMapper.Mapper.Map(OwnerListModel, responseLead.OwnerList);
            //contactBusiness.GetContactByLeadId(requestLead.CompanyId, requestLead.LeadId.Decrypt(), 1, 1, ref totalRecords);

            return responseLead;

        }

        public ResponseLead UpdateLeadRating(RequestLead requestLead)
        {
            ResponseLead responseLead = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0 || leadId <= 0)
            {
                responseLead.Status = (int)EnumCode.status.Failure;
                responseLead.Message = InvalideParameterMessage;
                return responseLead;
            }
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            responseLead.lead = new Lead();
            LeadModel leadModel = new LeadModel();//=  leadBusiness.UpdateLeadRating(requestLead.LeadId.Decrypt(), Convert.ToInt32(requestLead.RatingId));
            AutoMapper.Mapper.Map(leadModel, responseLead.lead);
            responseLead.Status = (int)EnumCode.status.Success;
            return responseLead;
        }

        public ResponseLead UpdateLead(RequestLead requestLead)
        {

            ResponseLead responseLeads = new ResponseLead();

            UserModel userModel = new UserModel();
            LeadModel leadModel = new LeadModel();

            if (requestLead.filedata != null)
            {
                FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
                string uploadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.LeadAudioPath);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                int userId = requestLead.UserId.Decrypt();
                int leadId = requestLead.LeadId.Decrypt();
                int companyId = requestLead.CompanyId;
                // FileAttachmentBusiness fileAttachmentBusiness = new FileAttachmentBusiness(unitOfWork);
                string fileExtension = Path.GetExtension((requestLead.FileName));
                // string uniqueName = Guid.NewGuid().ToShortGuid(6);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((requestLead.FileName));
                string newFileName = "Lead" + "_" + leadId + "_" + DateTime.UtcNow.ToString("MM_dd_yyyy") + "_" + DateTime.UtcNow.ToString("HH_mm_ss") + fileExtension;
                string filePath = Path.Combine(uploadFolder, newFileName);

                try
                {
                    byte[] audioStream = Convert.FromBase64String(requestLead.filedata);
                    FileStream fileToupload = new FileStream(filePath, FileMode.Create);
                    //FileStream fileToupload = new FileStream(@"E:\NewTrunk(3-6)\CODEBASE01\CODE\GIT\trunk\ErucaCRM.Web\Uploads\leads\testingnow.wav", FileMode.Create);
                    fileToupload.Write(audioStream, 0, audioStream.Length);
                    fileToupload.Close();
                    fileToupload.Dispose();
                }
                catch (Exception exception)
                {
                    responseLeads.Message = exception.Message;
                    //newFileName = string.Empty;
                    //requestLead.FileName = string.Empty;
                }
                //    try
                //    {

                //        FileAttachment fileAttachment = new FileAttachment();
                //        fileAttachment.DocumentName = requestLead.FileName;
                //        fileAttachment.DocumentPath = newFileName;
                //        fileAttachment.LeadId = leadId.Encrypt();
                //        fileAttachment.UserId = userId;
                //        fileAttachment.CompanyId = Convert.ToInt32(companyId);
                //        fileAttachment.FileDuration = requestLead.FileDuration;
                //        fileAttachment.DocumentId = requestLead.DocId;

                //        AutoMapper.Mapper.Map(fileAttachment, fileAttachmentModel);
                //        fileAttachmentModel.CreatedDate = DateTime.UtcNow;

                //        if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Delete.ToString())
                //        {
                //            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
                //            responseLeads.Message = "Deleted";
                //        }


                //        if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Added.ToString())
                //        {
                //            fileAttachmentBusiness.AddDocument(fileAttachmentModel);
                //            responseLeads.lead.DocumentId =fileAttachment.DocumentId;
                //            responseLeads.FileName = newFileName;

                //            responseLeads.Message = "Uploaded";
                //        }
                //        if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Updated.ToString())
                //        {
                //            fileAttachmentBusiness.UpdateDocument(fileAttachmentModel);
                //            responseLeads.FileName = newFileName;
                //            responseLeads.lead.DocumentId = fileAttachment.DocumentId;
                //            responseLeads.Message = "Updated";
                //        }
                //        //if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Delete.ToString())
                //        //{
                //        //    fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
                //        //    responseLeads.Message = "Deleted";
                //        //}
                //        //if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.None.ToString())
                //        //{
                //        //    responseLeads.Status = (int)EnumCode.status.Success;
                //        //}
                //            responseLeads.Status = (int)EnumCode.status.Success;


                //    }
                //    catch (Exception exc)
                //    {
                //        responseLeads.Message = exc.Message;
                //    }
            }

            if (requestLead.CompanyId <= 0 || requestLead.UserId.Decrypt() <= 0 || requestLead.LeadId.Decrypt() <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            RatingBusiness ratingBusiness = new RatingBusiness(unitOfWork);
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            RatingModel ratingModel = new RatingModel();

            LeadModel leadModelResponse = new LeadModel();

            responseLeads.lead = new Lead();
            AutoMapper.Mapper.Map(requestLead, leadModel);
            if (requestLead.filedata != null)
            {
                int userId = requestLead.UserId.Decrypt();
                int leadId = requestLead.LeadId.Decrypt();
                int companyId = requestLead.CompanyId;
                string fileExtension = Path.GetExtension((requestLead.FileName));
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((requestLead.FileName));
                string newFileName = "Lead" + "_" + leadId + "_" + DateTime.UtcNow.ToString("MM_dd_yyyy") + "_" + DateTime.UtcNow.ToString("HH_mm_ss") + fileExtension;
                leadModel.FileName = newFileName;
                leadModel.FileDuration = requestLead.FileDuration;
            }
            var ratingConstant = requestLead.RatingId;
            if (ratingConstant != null && ratingConstant > 0)
            {
                ratingModel = ratingBusiness.GetRatingByRatingConstant(requestLead.CompanyId, Convert.ToInt32(ratingConstant));
                if (ratingModel != null)
                {
                    leadModel.RatingId = ratingModel.RatingId;
                }
            }

            leadModel.ModifiedBy = requestLead.UserId.Decrypt();
            if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.None.ToString())
            {
                LeadModel leadModelGet = new LeadModel();
                leadModelGet = leadBusiness.GetLeadDetail(requestLead.LeadId.Decrypt());
                leadModel.FileName = leadModelGet.FileName;
                leadModel.FilePath = leadModelGet.FilePath;
                leadModel.FileDuration = leadModelGet.FileDuration;
                leadModelResponse = leadBusiness.UpdateLead(leadModel);

            }
            if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Added.ToString() || requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Updated.ToString())
                leadModelResponse = leadBusiness.UpdateLead(leadModel);

            if (requestLead.AudioStatus == Utility.Enums.LeadAudioStatus.Delete.ToString())
            {
                leadModel.FileName = "";
                leadModel.FilePath = "";
                leadModel.FileDuration = "";
                leadModelResponse = leadBusiness.UpdateLead(leadModel);
            }
            if (requestLead.IsClosedWin.HasValue && requestLead.IsClosedWin == true)
            {
                StageModel stageModel = new StageModel();
                StageBusiness stageBusiness = new StageBusiness(unitOfWork);
                stageModel = stageBusiness.GetFinalStage(requestLead.CompanyId);
                leadModel.StageId = stageModel.StageId;
                leadModelResponse = leadBusiness.UpdateLeadStage(leadModel);
            }
            AutoMapper.Mapper.Map(leadModelResponse, responseLeads.lead);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;

        }

        public ResponseLead GetLeadHistory(RequestLead requestLead)
        {
            int totalRecords = 0;
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (leadId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            LeadAuditBusiness leadAuditBusiness = new LeadAuditBusiness(unitOfWork);
            List<LeadAuditModel> leadAuditModel = new List<LeadAuditModel>();
            responseLeads.LeadAuditList = new List<LeadAudit>();
            short pageSize = (short)requestLead.PageSize;
            leadAuditModel = leadAuditBusiness.GetLeadHistorybyLeadId(requestLead.LeadId.Decrypt(), requestLead.CurrentPage, pageSize, ref totalRecords);
            AutoMapper.Mapper.Map(leadAuditModel, responseLeads.LeadAuditList);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;
        }

        public ResponseLead GetLeadTasks(RequestLead requestLead)
        {
            int totalRecords = 0;
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || leadId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            IList<TaskItemModel> taskItemModel = new List<TaskItemModel>();
            responseLeads.TaskItemList = new List<TaskItem>();
            int pageSize = requestLead.PageSize;
            taskItemModel = taskItemBusiness.GetLeadTasks(requestLead.CompanyId, requestLead.LeadId.Decrypt(), requestLead.CurrentPage, pageSize, ref totalRecords);

            List<TaskItem> items = new List<TaskItem>();

            AutoMapper.Mapper.Map(taskItemModel, responseLeads.TaskItemList);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;

        }
        public ResponseLead GetTasks(RequestLead requestLead)
        {
            int totalRecords = 0;
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            IList<TaskItemModel> taskItemModel = new List<TaskItemModel>();
            responseLeads.TaskItemList = new List<TaskItem>();
            int pageSize = requestLead.PageSize;
            taskItemModel = taskItemBusiness.GetTasks(userId, requestLead.CompanyId, requestLead.CurrentPage, pageSize, ref totalRecords, "", "");
            AutoMapper.Mapper.Map(taskItemModel, responseLeads.TaskItemList);
            responseLeads.Status = (int)EnumCode.status.Success;
            responseLeads.TotalRecords = totalRecords;
            return responseLeads;

        }
        public ResponseLead GetLeadDocuments(RequestLead requestLead)
        {
            int totalRecords = 0;
            ResponseLead responseLeads = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            int leadId = requestLead.LeadId.Decrypt();
            if (requestLead.CompanyId <= 0 || leadId <= 0)
            {
                responseLeads.Status = (int)EnumCode.status.Failure;
                responseLeads.Message = InvalideParameterMessage;
                return responseLeads;
            }
            FileAttachmentBusiness fileAttachmentBusiness = new FileAttachmentBusiness(unitOfWork);
            IList<FileAttachmentModel> fileAttachmentModel = new List<FileAttachmentModel>();
            responseLeads.AttachmentList = new List<FileAttachment>();
            int pageSize = requestLead.PageSize;
            fileAttachmentModel = fileAttachmentBusiness.GetLeadDocuments(requestLead.CompanyId, requestLead.LeadId.Decrypt(), requestLead.CurrentPage, pageSize, ref totalRecords);
            AutoMapper.Mapper.Map(fileAttachmentModel, responseLeads.AttachmentList);
            responseLeads.Status = (int)EnumCode.status.Success;
            return responseLeads;

        }

        public ResponseContact GetNonAssociatedLeadContactList(RequestLead requestContact)
        {
            int totalRecords = 0;
            ResponseContact responseContacts = new ResponseContact();
            int userId = requestContact.UserId.Decrypt();
            int leadId = requestContact.LeadId.Decrypt();
            if (requestContact.CompanyId <= 0 || userId <= 0 || leadId <= 0)
            {
                responseContacts.Status = (int)EnumCode.status.Failure;
                responseContacts.Message = InvalideParameterMessage;
                return responseContacts;
            }

            List<ContactModel> contactModelList = new List<ContactModel>();
            List<Contact> contactList = new List<Contact>();
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            short pageSize = (short)requestContact.PageSize;
            contactModelList = contactBusiness.NonAssociatedContactList(requestContact.CompanyId, requestContact.CurrentPage, pageSize, userId, "LeadContacts", leadId, ref totalRecords);
            AutoMapper.Mapper.Map(contactModelList, contactList);
            responseContacts.ContactList = contactList;
            responseContacts.TotalRecords = totalRecords;
            responseContacts.Status = (int)EnumCode.status.Success;
            return responseContacts;
        }
        public Stream IsLeadContactExists(RequestLead requestLead)
        {
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
            bool isLeadContactExists = contactBusiness.IsLeadContactExists(requestLead.CompanyId, requestLead.LeadId.Decrypt());
            string sResponse = jsSerializer.Serialize(new { Status = EnumCode.status.Success, ContactExists = isLeadContactExists });
            byte[] byResponse = Encoding.UTF8.GetBytes(sResponse);
            return new MemoryStream(byResponse);
        }
        #region TaskRegion
        public ResponseActivity ActivityDetail(TaskItem taskItem)
        {
            ResponseActivity responceActivity = new ResponseActivity();
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            TaskItemModel taskItemModel = new TaskItemModel();
            TaskItem taskItemDetail = new TaskItem();
            taskItemModel = taskItemBusiness.GetTask(Convert.ToInt32(taskItem.TaskId.Decrypt()));
            AutoMapper.Mapper.Map(taskItemModel, taskItemDetail);
            taskItemDetail.OwnerName = taskItemBusiness.GetTaskOwnerName(taskItemModel.OwnerId);
            taskItemDetail.TaskType = Enum.GetName(typeof(Utility.Enums.Module), taskItemModel.AssociatedModuleId);
            taskItemDetail.TaskAssociatedPerson = taskItemBusiness.GetTaskAssociatedPersonName(taskItemModel.AssociatedModuleId, taskItemModel.AssociatedModuleValue);
            taskItemDetail.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), taskItemModel.PriorityId);
            taskItemDetail.TaskStatus = Enum.GetName(typeof(Utility.Enums.TaskStaus), taskItemModel.Status);
            responceActivity.taskItem = new TaskItem();
            responceActivity.taskItem = taskItemDetail;
            responceActivity.Status = (int)EnumCode.status.Success;
            return responceActivity;

        }

        public ResponseActivity AddActivity(TaskItem taskItem)
        {
            ResponseActivity responceActivity = new ResponseActivity();
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            TaskItemModel taskItemModel = new TaskItemModel();
            //if (taskItem.Audiofiledata != null)
            //{
            //    FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
            //    string uploadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.TaskItemAudioPath);
            //    if (!Directory.Exists(uploadFolder))
            //    {
            //        Directory.CreateDirectory(uploadFolder);
            //    }
            //    int userId = taskItem.UserId.Decrypt();
            //    int taskId = taskItem.TaskId.Decrypt();
            //    int companyId = taskItem.CompanyId;
            //    string fileExtension = Path.GetExtension((taskItem.AudioFileName));
            //    // string uniqueName = Guid.NewGuid().ToShortGuid(6);
            //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((taskItem.AudioFileName));
            //    string newFileName = fileNameWithoutExtension + "_" + taskId + "_" + DateTime.UtcNow.ToString("MM_dd_yyyy") + "_" + DateTime.UtcNow.ToString("HH_mm_ss") + fileExtension;
            //    string filePath = Path.Combine(uploadFolder, newFileName);

            //    try
            //    {
            //        byte[] audioStream = Convert.FromBase64String(taskItem.Audiofiledata);
            //        FileStream fileToupload = new FileStream(filePath, FileMode.Create);
            //        //FileStream fileToupload = new FileStream(@"E:\NewTrunk(3-6)\CODEBASE01\CODE\GIT\trunk\ErucaCRM.Web\Uploads\leads\testingnow.wav", FileMode.Create);
            //        fileToupload.Write(audioStream, 0, audioStream.Length);
            //        fileToupload.Close();
            //        fileToupload.Dispose();
            //    }
            //    catch (Exception exception)
            //    {
            //        responceActivity.Message = exception.Message;
            //    }
            //    taskItem.AudioFileName = newFileName;

            //}

            //if (taskItem.CompanyId <= 0 || taskItem.UserId.Decrypt() <= 0 || taskItem.TaskId.Decrypt() <= 0)
            //{
            //    responceActivity.Status = (int)EnumCode.status.Failure;
            //    responceActivity.Message = InvalideParameterMessage;
            //    return responceActivity;
            //}

            AutoMapper.Mapper.Map(taskItem, taskItemModel);
            taskItemBusiness.AddTask(taskItemModel);
            responceActivity.taskItem = new TaskItem();
            responceActivity.taskItem = null; ;
            responceActivity.Status = (int)EnumCode.status.Success;
            return responceActivity;

        }
        public ResponseActivity GetAddEditActivity(TaskItem taskItem)
        {
            ResponseActivity responseActivity = new ResponseActivity();
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            UserBusiness userBsiness = new UserBusiness(unitOfWork);
            TaskItemModel taskItemModel = new TaskItemModel();
            TaskItem taskItemDetail = new TaskItem();
            if (!string.IsNullOrEmpty(taskItem.TaskId))
            {
                taskItemModel = taskItemBusiness.GetTask(Convert.ToInt32(taskItem.TaskId.Decrypt()));
                AutoMapper.Mapper.Map(taskItemModel, taskItemDetail);
                responseActivity.taskItem = new TaskItem();
                responseActivity.taskItem = taskItemDetail;
            }
            responseActivity.OwnerList = new List<DDL>();
            responseActivity.PriorityList = new List<DDL>();
            responseActivity.StatusList = new List<DDL>();
            List<OwnerListModel> OwnerListModel = userBsiness.GetAllOwnerDropdownList(taskItem.CompanyId, true);
            List<TaskStatusModel> TaskStatusList = taskItemBusiness.GetTaskStatus().ToList();

            Priority priority = null;
            IList<Utility.WebClasses.Priority> priorities = new List<Utility.WebClasses.Priority>();
            Array values = Enum.GetValues(typeof(Utility.Enums.TaskPriority));
            foreach (Utility.Enums.TaskPriority item in values)
            {
                priority = new Priority();
                priority.PriorityId = (int)item;
                priority.PriorityName = Enum.GetName(typeof(Utility.Enums.TaskPriority), item);
                priorities.Add(priority);
            }

            responseActivity.AssociatedModules = new List<DDL>();


            List<ModuleModel> ModuleList = new List<ModuleModel>();


            ModuleModel module = new ModuleModel();
            module.ModuleId = (int)Utility.Enums.Module.Account;
            module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Account);
            ModuleList.Add(module);
            module = new ModuleModel();
            module.ModuleId = (int)Utility.Enums.Module.Lead;
            module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Lead);
            ModuleList.Add(module);
            module = new ModuleModel();
            module.ModuleId = (int)Utility.Enums.Module.Contact;
            module.ModuleName = Enum.GetName(typeof(Utility.Enums.Module), Utility.Enums.Module.Contact);
            ModuleList.Add(module);

            AutoMapper.Mapper.Map(ModuleList, responseActivity.AssociatedModules);
            AutoMapper.Mapper.Map(OwnerListModel, responseActivity.OwnerList);
            AutoMapper.Mapper.Map(TaskStatusList, responseActivity.StatusList);
            AutoMapper.Mapper.Map(priorities, responseActivity.PriorityList);
            responseActivity.Status = (int)EnumCode.status.Success;
            return responseActivity;

        }
        #endregion
        public ResponseActivity GetAssociatedModuleValues(TaskItem taskItem)
        {
            ResponseActivity responseActivity = new ResponseActivity();
            List<AssociatedModuleResponses> response = new List<AssociatedModuleResponses>();
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            responseActivity.TaskAssignedToList = new List<DDLEncrypt>();
            if (taskItem.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Lead))
            {
                AssociatedModuleResponses module;
                List<LeadModel> leads = leadBusiness.GetAllLeadByOwnerId(taskItem.CompanyId, taskItem.UserId.Decrypt()).ToList();
                foreach (var lead in leads)
                {
                    module = new AssociatedModuleResponses();
                    module.Id = lead.LeadId;
                    module.value = lead.Title;
                    response.Add(module);
                }
            }
            else if (taskItem.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Contact))
            {
                ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);
                AssociatedModuleResponses module;
                List<ContactModel> contacts = contactBusiness.GetContactsByOwnerIdAndCompanyID(taskItem.CompanyId, taskItem.UserId.Decrypt()).ToList();
                foreach (var contact in contacts)
                {
                    module = new AssociatedModuleResponses();
                    module.Id = contact.ContactId;
                    module.value = contact.FirstName + " " + contact.LastName;
                    response.Add(module);
                }
            }
            else if (taskItem.AssociatedModuleId == Convert.ToInt32(Utility.Enums.Module.Account))
            {
                AccountBusiness accountBussiness = new AccountBusiness(unitOfWork);
                AssociatedModuleResponses module;
                List<AccountModel> accounts = accountBussiness.GetAccountsByOwnerIdAndCompanyID(taskItem.CompanyId, taskItem.UserId.Decrypt()).ToList();
                foreach (var account in accounts)
                {
                    module = new AssociatedModuleResponses();
                    module.Id = account.AccountId;
                    module.value = account.AccountName;
                    response.Add(module);
                }
            }
            responseActivity.Status = (int)EnumCode.status.Success;
            AutoMapper.Mapper.Map(response, responseActivity.TaskAssignedToList);
            return responseActivity;
        }

        public ResponseActivity AddEditActivity(TaskItem requestActivity)
        {
            int userId = requestActivity.UserId.Decrypt();
            int taskId = requestActivity.TaskId.Decrypt();
            int companyId = requestActivity.CompanyId;

            TaskItemBusiness taskBusiness = new TaskItemBusiness(unitOfWork);
            ResponseActivity responceActivity = new ResponseActivity();
            TaskItemModel taskItemModel = new TaskItemModel();
            TaskItemModel taskModel = taskBusiness.GetTask(taskId);
                
            if (requestActivity.AudioStatus == Utility.Enums.LeadAudioStatus.None.ToString())
            {
                //AutoMapper.Mapper.Map(requestActivity, taskItemModel);
                requestActivity.AudioFileName = taskModel.AudioFileName;
                requestActivity.AudioFileDuration = taskModel.AudioFileDuration;
                //taskModel = taskBusiness.AddTask(taskItemModel);
            }
            else if (requestActivity.AudioStatus == Utility.Enums.LeadAudioStatus.Added.ToString() || requestActivity.AudioStatus == Utility.Enums.LeadAudioStatus.Updated.ToString())
            {
                //taskItemModel.CreatedDate = DateTime.UtcNow;
                if (requestActivity.Audiofiledata != null)
                {
                    string uploadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.TaskItemAudioPath);
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    string fileExtension = Path.GetExtension((requestActivity.AudioFileName));
                    // string uniqueName = Guid.NewGuid().ToShortGuid(6);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((requestActivity.AudioFileName));
                    string newFileName = "Task_" + "_" + taskId + "_" + DateTime.UtcNow.ToString("MM_dd_yyyy") + "_" + DateTime.UtcNow.ToString("HH_mm_ss") + fileExtension;
                    string filePath = Path.Combine(uploadFolder, newFileName);

                    try
                    {
                        byte[] audioStream = Convert.FromBase64String(requestActivity.Audiofiledata);
                        FileStream fileToupload = new FileStream(filePath, FileMode.Create);
                        //FileStream fileToupload = new FileStream(@"E:\NewTrunk(3-6)\CODEBASE01\CODE\GIT\trunk\ErucaCRM.Web\Uploads\leads\testingnow.wav", FileMode.Create);
                        fileToupload.Write(audioStream, 0, audioStream.Length);
                        fileToupload.Close();
                        fileToupload.Dispose();
                    }
                    catch (Exception exception)
                    {
                        responceActivity.Message = exception.Message;
                        responceActivity.Status = (int)Enums.ResponseResult.Failure;
                        return responceActivity;
                    }
                    requestActivity.AudioFileName = newFileName;

                    responceActivity.taskItem = new TaskItem();
                    /*******************************************************/
                    requestActivity.AudioFileName = newFileName;
                    /*******************************************************/
                    responceActivity.taskItem.AudioFileName = newFileName;
                    responceActivity.taskItem.AudioFileDuration = requestActivity.AudioFileDuration;
                }
                //taskModel = taskBusiness.AddTask(taskItemModel);
            }
            else if (requestActivity.AudioStatus == Utility.Enums.LeadAudioStatus.Delete.ToString())
            {
                requestActivity.AudioFileName = string.Empty;
                requestActivity.AudioFileDuration = string.Empty;
                //AutoMapper.Mapper.Map(requestActivity, taskItemModel);
                //taskModel = taskBusiness.AddTask(taskItemModel);
            }

            AutoMapper.Mapper.Map(requestActivity, taskItemModel);

            DateTime dateResult = new DateTime();
            DateTime.TryParse(requestActivity.DueDate, out dateResult);

            taskItemModel.DueDate = dateResult;
            taskItemModel.ModifiedBy = userId;
            taskItemModel.ModifiedDate = DateTime.UtcNow;

            if (taskItemModel.TaskId == 0)
            {
                taskItemModel.CreatedBy = userId;
                taskItemModel.CreatedDate = DateTime.UtcNow;

                /*******************************************************/
                taskItemModel.EndDate = DateTime.UtcNow;
                /*******************************************************/

                //AutoMapper.Mapper.Map(taskModel, taskItemModel);
                //if (requestActivity.Audiofiledata != null)
                //{
                //    string fileExtension = Path.GetExtension((requestActivity.AudioFileName));
                //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((requestActivity.AudioFileName));
                //    string newFileName = "Task_" + "_" + taskId + "_" + DateTime.UtcNow.ToString("MM_dd_yyyy") + "_" + DateTime.UtcNow.ToString("HH_mm_ss") + fileExtension;
                //    taskItemModel.AudioFileName = newFileName;
                //    taskItemModel.AudioFileDuration = taskModel.AudioFileDuration;
                //}

            }
            else
            {
                taskItemModel.CreatedDate = taskModel.CreatedDate;
                taskItemModel.CreatedBy = taskModel.CreatedBy;
            }
            taskBusiness.AddTask(taskItemModel);
            responceActivity.Status = (int)EnumCode.status.Success;
            //responceActivity.taskItem.AudioFileName = taskModel.AudioFileName;
            //responceActivity.taskItem.AudioFileDuration = taskModel.AudioFileDuration;
            return responceActivity;
        }

        //public ResponseStatus UploadFiles(Stream FileData)
        //{
        //    //int LeadId = fileContent.LeadId.Decrypt();
        //    FileStream targetStream = null;
        //    // Stream sourceStream = fileContents;
        //    ResponseStatus response = new ResponseStatus();
        //    FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

        //    ErucaCRM.Utility.SampleService.MultipartParser parser = new Utility.SampleService.MultipartParser(FileData);
        //    // Stream sourceStream = parser.FileContents;
        //    string uploadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.LeadDocumentPath);
        //    if (!Directory.Exists(uploadFolder))
        //    {
        //        Directory.CreateDirectory(uploadFolder);
        //    }
        //    string userId = GetValueByKey(parser, "UserId");
        //    string leadId = GetValueByKey(parser, "LeadId");
        //    string companyId = GetValueByKey(parser, "CompanyId");
        //    FileAttachmentBusiness fileAttachmentBusiness = new FileAttachmentBusiness(unitOfWork);
        //    string fileExtension = Path.GetExtension((parser.Filename));
        //    string uniqueName = Guid.NewGuid().ToShortGuid(6);
        //    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((parser.Filename));
        //    string newFileName = fileNameWithoutExtension + "_" + leadId.Decrypt() + "_" + uniqueName + fileExtension;
        //    string filePath = Path.Combine(uploadFolder, newFileName);

        //    try
        //    {
        //        if (parser.Success)
        //        {
        //            //string fileName = parser.Filename;
        //            string contentType = parser.ContentType;
        //            byte[] fileContentData = parser.FileContents;
        //            FileStream fileToupload = new FileStream(filePath, FileMode.Create);
        //            fileToupload.Write(fileContentData, 0, fileContentData.Length);
        //            fileToupload.Close();
        //            fileToupload.Dispose();
        //            FileData.Close();

        //        }
        //        FileAttachment fileAttachment = new FileAttachment();
        //        fileAttachment.DocumentName = parser.Filename;
        //        fileAttachment.DocumentPath = newFileName;
        //        fileAttachment.LeadId = leadId;
        //        fileAttachment.UserId = userId.Decrypt();
        //        fileAttachment.CompanyId = Convert.ToInt32(companyId);
        //        AutoMapper.Mapper.Map(fileAttachment, fileAttachmentModel);
        //        fileAttachmentBusiness.AddDocument(fileAttachmentModel);
        //        fileAttachmentModel.CreatedDate = DateTime.UtcNow;
        //        response.Status = (int)EnumCode.status.Success;
        //        response.Message = "Uploaded";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = (int)EnumCode.status.Failure;
        //        response.Message = ex.Message;

        //    }
        //    return response;
        //}

        ///// <summary>
        ///// Not in Use
        ///// </summary>
        ///// <param name="parser"></param>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public ResponseStatus SaveLead(Stream FileData)//Not in Use
        //{

        //    //new MemoryStream(byteArray);
        //    //int LeadId = fileContent.LeadId.Decrypt();
        //    FileStream targetStream = null;
        //    // Stream sourceStream = fileContents;
        //    ResponseStatus response = new ResponseStatus();
        //    FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

        //    ErucaCRM.Utility.SampleService.MultipartParser parser = new Utility.SampleService.MultipartParser(FileData);
        //    // Stream sourceStream = parser.FileContents;
        //    string uploadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.LeadDocumentPath);
        //    if (!Directory.Exists(uploadFolder))
        //    {
        //        Directory.CreateDirectory(uploadFolder);
        //    }
        //    string userId = GetValueByKey(parser, "UserId");
        //    string leadId = GetValueByKey(parser, "LeadId");
        //    string companyId = GetValueByKey(parser, "CompanyId");
        //    ///for Saving lead data
        //    string stageId = GetValueByKey(parser, "StageId");
        //    string isWinStage = GetValueByKey(parser, "IsWinStage");//?
        //    string title = GetValueByKey(parser, "Title");
        //    string leadCompanyName = GetValueByKey(parser, "LeadCompanyName");
        //    string isClosedWin = GetValueByKey(parser, "IsClosedWin");//?
        //    string modifiedBy = GetValueByKey(parser, "ModifiedBy");
        //    string ratingId = GetValueByKey(parser, "RatingId");//?
        //    string amount = GetValueByKey(parser, "Amount");//Decimal?
        //    string leadOwnerId = GetValueByKey(parser, "LeadOwnerId");//int?
        //    string description = GetValueByKey(parser, "Description");
        //    string docId = GetValueByKey(parser, "DocId");//int
        //    //start
        //    ResponseLead responseLeads = new ResponseLead();
        //    //int userId = requestLead.UserId.Decrypt();
        //    //int leadId = requestLead.LeadId.Decrypt();
        //    if (companyId.Decrypt() <= 0 || userId.Decrypt() <= 0 || leadId.Decrypt() <= 0)
        //    {
        //        responseLeads.Status = (int)EnumCode.status.Failure;
        //        responseLeads.Message = InvalideParameterMessage;
        //        return responseLeads;
        //    }
        //    RatingBusiness ratingBusiness = new RatingBusiness(unitOfWork);
        //    LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
        //    RatingModel ratingModel = new RatingModel();
        //    LeadModel leadModel = new LeadModel();
        //    LeadModel leadModelResponse = new LeadModel();
        //    responseLeads.lead = new Lead();
        //    // AutoMapper.Mapper.Map(requestLead, leadModel);
        //    responseLeads.lead = new Lead();
        //    leadModel.UserModel.UserId = userId.Decrypt();
        //    leadModel.LeadId = leadId.Decrypt();
        //    leadModel.StageId = stageId.Decrypt();
        //    leadModel.CompanyId = companyId.Decrypt();
        //    leadModel.IsClosedWin = Convert.ToBoolean(isClosedWin);
        //    leadModel.Title = title;
        //    leadModel.LeadCompanyName = leadCompanyName;
        //    leadModel.ModifiedBy = modifiedBy.Decrypt();
        //    leadModel.RatingId = ratingId.Decrypt();
        //    leadModel.Amount = Convert.ToDecimal(amount);
        //    leadModel.LeadOwnerId = leadOwnerId.Decrypt();
        //    leadModel.Description = description;

        //    bool? IsClosedWin = Convert.ToBoolean(isClosedWin);

        //    if (ratingId != null && ratingId.Decrypt() > 0)
        //    {
        //        ratingModel = ratingBusiness.GetRatingByRatingConstant(companyId.Decrypt(), Convert.ToInt32(ratingId));
        //        if (ratingModel != null)
        //        {
        //            leadModel.RatingId = ratingModel.RatingId;
        //        }
        //    }
        //    leadModel.ModifiedBy = userId.Decrypt();
        //    leadModelResponse = leadBusiness.UpdateLead(leadModel);
        //    try
        //    {

        //        if (IsClosedWin.HasValue && IsClosedWin == true)
        //        {
        //            StageModel stageModel = new StageModel();
        //            StageBusiness stageBusiness = new StageBusiness(unitOfWork);
        //            stageModel = stageBusiness.GetFinalStage(companyId.Decrypt());
        //            leadModel.StageId = stageModel.StageId;
        //            leadModelResponse = leadBusiness.UpdateLeadStage(leadModel);
        //        }
        //        AutoMapper.Mapper.Map(leadModelResponse, responseLeads.lead);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Status = (int)EnumCode.status.Failure;
        //        response.Message = ex.Message;

        //    }
        //    // responseLeads.Status = (int)EnumCode.status.Success;
        //    //end
        //    UploadFiles(FileData);

        //    return response;
        //}



        private string GetValueByKey(ErucaCRM.Utility.SampleService.MultipartParser parser, string key)
        {
            string Keyvalue = string.Empty;
            string keyToFind = string.Empty;
            string remove = "/";
            foreach (var content in parser.MyContents)
            {
                keyToFind = content.PropertyName;
                keyToFind = keyToFind.Replace('"', ' ').Trim();


                if (keyToFind == key)
                {
                    Keyvalue = content.StringData.Trim();
                    break;
                }
            }
            return Keyvalue;
        }
        public Stream DownloadLeadFile(RequestLead requestLead)
        {
            string downloadFilePath = string.Empty;
            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
            FileAttachmentBusiness fileAttachmentBusiness = new FileAttachmentBusiness(unitOfWork);
            fileAttachmentModel.DocumentId = requestLead.DocId;
            fileAttachmentModel = fileAttachmentBusiness.GetDocument(fileAttachmentModel);
            if (fileAttachmentModel != null)
            {
                string downloadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.LeadDocumentPath);
                if (!Directory.Exists(downloadFolder))
                {
                    Directory.CreateDirectory(downloadFolder);
                }
                downloadFilePath = Path.Combine(downloadFolder, fileAttachmentModel.DocumentPath);
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
            }
            FileStream s = new FileStream(downloadFilePath, FileMode.Open, FileAccess.Read);

            return s;

            //WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";
            //FileStream f = new FileStream(downloadFilePath, FileMode.Open);
            //int length = (int)f.Length;
            //WebOperationContext.Current.OutgoingResponse.ContentLength = length;
            //byte[] buffer = new byte[length];
            //int sum = 0;
            //int count;
            //while ((count = f.Read(buffer, sum, length - sum)) > 0)
            //{
            //    sum += count;
            //}
            //f.Close();
            //return new MemoryStream(buffer); 
        }




        public ResponseActivity DeleteActivity(TaskItem requestActivity)
        {
            int UserId = requestActivity.UserId.Decrypt();
            int TaskId = requestActivity.TaskId.Decrypt();
            TaskItemBusiness taskBusiness = new TaskItemBusiness(unitOfWork);
            TaskItemModel taskModel = new TaskItemModel();
            taskBusiness.DeleteTaskItem(TaskId, UserId);
            ResponseActivity response = new ResponseActivity();
            response.Status = (int)EnumCode.status.Success;
            return response;
        }


        public ResponseLead GetAccounts(RequestLead requestLead)
        {
            int totalRecords = 0;
            int? tagId = null;
            string tagName = string.Empty;
            ResponseLead responseLead = new ResponseLead();
            int userId = requestLead.UserId.Decrypt();
            if (requestLead.CompanyId <= 0 || userId <= 0)
            {
                responseLead.Status = (int)EnumCode.status.Failure;
                responseLead.Message = InvalideParameterMessage;
                return responseLead;
            }

            AccountBusiness accountBussiness = new AccountBusiness(unitOfWork);
            List<AccountModel> accountModelList = new List<AccountModel>();
            accountModelList = accountBussiness.GetAccountsByUserId(requestLead.CompanyId, userId, tagId, tagName, requestLead.CurrentPage, requestLead.PageSize, ref totalRecords);

            responseLead.AccountList = new List<Account>();
            AutoMapper.Mapper.Map(accountModelList, responseLead.AccountList);
            responseLead.TotalRecords = totalRecords;
            responseLead.Status = (int)EnumCode.status.Success;
            return responseLead;
        }
        /// <summary>
        /// For retriving userDetail
        /// </summary>
        /// <returns>Details of user</returns>
        public ResponseUser UserProfile(UserProfile user)
        {
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            ResponseUser profile = new ResponseUser();
            UserModel userModel = new UserModel();
            userModel = userBusiness.GetUserByUserId(user.UserId.Decrypt());
            profile.User = new RequestModel.UserProfile();
            Mapper.Map(userModel, profile.User);
            profile.Status = (int)EnumCode.status.Success;
            return profile;
        }

        public ResponseUser EditUserProfile(UserProfile user)
        {
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            ResponseUser responseProfile = new ResponseUser();
            responseProfile.User = new UserProfile();
            UserModel userModel = new UserModel();
            userModel = userBusiness.GetUserByUserId(user.UserId.Decrypt());
            Mapper.Map(userModel, responseProfile.User);
            List<CultureInformation> cultureInformationList = new List<CultureInformation>();
            cultureInformationList = GetCultureList();
            List<TimeZone> TimeZone = new List<TimeZone>();
            TimeZone = GetTimeZonesList();
            responseProfile.TimeZoneList = new List<DDL>();
            responseProfile.CultureInformationList = new List<DDL>();
            responseProfile.CountryList = new List<DDL>();
            List<CountryModel> listCountryModel = userBusiness.GetCountries();
            Mapper.Map(listCountryModel, responseProfile.CountryList);
            Mapper.Map(TimeZone, responseProfile.TimeZoneList);
            Mapper.Map(cultureInformationList, responseProfile.CultureInformationList);

            return responseProfile;
        }

        public ResponseUser UpdateUserProfile(UserProfile user)
        {
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            ResponseUser responseProfile = new ResponseUser();
            UserModel userModel = new UserModel();
            userModel = userBusiness.GetUserByUserId(user.UserId.Decrypt());
            userModel.FirstName = user.FirstName;
            userModel.LastName = user.LastName;
            userModel.EmailId = user.EmailId;
            userModel.TimeZoneId = user.TimeZoneId;
            userModel.CultureInformationId = user.CultureInformationId;
            userModel.AddressModel.CountryId = user.CountryId;
            userModel.AddressModel.Street = user.Street;
            userModel.AddressModel.Zipcode = user.ZipCode;
            userModel.AddressModel.CountryModel.CountryId = user.CountryId;
            userModel.AddressModel.City = user.City;
            userModel = userBusiness.UpdateUser(userModel);
            responseProfile.Status = (int)EnumCode.status.Success;
            return responseProfile;
        }
        public List<CultureInformation> GetCultureList()
        {
            CultureInformationBusiness cultureBusiness = new CultureInformationBusiness(unitOfWork);
            List<CultureInformationModel> cultureInfoModelList = new List<CultureInformationModel>();
            List<CultureInformation> cultureInfoList = new List<CultureInformation>();
            cultureInfoModelList = cultureBusiness.GetUserCultures();
            Mapper.Map(cultureInfoModelList, cultureInfoList);
            return cultureInfoList;
        }



        public ResponseStatus RemoveLeadDocument(RequestLead requestLead)
        {
            FileAttachmentBusiness fileAttachmentBusiness = new FileAttachmentBusiness(unitOfWork);
            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
            fileAttachmentModel.DocumentId = requestLead.DocId;
            fileAttachmentBusiness.RemoveDocument(fileAttachmentModel);
            ResponseStatus response = new ResponseStatus();
            response.Status = (int)EnumCode.status.Success;
            response.Message = "Document removed successfully.";
            return response;
        }
        /// <summary>
        /// To get recent Atitvity
        /// ProcedureName - SSP_GetLeadAuditsForHomeRecentActivites
        /// </summary>
        /// <returns>Recents Activities</returns>
        /// by- Manoj Singh
        public ResponseActivity GetRecentActivities(RequestTaskItem requestTaskItem)
        {
            int TotalRecords = requestTaskItem.TotalRecords;
            int MaxLeadAuditId = requestTaskItem.MaxLeadAuditId;
            TotalRecords = 0;
            ResponseActivity responseactivity = new ResponseActivity();
            HomeBusiness homeBusiness = new HomeBusiness(unitOfWork);
            //List<HomeVM> homeVMList = new List<HomeVM>();
            List<HomeModel> homeModelList = new List<HomeModel>();
            // List<HomeModel> homeNotification = new List<HomeModel>();
            int pageSize = Convert.ToInt32(requestTaskItem.PageSize);
            int leadAuditId = Convert.ToInt32(requestTaskItem.LeadAuditId.Decrypt());
            homeModelList = homeBusiness.GetRecentActivitiesForHome(pageSize, requestTaskItem.CurrentPage, leadAuditId, requestTaskItem.IsLoadMore, requestTaskItem.CompanyId, requestTaskItem.UserId.Decrypt());
            int homeNotification = homeBusiness.GetNotification(pageSize, requestTaskItem.CompanyId, requestTaskItem.UserId.Decrypt(), ref MaxLeadAuditId, requestTaskItem.UpdateNotification, ref TotalRecords);
            responseactivity.TotalNotificationRecords = TotalRecords;
            responseactivity.MaxLeadAuditId = MaxLeadAuditId;
            responseactivity.RecentActivities = new List<HomeRecentActivites>();
            responseactivity.GetNotification = new List<Notification>();
            Mapper.Map(homeModelList, responseactivity.RecentActivities);
            // Mapper.Map(homeModelList, responseactivity.GetNotification);
            responseactivity.Status = (int)EnumCode.status.Success;
            return responseactivity;
        }
        /// <summary>
        /// For retriving data to the dashboard (chart)
        /// </summary>
        /// <returns>chart with New Clients,Closed Leads,Revenue,Win Leads and Closed Leads</returns>
        ///by- Manoj Singh
        public ResponseActivity GetDashboardData(RequestDashboardData requestDashboardData)
        {
            int totalRecords = 0;
            ResponseActivity responseactivity = new ResponseActivity();
            HomeBusiness homeBusiness = new HomeBusiness(unitOfWork);
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            List<DashboarActivitiesModel> taskItemList = new List<DashboarActivitiesModel>();
            List<HomeModel> homeModelList = new List<HomeModel>();
            //homeModelList = homeBusiness.GetDashboardData(requestDashboardData.CompanyId, requestDashboardData.Interval);
            HomeModel homeModel = new HomeModel();
            homeModel = homeBusiness.GetDashboardData(requestDashboardData.CompanyId, requestDashboardData.Interval);
            homeModelList.Add(homeModel);
            taskItemList = taskItemBusiness.GetTasks(requestDashboardData.UserId.Decrypt(), requestDashboardData.CompanyId, requestDashboardData.CurrentPage, requestDashboardData.PageSize, "DueDate", ref totalRecords);
            responseactivity.DashboardData = new List<DashboardData>();
            responseactivity.Activities = new List<TaskItem>();
            Mapper.Map(homeModelList, responseactivity.DashboardData);
            Mapper.Map(taskItemList, responseactivity.Activities);


            responseactivity.Status = (int)EnumCode.status.Success;
            responseactivity.APICulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            return responseactivity;
        }
        /// <summary>
        /// For retriving DashboardData ,GetRecentActivity,GetTasks,GetLeadsByStageGroupformobile,GetUserByUserId,APICulture(for datetime format where server is based)
        /// </summary>
        /// <returns>Dashboard ,GetRecentActivity,GetTasks,top 10 leads of all stages,top 10 contacts of every lead,top 10 tasks of all leads,top 10 documents of all leads,</returns>
        ///by- Manoj Singh

        public ResponseActivity GetLeadsByStageGroup(AllUserDetail UserAllDetails)
        {
            int TotalRecords = UserAllDetails.TotalRecords;
            int MaxLeadAuditId = UserAllDetails.MaxLeadAuditId;
            TotalRecords = 0;
            string Interval = "Week";
            int PageSize = 10;
            bool IsLoadMore = false;
            int CurrentPage = 1;
            string FilterBy = "Allcontacts";
            ResponseActivity responseactivity = new ResponseActivity();
            HomeBusiness homeBusiness = new HomeBusiness(unitOfWork);
            TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
            LeadBusiness leadBusiness = new LeadBusiness(unitOfWork);
            UserBusiness userBusiness = new UserBusiness(unitOfWork);
            ContactBusiness contactBusiness = new ContactBusiness(unitOfWork);

            List<DashboarActivitiesModel> taskItemList = new List<DashboarActivitiesModel>();
            List<HomeModel> homeModelListGetDashboardData = new List<HomeModel>();
            List<HomeModel> homeModelListGetRecentActivity = new List<HomeModel>();
            List<LeadStagesJSONModel> leadJsonModelList = new List<LeadStagesJSONModel>();
            IList<TaskItemModel> taskItemModel = new List<TaskItemModel>();
            List<ContactModel> contactModelList = new List<ContactModel>();
            HomeModel homeModelGetDashboardData = new HomeModel();
            UserModel userModel = new UserModel();

            homeModelGetDashboardData = homeBusiness.GetDashboardData(UserAllDetails.CompanyId, Interval);
            homeModelListGetDashboardData.Add(homeModelGetDashboardData);

            int leadAuditId = Convert.ToInt32(UserAllDetails.LeadAuditId.Decrypt());

            homeModelListGetRecentActivity = homeBusiness.GetRecentActivitiesForHome(PageSize, CurrentPage, leadAuditId, IsLoadMore, UserAllDetails.CompanyId, UserAllDetails.UserId.Decrypt());

            taskItemList = taskItemBusiness.GetTasks(UserAllDetails.UserId.Decrypt(), UserAllDetails.CompanyId, CurrentPage, PageSize, "DueDate", ref TotalRecords);

            leadJsonModelList = leadBusiness.GetLeadsByStageGroupformobile(UserAllDetails.UserId.Decrypt(), UserAllDetails.CompanyId, CurrentPage, PageSize, ref TotalRecords).ToList();


            userModel = userBusiness.GetUserByUserId(UserAllDetails.UserId.Decrypt());


            responseactivity.User = new RequestModel.UserProfile();

            taskItemModel = taskItemBusiness.GetTasks(UserAllDetails.UserId.Decrypt(), UserAllDetails.CompanyId, CurrentPage, PageSize, ref TotalRecords, "", "");

            contactModelList = contactBusiness.GetAllContacts(UserAllDetails.CompanyId, CurrentPage, 10, UserAllDetails.UserId.Decrypt(), "", 0, FilterBy, ref TotalRecords);


            responseactivity.RecentActivities = new List<HomeRecentActivites>();
            responseactivity.DashboardData = new List<DashboardData>();
            responseactivity.Activities = new List<TaskItem>();
            responseactivity.GetLeadsByStageGroupformobile = new List<LeadStagesJSON>();
            responseactivity.AllTaskItem = new List<TaskItem>();
            responseactivity.AllContactList = new List<Contact>();
            Mapper.Map(homeModelListGetDashboardData, responseactivity.DashboardData);
            Mapper.Map(homeModelListGetRecentActivity, responseactivity.RecentActivities);
            Mapper.Map(userModel, responseactivity.User);
            Mapper.Map(taskItemList, responseactivity.Activities);
            Mapper.Map(leadJsonModelList, responseactivity.GetLeadsByStageGroupformobile);
            Mapper.Map(taskItemModel, responseactivity.AllTaskItem);
            Mapper.Map(contactModelList, responseactivity.AllContactList);

            // responseactivity.PermissionList = userBusiness.GetAutherizedModuleNameForMobile(UserAllDetails.UserId.Decrypt());

            responseactivity.APICulture = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            responseactivity.Status = (int)EnumCode.status.Success;
            return responseactivity;
        }

        /// <summary>
        /// This API sends the Application Version 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// by-Manoj Singh
        public ResponseUserLogedInfo GetAppVersion()
        {
            ResponseUserLogedInfo responseUserInfo = new ResponseUserLogedInfo();
            AppVersion appVersionModel = new AppVersion();
            AppVersionBusiness appVersionBusiness = new AppVersionBusiness(unitOfWork);

            AppVersionModel appVersion = appVersionBusiness.GetVersion();

            AutoMapper.Mapper.Map(appVersion, appVersionModel);
            responseUserInfo.Status = (int)EnumCode.status.Success;
            responseUserInfo.VersionCode = appVersionModel.VersionCode;
            return responseUserInfo;


        }
        /// <summary>
        /// This API add comment and Audio in LeadComment Table 
        /// </summary>
        /// <param name="leadComment"></param>
        /// <returns>response Message,Status,AudioFileName,AudioFileDuration</returns>
        /// by-Manoj Singh
        public ResponseActivity AddLeadComment(LeadComment leadComment)
        {
            LeadCommentModel leadCommentModel = new LeadCommentModel();
            LeadCommentBussiness leadCommentBusiness = new LeadCommentBussiness(unitOfWork);
            // ResponseStatus responseStatus = new ResponseStatus();
            ResponseActivity responseActivity = new ResponseActivity();
            AutoMapper.Mapper.Map(leadComment, leadCommentModel);
            leadCommentModel.CreatedDate = DateTime.UtcNow;

            if (leadComment.Audiofiledata != null)
            {
                FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
                string uploadFolder = HttpContext.Current.Server.MapPath(ReadConfiguration.CommentsClipsPath);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }
                int userId = leadComment.UserId.Decrypt();
                int leadId = leadComment.LeadId.Decrypt();

                string fileExtension = Path.GetExtension((leadComment.AudioFileName));
                // string uniqueName = Guid.NewGuid().ToShortGuid(6);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((leadComment.AudioFileName));
                string newFileName = "Comment" + "_" + leadId + "_" + "Audio" + DateTime.UtcNow.ToString("MM_dd_yyyy") + "_" + DateTime.UtcNow.ToString("HH_mm_ss") + fileExtension;
                string filePath = Path.Combine(uploadFolder, newFileName);

                try
                {
                    byte[] audioStream = Convert.FromBase64String(leadComment.Audiofiledata);
                    FileStream fileToupload = new FileStream(filePath, FileMode.Create);
                    //FileStream fileToupload = new FileStream(@"E:\NewTrunk(3-6)\CODEBASE01\CODE\GIT\trunk\ErucaCRM.Web\Uploads\leads\testingnow.wav", FileMode.Create);
                    fileToupload.Write(audioStream, 0, audioStream.Length);
                    fileToupload.Close();
                    fileToupload.Dispose();
                }
                catch (Exception exception)
                {
                    responseActivity.Message = exception.Message;
                }
                responseActivity.leadComment = new LeadComment();
                responseActivity.leadComment.AudioFileName = newFileName;
                responseActivity.leadComment.AudioFileDuration = leadComment.AudioFileDuration;
                responseActivity.Message = "Audio Saved";
                responseActivity.Status = (int)EnumCode.status.Success;
            }
            else
            {
                try
                {
                    leadCommentModel = leadCommentBusiness.AddCommentInLead(leadCommentModel);
                }
                catch
                {
                    responseActivity.Status = (int)EnumCode.status.Failure;
                    responseActivity.Message = "Comment Not Saved";
                }

                responseActivity.Message = "Comment Saved";
                responseActivity.Status = (int)EnumCode.status.Success;
            }
            return responseActivity;
        }
/// <summary>
        /// provide lead comment on {LeadId, CurrentPage, PageSize}
/// </summary>
/// <param name="leadComment"></param>
/// <returns>lead comment data</returns>
///  by-Manoj Singh
        public ResponseLead GetLeadComment(LeadComment leadComment)
        {
            ResponseLead responseLead = new ResponseLead();
            int totalRecords=leadComment.TotalRecords;
            try
            {
                List<LeadCommentModel> leadcommentModel = new List<LeadCommentModel>();
                LeadCommentBussiness leadCommentBusiness = new LeadCommentBussiness(unitOfWork);
                leadcommentModel = leadCommentBusiness.GetCommentsByLeadId(leadComment.LeadId.Decrypt(), leadComment.CurrentPage, leadComment.PageSize, ref totalRecords);
                responseLead.LeadComment = new List<LeadComment>();
                AutoMapper.Mapper.Map(leadcommentModel, responseLead.LeadComment);
                responseLead.Status = (int)EnumCode.status.Success;    
            }
            catch {
                responseLead.Status = (int)EnumCode.status.Failure;
            }

            return responseLead;
        }
    }

}

