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
   public class LeadCommentRepository:BaseRepository<LeadComment>
    {
       public LeadCommentRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
