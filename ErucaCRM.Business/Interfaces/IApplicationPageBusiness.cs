using ErucaCRM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
    public interface IApplicationPageBusiness
    {
        ApplicationPageModel GetPageDetail(int cultureInformationId, int pageId);
        void UpdatePageContent(ApplicationPageModel pageModel);
        List<ApplicationPageModel> GetPublicPages();
        void AddCustomPage(ApplicationPageModel pageModel);
        List<ContentApplicationPageModel> GetCustomPages(int applicationPageId, int cultureInformationId, bool viewAll);
    
        bool RemoveCustomPage(int applicationPageId, int customPageId);
        bool AddCustomPage(int applicationPageId, int customPageId);
        void DeleteCustomPage(int applicationPageId);
    }
}
