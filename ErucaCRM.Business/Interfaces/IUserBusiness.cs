using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IUserBusiness
    {
        UserModel GetUserByEmailId(string emailId);
        void AddUser(UserModel user);
        UserModel RegisterUser(UserModel user);
        void AddStaffUser(UserModel user, string emailSubject, string emailBody);
        UserModel ValidateUser(string userName, string password, bool isChecked);
        /// <summary>
        /// Send PasswordRecovery Email By Excel Template
        /// </summary>
        /// <param name="user"></param>
        /// <param name="EmailSubject"></param>
        /// <param name="EmailBody"></param>
        /// <returns></returns>
        bool SendPasswordRecoveryMail(UserModel user,string emailSubject,string emailBody);
        /// <summary>
        /// Send PasswordRecovery Email without Excel Template
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool SendPasswordRecoveryMail(UserModel user);
        UserModel GetUserByUserId(int userId);
        List<CountryModel> GetCountries();
        UserModel UpdateUser(UserModel user);
        void UpdateUserData(UserModel user);
        String UpdateUserStatus(int[] userIDs, Boolean userStatus, int ownerUserID);
        void ChangeImage(int userId, string imageURL);
        List<UserModel> GetAllUsersByStaus(int companyId, int currentPage, Boolean userStatus, Int16 pageSize, ref long totalRecords);
        List<UserModel> GetAllUsers(int companyId);
        List<UserModel> GetAllUsers(int companyId, bool isActive, int exceptThisId = 0);
        void AssignRoles(List<UserModel> userModelList, int modifiedBy);
        void AssignProfiles(List<UserModel> userModelList, int modifiedBy);
        List<OwnerListModel> GetAllOwnerDropdownList(int companyId, bool isActive, int exceptThisId = 0);
        Boolean IsEmailExist(string emailId, int? userID);
        int GetDefaultRegistertedUserProfileId();
        int GetDefaultRegistertedUserRoleId();
        /// <summary>
        ///  Send Registration Email By Excel Template
        /// </summary>
        /// <param name="user"></param>
        /// <param name="EmailSubject"></param>
        /// <param name="EmailBody"></param>
        /// <returns></returns>
        bool SendRegistrationEmail(UserModel user,string emailSubject,string emailBody);
        /// <summary>
        ///  Send Registration Email without Excel Template
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool SendRegistrationEmail(UserModel user);
        Boolean ChangePassword(int userId, string currentPassword, string newPassword);
        bool DeleteProfile(int profileId, int reassignProfileId, int? companyId, int ModifiedBy);
        /// <summary>
        /// Send Send Lead AssociationMail Email By Excel Template
        /// </summary>
        /// <param name="lead"></param>
        /// <param name="EmailSubject"></param>
        /// <param name="EmailBody"></param>
        /// <returns></returns>
        bool SendLeadAssociationMail(LeadModel lead,string emailSubject,string emailBody);
        /// <summary>
        ///  Send Send Lead AssociationMail Email without Excel Template
        /// </summary>
        /// <param name="lead"></param>
        /// <returns></returns>
        bool SendLeadAssociationMail(LeadModel lead);
        string CheckCompanyPlanProfile(int companyId, int userId);
        List<UserModel> GetAdminUsersByStaus(int userTypeId, int currentPage, Boolean userStatus, Int16 pageSize, ref long totalRecords);
        UserModel AddUpdateAdminStaffUser(UserModel user);
        UserAccess ValidateUserAccess(string tokenId, string moduleCONSTANT = "", string permissionCONSTANT = "");
        List<UserModel> GetAllActiveUsers(int companyId);
        UserSettingModel GetUserAccountSetting(int userId);
        void UpdateUserAccountSetting(UserSettingModel userSettingModel);
        void UpdateUserNotification(int userid, int leadauditId);
    }

}
