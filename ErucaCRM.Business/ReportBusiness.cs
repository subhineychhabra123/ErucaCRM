using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;

namespace ErucaCRM.Business
{
    public class ReportBusiness : IReportBusiness
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly LeadRepository leadRepository;

        public ReportBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leadRepository = new LeadRepository(unitOfWork);

        }

        public List<LeadsInPipeLineModel> GetLeadsInPiplineByStages(int companyId, string dateFilterOption)
        {
            List<LeadsInPipeLineModel> leadsInPipeLineModel = new List<LeadsInPipeLineModel>();

            List<ssp_GetLeadsInPipeLine_Result> listLeadsInPipeLine_Result = leadRepository.GetLeadsInPiplineByStages(companyId, dateFilterOption);
            AutoMapper.Mapper.Map(listLeadsInPipeLine_Result, leadsInPipeLineModel);
            return leadsInPipeLineModel;
        }




        public List<AccountSaleRevenueModel> GetMonthWiseAccountSaleRevenue(int companyId)
        {
            List<AccountSaleRevenueModel> accountSaleRevenueModel = new List<AccountSaleRevenueModel>();

            List<ssp_GetMonthWiseAccountSaleRevenue_Result> listssp_GetMonthWiseAccountSaleRevenue_Result = leadRepository.GetMonthWiseAccountSaleRevenue(companyId);

            AutoMapper.Mapper.Map(listssp_GetMonthWiseAccountSaleRevenue_Result, accountSaleRevenueModel);
            return accountSaleRevenueModel;
        }

        public List<AccountSaleRevenueModel> GetAccountByTopHighestSaleRevenue(int companyId)
        {
            List<AccountSaleRevenueModel> accountSaleRevenueModel = new List<AccountSaleRevenueModel>();

            List<ssp_GetAccountByTopHighestSaleRevenue_Result> listssp_GetAccountByTopHighestSaleRevenue_Result = leadRepository.GetAccountByTopHighestSaleRevenue(companyId);

            AutoMapper.Mapper.Map(listssp_GetAccountByTopHighestSaleRevenue_Result, accountSaleRevenueModel);
            return accountSaleRevenueModel;
        }

    }
}
