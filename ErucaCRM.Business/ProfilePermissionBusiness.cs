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
    public class ProfilePermissionBusiness : IProfilePermissionBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProfilePermissionRepository profilePermissionRepository;

        public ProfilePermissionBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            profilePermissionRepository = new ProfilePermissionRepository(unitOfWork);
        }

        public void UpdateProfilePermission(List<ProfilePermissionModel> profilePermissionModels,int modifiedBy)
        {
            int? profileId = profilePermissionModels[0].ProfileId;
            //ProfilePermission profilePermission = profilePermissionRepository.SingleOrDefault(r => r.ProfilePermissionId == profilePermissionModel.ProfilePermissionId);
            List<ProfilePermission> profilePermissionList = profilePermissionRepository.GetAll(r => r.ProfileId == profileId).ToList();
            if (profilePermissionList != null)
            {
                foreach (var profilePermissionModel in profilePermissionModels)
                {
                    ProfilePermission profilePermission = profilePermissionList.Where(x => x.ProfilePermissionId == profilePermissionModel.ProfilePermissionId).SingleOrDefault();
                    profilePermission.HasAccess = profilePermissionModel.HasAccess;
                    profilePermission.ModifiedDate = DateTime.UtcNow;
                    profilePermission.ModifiedBy = modifiedBy;
                }
                profilePermissionRepository.UpdateAll(profilePermissionList);
            }

        }
    }
}
