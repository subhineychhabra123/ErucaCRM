using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;

namespace ErucaCRM.Business.Interfaces
{
    public interface ITagBusiness
    {
        List<TagModel> GetAllTags(int companyId, int currentPage, Int16 pageSize, ref int totalRecords);
        List<AutoCompleteModel> GetSearchTags(int companyId, string tagSearchText);
         bool AddTag(TagModel tagModel);
         bool UpdateTag(TagModel tagModel);
         bool DeleteTag(int tagId, int userId);
         TagModel GetTagDetails(int tagId);

    }
}
