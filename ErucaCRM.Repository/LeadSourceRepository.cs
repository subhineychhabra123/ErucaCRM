using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
namespace ErucaCRM.Repository
{
   public class LeadSourceRepository: BaseRepository<LeadSource>
    {
       public LeadSourceRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}