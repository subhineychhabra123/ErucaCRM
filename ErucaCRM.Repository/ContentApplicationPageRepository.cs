using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Repository
{
   public class ContentApplicationPageRepository :BaseRepository<ContentApplicationPage>
    {
        public ContentApplicationPageRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
    }
}
