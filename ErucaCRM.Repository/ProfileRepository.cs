using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
namespace ErucaCRM.Repository
{
  public  class ProfileRepository : BaseRepository<Profile>
    {
        public ProfileRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public int CreateDefaultStandardProfile(Nullable<int> companyID)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            return entities.ssp_CreateDefaultStandardProfile(companyID);
        }
    }
}
