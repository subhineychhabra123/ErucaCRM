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
    public class ProfileBusiness : IProfileBusiness
    {
        private readonly ProfileRepository profileRepository;
        private readonly ProfilePermissionRepository profilePermissionRepository;

        private readonly CompanyRepository companyRepository;

        public ProfileBusiness(IUnitOfWork _unitOfWork)
        {
            profileRepository = new ProfileRepository(_unitOfWork);
            profilePermissionRepository = new ProfilePermissionRepository(_unitOfWork);
            companyRepository = new CompanyRepository(_unitOfWork);
        }
        public void UpdateCompanyStandardProfile()
        {
            List<Company> _listActiveCompanies = companyRepository.GetAll(x => x.IsActive == true && x.RecordDeleted == false).ToList();
            for (int i = 0; i < _listActiveCompanies.Count; i++)
            {
                if (_listActiveCompanies[i].Profiles.Where(x => x.IsDefaultForStaffUser != null && x.IsDefaultForStaffUser == true).Count() == 0)
                {
                    profileRepository.CreateDefaultStandardProfile(_listActiveCompanies[i].CompanyId);
                }
            
            }          
            
           
        }
        public bool AddProfile(ProfileModel profileModel)
        {
            Profile profile = new Profile();
            AutoMapper.Mapper.Map(profileModel, profile);
            bool isExists = profileRepository.Exists(r => r.ProfileName == profileModel.ProfileName && (r.CompanyId == profileModel.CompanyId || profileModel.CompanyId == null) && r.RecordDeleted == false);

         if (!isExists)
         {
             List<ProfilePermission> defaultPermissions = profilePermissionRepository.GetAll(x => x.Profile.IsDefaultForRegisterdUser == true ).ToList();
             foreach (ProfilePermission profilePermission in defaultPermissions)
             {
                 profile.ProfilePermissions.Add(new ProfilePermission()
                 {
                     HasAccess = profilePermission.HasAccess,
                     CreatedDate = DateTime.UtcNow,
                     ModulePermissionId = profilePermission.ModulePermissionId,
                     CreatedBy = profileModel.CreatedBy
                 });
             }
             profileRepository.Insert(profile);
         return true;
         }
         else
         {
             return false;
         }
        }
        public bool UpdateProfile(ProfileModel profileModel)
        {
            bool returnMessage = false;
            Profile profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileModel.ProfileId && p.RecordDeleted == false);
            bool isExists = profileRepository.Exists(r => r.ProfileName.Trim() == profileModel.ProfileName.Trim() && (r.CompanyId == profileModel.CompanyId || profileModel.CompanyId == null) && r.RecordDeleted == false && r.ProfileId != profileModel.ProfileId);
            if (!isExists)
            {
                if (profile != null)
                {
                    profile.ProfileName = profileModel.ProfileName;
                    profile.Description = profileModel.Description;
                    profile.ModifiedDate = profileModel.ModifiedDate;
                    profile.ModifiedBy = profileModel.ModifiedBy;
                    profileRepository.Update(profile);
                    returnMessage = true;
                }
                
            }
            else
            {

                if (profile != null && profileModel.Description != profile.Description)
                {
                    profile.Description = profileModel.Description;
                    profile.ModifiedDate = profileModel.ModifiedDate;
                    profile.ModifiedBy = profileModel.ModifiedBy;
                    profileRepository.Update(profile);
                    returnMessage = true;
                }
                else
                {
                    returnMessage = false;
                }

            }
            return returnMessage;
           
        }
        public List<ProfileModel> GetProfileByCompanyId(int companyId)
        {
            List<ProfileModel> listProfileModel = new List<ProfileModel>();
            List<Profile> listProfile = profileRepository.GetAll(p => p.CompanyId == companyId && p.RecordDeleted == false || (p.CompanyId.HasValue == false && p.IsDefaultForRegisterdUser!=null && p.IsDefaultForRegisterdUser == true)).ToList();
            AutoMapper.Mapper.Map(listProfile, listProfileModel);
            return listProfileModel;
        }

        public ProfileModel GetProfileTypeById(int profileId)
        {

            ProfileModel profileModel = new ProfileModel();
            Profile profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);
            AutoMapper.Mapper.Map(profile, profileModel);
            return profileModel;
        }
        public ProfileModel GetProfileDetail(int profileId, int? companyId)
        {
            ProfileModel profileModel = new ProfileModel();
            Profile profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);
            if (profile != null)
                if (profile.IsDefaultForRegisterdUser != true)
                {
                    if (companyId == null)
                    {
                        profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false);
                    }
                    else
                    {
                        profile = profileRepository.SingleOrDefault(p => p.ProfileId == profileId && p.RecordDeleted == false && p.CompanyId == (int)companyId);
                    }

                }
            AutoMapper.Mapper.Map(profile, profileModel);
            AutoMapper.Mapper.Map(profile.ProfilePermissions, profileModel.ProfilePermissionModels);
          //  profileModel.ProfilePermissionModels=profileModel.ProfilePermissionModels.Select(x => x).OrderBy(y => y.ModulePermission.Permission.PermissionId).ToList();

            //profileModel.ProfilePermissionModels
       //     profileModel.ProfilePermissionModels.

            return profileModel;
        }
        public string GetPipeSepratedPermission(int profileId, int? companyId)
        {
            String pipesepratedPermissions = string.Empty;
            ProfileModel profileModel = this.GetProfileDetail(profileId, companyId);
            // profileModel.ProfilePermissions = profileModel.ProfilePermissions.OrderBy(x => x.ModulePermission.Module.SortOrder).ToList();
            var permissionArray = profileModel.ProfilePermissionModels.Where(x => x.HasAccess == true).Select(x => x.ModulePermission.Module.ModuleCONSTANT + x.ModulePermission.Permission.PermissionCONSTANT).ToList();
            pipesepratedPermissions = String.Join("|", permissionArray.ToArray());
            return pipesepratedPermissions;
        }
        public int CreateDefaultStandardProfile(Nullable<int> companyID)
        {
            return profileRepository.CreateDefaultStandardProfile(companyID);
        }
    }
}
