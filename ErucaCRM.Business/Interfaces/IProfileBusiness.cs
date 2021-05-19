using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IProfileBusiness
    {
        bool AddProfile(ProfileModel profileModel);
        bool UpdateProfile(ProfileModel profileModel);
        List<ProfileModel> GetProfileByCompanyId(int companyId);
        ProfileModel GetProfileTypeById(int profileId);
        ProfileModel GetProfileDetail(int profileId, int? companyId);
        void UpdateCompanyStandardProfile();
        string GetPipeSepratedPermission(int profileId, int? companyId);
        int CreateDefaultStandardProfile(Nullable<int> companyID);
    }
}
