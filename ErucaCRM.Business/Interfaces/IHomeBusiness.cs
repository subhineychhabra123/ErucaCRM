using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;

namespace ErucaCRM.Business.Interfaces
{
   public interface IHomeBusiness
    {
       List<HomeModel> GetRecentActivitiesForHome(int pageSize, int currentPage,int leadAuditId,bool isLoadMore,int companyId,int UserId);

      HomeModel GetDashboardData(int CompanyId, string Interval);
    }
}
