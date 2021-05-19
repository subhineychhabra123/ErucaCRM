using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;

namespace ErucaCRM.Repository
{
    public class TagRepository : BaseRepository<Tag>
    {
        public TagRepository(IUnitOfWork unit)
            : base(unit)
        {

        }

        public List<Tag> GetAll(int companyId)
        {
            List<Tag> listTags = GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadTags.Count() > 0).OrderByDescending(y => y.LeadTags.Count()).Distinct().ToList();
            return listTags;
        }
    }
}
