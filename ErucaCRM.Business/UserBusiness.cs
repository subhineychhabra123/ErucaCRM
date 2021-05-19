using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
namespace ErucaCRM.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserRepository userRepository;
        private readonly CountryRepository countryRepository;
        private readonly ProfileRepository profileRepository;
        private readonly RoleRepository roleRepository;
        public readonly StageRepository stageRepositry;
        public readonly AccountSettingRepository accountRepository;
        public UserBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            userRepository = new UserRepository(unitOfWork);
            countryRepository = new CountryRepository(unitOfWork);
            profileRepository = new ProfileRepository(unitOfWork);
            roleRepository = new RoleRepository(unitOfWork);
            stageRepositry = new StageRepository(unitOfWork);
            accountRepository=new AccountSettingRepository(unitOfWork);
        }


        public void AddUser(UserModel userModel)
        {
            User user = new User();
            AutoMapper.Mapper.Map(userModel, user);
            user.CreatedDate = DateTime.UtcNow;
            user.Active = true;
            user.AddressId = null;
            user.Address = null;
            user.Profile = null;
            user.Role = null;
            userRepository.Insert(user);
        }

        public Boolean ChangePassword(int userId, string currentPassword, string newPassword)
        {
            // string response = "";
            Boolean response = false;
            User user = UserByUserId(userId);
            if (user != null)
            {

                if (user.Password != currentPassword)
                {
                    // response = "current password is wrong";
                    response = false;
                }
                else
                {
                    user.Password = newPassword;
                    user.ModifiedDate = DateTime.UtcNow;
                    user.ModifiedBy = userId;
                    userRepository.Update(user);
                    response = true;
                    // response = "Success";
                }

            }
            return response;
        }


        public UserModel RegisterUser(UserModel userModel)
        {
            User user = new User();
            user.Company = new Company();            
            AutoMapper.Mapper.Map(userModel, user);
            user.CreatedDate = DateTime.UtcNow;
            user.Active = true;
            user.ProfileId = GetDefaultRegistertedUserProfileId();//(int)Enums.Profile.Administrator;
            user.UserTypeId = (int)Enums.UserType.User;
            user.RoleId = roleRepository.SingleOrDefault(x => x.IsDefaultForRegisterdUser == true && x.RecordDeleted == false).RoleId;
            
            user.Address = null;
            user.Profile = null;
            user.Role = null;
            user.Company.CreatedOn = DateTime.UtcNow;
            user.Company.IsActive = true;
            if (user.CultureInformationId == 0)
                user.CultureInformationId = null;
            if (user.TimezoneId == 0)
                user.TimezoneId = null;
            userRepository.Insert(user);
            UserSetting userSetting = new UserSetting();
            userSetting.IsSendNotificationsRecentActivities = true;
            userSetting.UserId = user.UserId;
            accountRepository.Insert(userSetting);
            //Add the default stage for newly created company of registered user

            List<Stage> listDefaultStages = stageRepositry.GetAll(x => x.CompanyId == null).ToList();

            for (int i = 0; i < listDefaultStages.Count; i++)
            {
                Stage objDefaultStage = new Stage();
                objDefaultStage = listDefaultStages[i];
                objDefaultStage.CompanyId = user.CompanyId;
                if (objDefaultStage.IsInitialStage == true)
                    objDefaultStage.StageOrder = 0;
                else if (objDefaultStage.IsLastStage == true)
                    objDefaultStage.StageOrder = 100;
                stageRepositry.Insert(objDefaultStage);
            }

            user = userRepository.SingleOrDefault(r => r.UserId == user.UserId);
            AutoMapper.Mapper.Map(user, userModel);
            //Code Added by mahesh bhatt for loading a user after register with full permission

            AutoMapper.Mapper.Map(user.Company, userModel.CompanyModel);
            AutoMapper.Mapper.Map(user.Profile, userModel.ProfileModel);
            AutoMapper.Mapper.Map(user.CultureInformation, userModel.CultureInformationModel);

            List<ProfilePermissionModel> profilePermissionModels = new List<ProfilePermissionModel>();
            AutoMapper.Mapper.Map(user.Profile.ProfilePermissions, profilePermissionModels);

            userModel.ProfileModel.ProfilePermissionModels = profilePermissionModels;

            ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
            AutoMapper.Mapper.Map(user.Profile.ProfilePermissions, profilePermissionModels);
            userModel.ProfileModel.ProfilePermissionModels = profilePermissionModels;
            ///////////////////////////////////////


            profileRepository.CreateDefaultStandardProfile(user.CompanyId);

            return userModel;
        }


        public void AddStaffUser(UserModel userModel, string emailSubject, string emailBody)
        {
            User user = new User();
            //fill data to user fields 
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.EmailId = userModel.EmailId;
            user.Password = GenerateRandoMPassword();
            user.TimezoneId = userModel.TimeZoneId;
            user.CultureInformationId = userModel.CultureInformationId;
           
            user.UserTypeId = (int)Enums.UserType.User;
            //update password in model to send email
            userModel.Password = user.Password;

            if (userModel.CompanyModel != null)
                user.CompanyId = userModel.CompanyModel.CompanyId;

            user.CreatedDate = DateTime.UtcNow;

            //if (userModel.RoleModel.RoleId == 0)
            //    user.RoleId = roleRepository.SingleOrDefault(x => x.IsDefaultForStaffUser == true && x.RecordDeleted == false).RoleId;
            //else
            //{
            user.RoleId = userModel.RoleModel.RoleId;
            // }
            if (userModel.ProfileModel.ProfileId == 0)
                user.ProfileId = profileRepository.SingleOrDefault(x => x.IsDefaultForStaffUser == true && x.RecordDeleted == false).ProfileId;
            else
                user.ProfileId = userModel.ProfileModel.ProfileId;
            //fill data to address fields 

            if (userModel.AddressModel != null)
            {
                Address address = new Address();
                address.Street = userModel.AddressModel.Street;
                address.City = userModel.AddressModel.City;
                address.Zipcode = userModel.AddressModel.Zipcode;
                if (userModel.AddressModel.CountryModel.CountryId > 0)
                    address.CountryId = userModel.AddressModel.CountryModel.CountryId;
                user.Address = address;
            }
            //Associate address to user

            user.Active = true;

            //save user data in database
            userRepository.Insert(user);

            UserSetting userSetting = new UserSetting();
            userSetting.IsSendNotificationsRecentActivities = true;
            userSetting.UserId = user.UserId;
            accountRepository.Insert(userSetting);
            //Send password email to newlt added user

            SendRegistrationEmail(userModel, emailSubject, emailBody);
        }

        public void ChangeImage(int userId, string ImageURL)
        {
            User user = UserByUserId(userId);
            if (user != null)
            {
                user.ImageURL = ImageURL;
                userRepository.Update(user);
            }
        }
        public void UpdateUserData(UserModel userModel)
        {
            UserModel updatedUserObj = new UserModel();
            User user = UserByUserId(userModel.UserId);
            if (user != null)
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.EmailId = userModel.EmailId;
                user.UserTypeId = (int)Enums.UserType.User;
                user.TimezoneId = userModel.TimeZoneId;
                user.CultureInformationId = userModel.CultureInformationId;
                if (userModel.RoleModel.RoleId == 0)
                    user.RoleId = roleRepository.SingleOrDefault(x => x.IsDefaultForStaffUser == true && x.RecordDeleted == false).RoleId;
                else
                    user.RoleId = userModel.RoleModel.RoleId;

                if (userModel.ProfileModel.ProfileId == 0)
                    user.ProfileId = profileRepository.SingleOrDefault(x => x.IsDefaultForStaffUser == true && x.RecordDeleted == false).ProfileId;
                else
                    user.ProfileId = userModel.ProfileModel.ProfileId;

                Address address;
                if (user.AddressId == null)
                {
                    address = new Address();
                }
                else
                    address = user.Address;

                address.Street = userModel.AddressModel.Street;
                address.City = userModel.AddressModel.City;
                address.Zipcode = userModel.AddressModel.Zipcode;
                if (userModel.AddressModel.CountryModel.CountryId > 0)
                    address.CountryId = userModel.AddressModel.CountryModel.CountryId;
                else
                    address.CountryId = null;
                user.Address = address;
                user.ModifiedDate = DateTime.UtcNow;
                userRepository.Update(user);

            }
        }

        private string GenerateRandoMPassword()
        {
            string password = "";
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            password = result;
            return password;
        }

        public Boolean IsEmailExist(string emailId, int? userID)
        {

            User userObj = userRepository.SingleOrDefault(x => x.EmailId == emailId && x.RecordDeleted == false && x.UserId != (userID ?? 0));
            if (userObj is User)
            {
                return true;
            }
            else
                return false;


        }


        public string UpdateUserStatus(int[] userIDs, Boolean userStatus, int ownerUserID)
        {
            string result = "";

            List<User> _listUsers = userRepository.GetAll(x => userIDs.Contains(x.UserId) && x.RecordDeleted == false).ToList();

            for (int i = 0; i < _listUsers.Count; i++)
            {
                _listUsers[i].Active = userStatus;
                _listUsers[i].ModifiedBy = ownerUserID;
                _listUsers[i].ModifiedDate = DateTime.UtcNow;
                userRepository.Update(_listUsers[i]);
            }

            result = "UpdateSucess";
            return result;

        }

        public UserModel UpdateUser(UserModel userModel)
        {
            UserModel updatedUserObj = new UserModel();
            User user = UserByUserId(userModel.UserId);
            if (user != null)
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.EmailId = userModel.EmailId;
                user.TimezoneId = userModel.TimeZoneId;
                user.CultureInformationId = userModel.CultureInformationId;
                Address address;
                if (user.AddressId == null)
                {
                    address = new Address();
                }
                else
                    address = user.Address;
                address.Street = userModel.AddressModel.Street;
                address.City = userModel.AddressModel.City;
                address.Zipcode = userModel.AddressModel.Zipcode;

                if (userModel.AddressModel.CountryModel.CountryId > 0)
                    address.CountryId = userModel.AddressModel.CountryModel.CountryId;
                else
                    address.CountryId = null;

                user.Address = address;
                user.ModifiedDate = DateTime.UtcNow;
                userRepository.Update(user);
                user = UserByUserId(userModel.UserId);
                AutoMapper.Mapper.Map(user, updatedUserObj);
                AutoMapper.Mapper.Map(user.TimeZone, updatedUserObj.TimeZoneModel);
                return updatedUserObj;
            }
            else return null;
        }

        public UserModel GetUserByUserId(int userId)
        {
            UserModel usermodel = null;
            User user = userRepository.SingleOrDefault(u => u.UserId == userId && u.RecordDeleted == false);
            usermodel = new UserModel();
            AutoMapper.Mapper.Map(user, usermodel);
            AutoMapper.Mapper.Map(user.Address, usermodel.AddressModel);
            AutoMapper.Mapper.Map(user.TimeZone, usermodel.TimeZoneModel);
            AutoMapper.Mapper.Map(user.CultureInformation, usermodel.CultureInformationModel);
            AutoMapper.Mapper.Map(user.Profile, usermodel.ProfileModel);
            AutoMapper.Mapper.Map(user.Role, usermodel.RoleModel);

            return usermodel;
        }

        public UserModel GetUserByEmailId(string emailId)
        {
            UserModel usermodel = null;
            User user = userRepository.SingleOrDefault(u => u.EmailId == emailId && u.RecordDeleted == false);
            if (user != null)
            {
                usermodel = new UserModel();
                AutoMapper.Mapper.Map(user, usermodel);
            }

            return usermodel;
        }

        /// <summary>
        /// Function to get the default profile id for registered user
        /// </summary>
        /// <returns></returns>
        public int GetDefaultRegistertedUserProfileId()
        {
            int profileID = 0;
            Profile DefaultRegistertedUserProfile = profileRepository.SingleOrDefault(x => x.CompanyId == null && x.RecordDeleted == false && x.IsDefaultForRegisterdUser == true && (x.IsDefaultForStaffUser ?? false) == false);
            if (DefaultRegistertedUserProfile != null)
                profileID = DefaultRegistertedUserProfile.ProfileId;

            return profileID;
        }


        /// <summary>
        /// Function to get the default profile id for registered user
        /// </summary>
        /// <returns></returns>
        public int GetDefaultRegistertedUserRoleId()
        {
            int roleId = 0;
            Role DefaultRegistertedUserRole = roleRepository.SingleOrDefault(x => x.CompanyId == null && x.RecordDeleted == false && (x.IsDefaultForRegisterdUser ?? false) == true && (x.IsDefaultForStaffUser ?? false) == false);
            if (DefaultRegistertedUserRole != null)
                roleId = DefaultRegistertedUserRole.RoleId;

            return roleId;
        }

        private User UserByUserId(int userId)
        {
            User user = userRepository.SingleOrDefault(u => u.UserId == userId && u.RecordDeleted == false);
            return user;
        }

        public UserModel ValidateUser(string email, string password, bool isChecked)
        {
            User objuser = userRepository.SingleOrDefault(a => a.EmailId == email && a.RecordDeleted == false && a.Active == true&&a.Company.IsActive==true);
            if (objuser != null && string.Compare(objuser.Password, password) == 0)
            {
                UserModel userModel = new UserModel();

                AutoMapper.Mapper.Map(objuser, userModel);
                AutoMapper.Mapper.Map(objuser.Company, userModel.CompanyModel);
                AutoMapper.Mapper.Map(objuser.Profile, userModel.ProfileModel);
                AutoMapper.Mapper.Map(objuser.CultureInformation, userModel.CultureInformationModel);
                AutoMapper.Mapper.Map(objuser.TimeZone, userModel.TimeZoneModel);

                List<ProfilePermissionModel> profilePermissionModels = new List<ProfilePermissionModel>();
                AutoMapper.Mapper.Map(objuser.Profile.ProfilePermissions, profilePermissionModels);

                userModel.ProfileModel.ProfilePermissionModels = profilePermissionModels;

                ModulePermissionModel modulePermissionModel = new ModulePermissionModel();
                AutoMapper.Mapper.Map(objuser.Profile.ProfilePermissions, profilePermissionModels);
                userModel.ProfileModel.ProfilePermissionModels = profilePermissionModels;

                return userModel;
            }
            return null;
        }

        public bool SendPasswordRecoveryMail(UserModel user, string emailSubject, string emailBody)
        {

            string websiteLogoPath = ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath;
            string to = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            bool IsMailSent = true;
            to = user.EmailId;
            subject = emailSubject;
            body = string.Format(emailBody, websiteLogoPath, user.EmailId, user.Password);            
            //body = "<div style='font-face:arial;'><img src='" + websiteLogoPath + "'><hr/>Dear Customer" + ",<br/><br/>Please return to the site and log in using the following information" + "<br/>Your Username: " + user.EmailId.Trim() + "<br/>Password: " + user.Password + " <br/><br/>Thank you.<br/ >Customer Relations</div>";

            try
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.SendMailMessage(to, subject, body);
            }
            catch (Exception ex)
            {
                IsMailSent = false;
            }

            return IsMailSent;
        }
        public bool SendPasswordRecoveryMail(UserModel user)
        {

            string websiteLogoPath = ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath;
            string to = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            bool IsMailSent = true;
            to = user.EmailId;
            subject = "Eurca CRM-Password Recovery";          
            body = "<div style='font-face:arial;'><img src='" + websiteLogoPath + "'><hr/>Dear Customer" + ",<br/><br/>Please return to the site and log in using the following information" + "<br/>Your Username: " + user.EmailId.Trim() + "<br/>Password: " + user.Password + " <br/><br/>Thank you.<br/ >Customer Relations</div>";

            try
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.SendMailMessage(to, subject, body);
            }
            catch (Exception ex)
            {
                IsMailSent = false;
            }

            return IsMailSent;
        }

        public bool SendRegistrationEmail(UserModel user, string emailSubject, string emailBody)
        {

            string websiteUrl = ErucaCRM.Utility.ReadConfiguration.WebsiteUrl;
            string websiteLogoPath = ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath;
            string to = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            bool IsMailSent = true;
            to = user.EmailId;
            subject = emailSubject;
            body = string.Format(emailBody, websiteLogoPath, websiteUrl, user.EmailId.Trim(), user.Password);           
            //body = "<div style='font-face:arial;'><img src='" + websiteLogoPath + "'><hr/>Dear Customer" + ",<br/><br/>Thanks for your registration to Eruca CRM. <br/> Please find the following details to login into Eruca CRM. <br/><br/>CRM URL: <a href=" + websiteUrl + ">Click here to navigate to CRM<a/><br/><br/>" + "Your Username: " + user.EmailId.Trim() + "<br/>Password: " + user.Password + " <br/><br/>Thank you.<br/><br/>Customer Relations</div>";

            try
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.SendMailMessage(to, subject, body);
            }
            catch (Exception ex)
            {
                IsMailSent = false;
            }

            return IsMailSent;
        }
        public bool SendRegistrationEmail(UserModel user)
        {

            string websiteUrl = ErucaCRM.Utility.ReadConfiguration.WebsiteUrl;
            string websiteLogoPath = ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath;
            string to = string.Empty;
            string subject = string.Empty;
            string body = string.Empty;
            bool IsMailSent = true;
            to = user.EmailId;
            subject = "Welcome to Eurca CRM";           
            body = "<div style='font-face:arial;'><img src='" + websiteLogoPath + "'><hr/>Dear Customer" + ",<br/><br/>Thanks for your registration to Eruca CRM. <br/> Please find the following details to login into Eruca CRM. <br/><br/>CRM URL: <a href=" + websiteUrl + ">Click here to navigate to CRM<a/><br/><br/>" + "Your Username: " + user.EmailId.Trim() + "<br/>Password: " + user.Password + " <br/><br/>Thank you.<br/><br/>Customer Relations</div>";

            try
            {
                MailHelper mailHelper = new MailHelper();
                mailHelper.SendMailMessage(to, subject, body);
            }
            catch (Exception ex)
            {
                IsMailSent = false;
            }

            return IsMailSent;
        }


        public List<CountryModel> GetCountries()
        {
            List<CountryModel> listCountryModel = new List<CountryModel>();
            List<Country> listCountry = new List<Country>();
            listCountry = countryRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(listCountry, listCountryModel);
            return listCountryModel;
        }
        public List<UserModel> GetAllUsers(int companyId)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            List<User> listUsers = new List<User>();
            listUsers = userRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }

        public List<UserModel> GetAllActiveUsers(int companyId)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            List<User> listUsers = new List<User>();
            listUsers = userRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.Active == true).ToList();
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }

        public List<UserSetting> GetSendNotificationsActiveUsersId(int companyId)
        {
            List<UserSetting> userSetting = accountRepository.GetAll(x => x.User.CompanyId == companyId && x.User.RecordDeleted == false && x.User.Active == true && x.IsSendNotificationsRecentActivities == true).ToList();
            return userSetting;
        }


        public List<UserModel> GetAllUsersByStaus(int companyId, int currentPage, Boolean userStatus, Int16 pageSize, ref long totalRecords)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            List<User> listUsers = new List<User>();
            totalRecords = userRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && (x.Active ?? false) == userStatus);
            listUsers = userRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && (x.Active ?? false) == userStatus, y => y.FirstName, currentPage, pageSize).ToList();
            AutoMapper.Mapper.Map(listUsers, listUserModel);

            return listUserModel;
        }

        public List<UserModel> GetAdminUsersByStaus(int userTypeId, int currentPage, Boolean userStatus, Int16 pageSize, ref long totalRecords)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            List<User> listUsers = new List<User>();
            totalRecords = userRepository.Count(x => x.UserTypeId == userTypeId && x.RecordDeleted == false && (x.Active ?? false) == userStatus);
            listUsers = userRepository.GetPagedRecords(x => x.UserTypeId == userTypeId && x.RecordDeleted == false && (x.Active ?? false) == userStatus, y => y.FirstName, currentPage, pageSize).ToList();
            AutoMapper.Mapper.Map(listUsers, listUserModel);

            return listUserModel;
        }

        public List<UserModel> GetAllUsers(int companyId, bool isActive, int exceptThisId = 0)
        {
            List<UserModel> listUserModel = new List<UserModel>();
            List<User> listUsers = new List<User>();
            listUsers = userRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.Active == isActive && x.UserId != exceptThisId).ToList();
            AutoMapper.Mapper.Map(listUsers, listUserModel);
            return listUserModel;
        }
        public void AssignRoles(List<UserModel> userModelList, int modifiedBy)
        {
            foreach (var userModel in userModelList)
            {
                User user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
                if (user != null)
                {
                    user.UserTypeId = userModel.UserTypeId;
                    user.ModifiedDate = DateTime.UtcNow;
                    user.ModifiedBy = modifiedBy;
                    userRepository.Update(user);
                }
            }
        }
        public void AssignProfiles(List<UserModel> userModelList, int modifiedBy)
        {
            foreach (var userModel in userModelList)
            {
                User user = userRepository.SingleOrDefault(x => x.UserId == userModel.UserId && x.RecordDeleted == false);
                if (user != null)
                {
                    user.ProfileId = userModel.ProfileId;
                    user.ModifiedDate = DateTime.UtcNow;
                    user.ModifiedBy = modifiedBy;
                    userRepository.Update(user);
                }
            }
        }
        public List<OwnerListModel> GetAllOwnerDropdownList(int companyId, bool isActive, int exceptThisId = 0)
        {
            List<OwnerListModel> ownerListModel = new List<OwnerListModel>();
            List<User> listUsers = new List<User>();
            listUsers = userRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.Active == isActive && x.UserId != exceptThisId).ToList();
            AutoMapper.Mapper.Map(listUsers, ownerListModel);
            //ownerListModel.Insert(0, new OwnerListModel { LeadOwnerId = 0, FirstName = "---Select---" });
            //ownerListModel.ForEach(y=> y.LeadOwnerId = 0);
            return ownerListModel;
        }

        public bool DeleteProfile(int ProfileId, int reassignedId, int? companyId, int ModifiedBy)
        {

            List<User> listUsers = new List<User>();
            Profile profile = new Profile();
            listUsers = userRepository.GetAll(x => x.ProfileId == ProfileId && x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            if (listUsers.Count > 0)
            {
                foreach (User userObj in listUsers)
                {
                    userObj.ProfileId = reassignedId;
                    userObj.ModifiedDate = DateTime.UtcNow;
                    userObj.ModifiedBy = ModifiedBy;

                }
                userRepository.UpdateAll(listUsers);
            }
            profile = profileRepository.SingleOrDefault(r => r.ProfileId == ProfileId && r.CompanyId == companyId && r.RecordDeleted == false);
            if (profile != null)
            {
                profile.RecordDeleted = true;
                profile.ModifiedDate = DateTime.UtcNow;
                profile.ModifiedBy = ModifiedBy;
                profileRepository.Update(profile);
                return true;
            }
            else
                return false;
        }

        public string CheckCompanyPlanProfile(int companyId, int userId)
        {
            string profile = userRepository.CheckUserProfile(companyId, userId);
            return profile;
        }

        public Dictionary<string, string> GetAutherizedModuleName(int userId)
        {
            User user = userRepository.SingleOrDefault(x => x.UserId == userId && x.Active == true);
            List<Permission> permissions = new List<Permission>();
            Dictionary<string, string> permissionList = new Dictionary<string, string>();
            List<string> moduleNameList = user.Profile.ProfilePermissions.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList();
            permissionList = moduleNameList.ToDictionary(x => x, x => x);
            return permissionList;
        }
        /// <summary>
        /// For Mobile App Sending Permission in True and False with the Permission name
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Permission name with True and False</returns>
        /// Manoj Singh
        public Dictionary<string, bool> GetAutherizedModuleNameForMobile(int userId)
        {
            User user = userRepository.SingleOrDefault(x => x.UserId == userId && x.Active == true);
            List<Permission> permissions = new List<Permission>();
            Dictionary<string, bool> permissionList = new Dictionary<string, bool>();
            // List<string> moduleNameList = user.Profile.ProfilePermissions.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList();
            //List<string> moduleNameList = user.Profile.ProfilePermissions.Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT +" : "+ x.HasAccess).ToList();// , Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList();

            List<Utility.WebClasses.Module> moduleNameList = user.Profile.ProfilePermissions.Select(x => new Utility.WebClasses.Module { HasAccess = x.HasAccess, ModuleName = x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT }).ToList();
            permissionList = moduleNameList.ToDictionary(x => x.ModuleName, x => x.HasAccess);
            return permissionList;
        }
        public void SaveUserToken(UserModel userObj)
        {
            User user = userRepository.SingleOrDefault(r => r.UserId == userObj.UserId);
            user.TokenId = userObj.tokenId;
            userRepository.Update(user);
            //Code For Saving token to database
        }

        //
        public bool ValidateUserToken(string token)
        {
            User user = userRepository.SingleOrDefault(r => r.TokenId == token);
            if (user != null)
                return true;
            else return false;
        }

        public bool SendLeadAssociationMail(LeadModel lead, string emailSubject, string emailBody)
        {
            User associatedTouser = new User();
            associatedTouser = userRepository.SingleOrDefault(r => r.UserId == lead.LeadOwnerId && r.RecordDeleted == false);
            if (associatedTouser != null)
            {

                string websiteUrl = ErucaCRM.Utility.ReadConfiguration.WebsiteUrl;
                string websiteLogoPath = ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath;
                string to = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;
                bool IsMailSent = true;
                to = associatedTouser.EmailId;
                subject = emailSubject;
                body = string.Format(emailBody, websiteLogoPath, Convert.ToString(!string.IsNullOrEmpty(lead.FirstName) ? lead.FirstName : lead.Title), lead.LeadCompanyName, Convert.ToString(lead.Amount.HasValue ? lead.Amount.Value : 0), websiteUrl);
                // body = "<div style='font-face:arial;'><img src='" + websiteLogoPath + "'><hr/>Dear Customer" + ",<br/><br/>You are assigned a new lead.<br><h2>Lead Information-</h2><br>LeadName:" + Convert.ToString(!string.IsNullOrEmpty(lead.FirstName) ? lead.FirstName : lead.Title) + "</br><br>Company Name:" + lead.LeadCompanyName + "</br><br>Amount:" + Convert.ToString(lead.Amount.HasValue ? lead.Amount.Value : 0) + "</br>Please login into Eruca CRM for more detail. <br/><br/>CRM URL: <a href=" + websiteUrl + ">Click here to navigate to CRM<a/><br/><br/><br/><br/>Thank you.<br/ >Customer Relations</div>";
                try
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.SendMailMessage(to, subject, body);
                }
                catch (Exception ex)
                {
                    IsMailSent = false;
                }

                return IsMailSent;
            }
            else return false;
        }
        public bool SendLeadAssociationMail(LeadModel lead)
        {
            User associatedTouser = new User();
            associatedTouser = userRepository.SingleOrDefault(r => r.UserId == lead.LeadOwnerId && r.RecordDeleted == false);
            if (associatedTouser != null)
            {

                string websiteUrl = ErucaCRM.Utility.ReadConfiguration.WebsiteUrl;
                string websiteLogoPath = ErucaCRM.Utility.ReadConfiguration.WebsiteLogoPath;
                string to = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;
                bool IsMailSent = true;
                to = associatedTouser.EmailId;
                subject = "Lead information Eurca CRM";                
                body = "<div style='font-face:arial;'><img src='" + websiteLogoPath + "'><hr/>Dear Customer" + ",<br/><br/>You are assigned a new lead.<br><h2>Lead Information-</h2><br>LeadName:" + Convert.ToString(!string.IsNullOrEmpty(lead.FirstName) ? lead.FirstName : lead.Title) + "</br><br>Company Name:" + lead.LeadCompanyName + "</br><br>Amount:" + Convert.ToString(lead.Amount.HasValue ? lead.Amount.Value : 0) + "</br>Please login into Eruca CRM for more detail. <br/><br/>CRM URL: <a href=" + websiteUrl + ">Click here to navigate to CRM<a/><br/><br/><br/><br/>Thank you.<br/ >Customer Relations</div>";
                //body =  "<div style='font-face:arial;'><img src='{0}'><hr/>Dear Customer" + ",<br/><br/>Thanks for your registration to Eruca CRM. <br/> Please find the following details to login into Eruca CRM. <br/><br/>CRM URL: <a href='{1}'>Click here to navigate to CRM<a/><br/><br/>" + "Your Username:  {2} <br/>Password: {3} <br/><br/>Thank you.<br/><br/>Customer Relations</div>";
                try
                {
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.SendMailMessage(to, subject, body);
                }
                catch (Exception ex)
                {
                    IsMailSent = false;
                }

                return IsMailSent;
            }
            else return false;
        }


        public UserModel AddUpdateAdminStaffUser(UserModel userModel)
        {
            User user = new User();
            if (userModel.UserId > 0)
            {
                user = userRepository.SingleOrDefault(r => r.UserId == userModel.UserId && r.RecordDeleted == false);
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.EmailId = userModel.EmailId;
                if (userModel.CompanyModel != null)
                    user.CompanyId = userModel.CompanyModel.CompanyId;
                if (userModel.AddressModel != null)
                {
                    Address address = new Address();
                    address.Street = userModel.AddressModel.Street;
                    address.City = userModel.AddressModel.City;
                    address.Zipcode = userModel.AddressModel.Zipcode;
                    if (userModel.AddressModel.CountryModel.CountryId > 0)
                        address.CountryId = userModel.AddressModel.CountryModel.CountryId;
                    user.Address = address;
                }

                userRepository.Update(user);
            }
            else
            {
                user.FirstName = userModel.FirstName;
                user.LastName = userModel.LastName;
                user.EmailId = userModel.EmailId;
                user.Password = GenerateRandoMPassword();
                user.UserTypeId = (int)Utility.Enums.UserType.Admin;
                //update password in model to send email
                userModel.Password = user.Password;
                user.RoleId = null;

                user.ProfileId = Utility.ReadConfiguration.SubAdminProfileID;

                if (userModel.CompanyModel != null)
                    user.CompanyId = userModel.CompanyModel.CompanyId;
                if (userModel.AddressModel != null)
                {
                    Address address = new Address();
                    address.Street = userModel.AddressModel.Street;
                    address.City = userModel.AddressModel.City;
                    address.Zipcode = userModel.AddressModel.Zipcode;
                    if (userModel.AddressModel.CountryModel.CountryId > 0)
                        address.CountryId = userModel.AddressModel.CountryModel.CountryId;
                    user.Address = address;
                }
                //save user data in database
                user.Active = true;
                userRepository.Insert(user);
            }

            //Send password email to newlt added user

            // SendPasswordRecoveryMail(userModel);
            return userModel;
        }
        public UserAccess ValidateUserAccess(string tokenId, string moduleCONSTANT = "", string permissionCONSTANT = "")
        {
            UserAccess userAccess = new UserAccess();
            User user = userRepository.SingleOrDefault(x => x.RecordDeleted == false && x.Active == true && x.TokenId == tokenId);
            if (user != null)
            {
                userAccess.IsValidToken = true;
                if (!string.IsNullOrEmpty(moduleCONSTANT) && !string.IsNullOrEmpty(permissionCONSTANT))
                {
                    List<ProfilePermission> profilePermissions = user.Profile.ProfilePermissions.Where(x => x.HasAccess == true && x.ModulePermission.Module.ModuleCONSTANT == moduleCONSTANT && x.ModulePermission.Permission.PermissionCONSTANT == permissionCONSTANT).ToList();
                    if (profilePermissions != null && profilePermissions.Count() > 0)
                    {
                        userAccess.hasMethodPermission = true;

                    }
                }
            }
            return userAccess;

        }
        public UserSettingModel GetUserAccountSetting(int userId)
        {
            UserSettingModel userSettingModel = new UserSettingModel();
            UserSetting userSetting = accountRepository.SingleOrDefault(x => x.UserId == userId);
            AutoMapper.Mapper.Map(userSetting, userSettingModel);
            return userSettingModel;
        }
        public void UpdateUserAccountSetting(UserSettingModel userSettingModel)
        {
            UserSetting userSetting = new UserSetting();
            userSetting = accountRepository.SingleOrDefault(x => x.UserSettingId == userSettingModel.UserSettingId);
            if (userSetting != null)
            {
                userSetting.UserId = userSettingModel.UserId;
                userSetting.IsSendNotificationsRecentActivities = userSettingModel.IsSendNotificationsRecentActivities;
                userSetting.UserSettingId = userSettingModel.UserSettingId;            
                accountRepository.Update(userSetting);
             //   AutoMapper.Mapper.Map(accountseting, accountSettingModel);
            }
            else
            {
                userSetting = new UserSetting();
                userSetting.UserId = userSettingModel.UserId;
                userSetting.IsSendNotificationsRecentActivities = userSettingModel.IsSendNotificationsRecentActivities;

                accountRepository.Insert(userSetting);
          //  AutoMapper.Mapper.Map(accountseting,accountSettingModel);
            }
           // return accountSettingModel;
           
           
        }
        public void UpdateUserNotification(int userid,int leadauditId)
        {
            UserSetting usersetting = accountRepository.SingleOrDefault(x => x.UserId == userid);
            if (usersetting != null)
            {
                usersetting.SchedulerMaxLeadAuditId = leadauditId;
                accountRepository.Update(usersetting);
            }
        }
    }



}
