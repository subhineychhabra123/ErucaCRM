using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System.Data.Objects;
namespace ErucaCRM.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public string CheckUserProfile(int companyId, int userId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            string profile = String.Join("|", entities.SSP_CheckUserPlan(companyId, userId).ToList().ToArray());
            return profile;
        }
        /// <summary>
        /// This function will create default profiles for registered customer and will return NewAdministratorProfileId for registered customers
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="NewAdministratorProfileId"></param>
        /// <returns></returns>
        public int CreateNewDefaultCustomerProfiles(int companyId, out int NewAdministratorProfileId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("NewAdministratorProfileId", 0);
            entities.ssp_CreateNewDefaultCustomerProfiles(companyId, objParam);
            NewAdministratorProfileId = Convert.ToInt32(objParam.Value);
            return NewAdministratorProfileId;
        }
    }
}
