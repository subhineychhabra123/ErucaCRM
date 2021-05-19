using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;

namespace ErucaCRM.Business.Interfaces
{
    public interface IReportBusiness
    {
        List<LeadsInPipeLineModel> GetLeadsInPiplineByStages(int companyId, string dateFilterOption);
        List<AccountSaleRevenueModel> GetMonthWiseAccountSaleRevenue(int companyId);
        List<AccountSaleRevenueModel> GetAccountByTopHighestSaleRevenue(int companyId);
    }
}
