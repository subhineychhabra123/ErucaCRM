using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Transactions;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System.Linq;

namespace ErucaCRM.Repository
{
    public class LeadRepository : BaseRepository<Lead>
    {
        private FileAttachmentRepository fileAttachmentRepository;
        public LeadRepository(IUnitOfWork unit)
            : base(unit)
        {
            this.fileAttachmentRepository = new FileAttachmentRepository(unit);
        }

        public void DeleteLead(int leadId)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                this.fileAttachmentRepository.Delete(x => x.LeadId == leadId);
                this.Delete(x => x.LeadId == leadId);
                transactionScope.Complete();
            }
        }

        public List<Lead> GetLeadsByUserId(int userId, int companyId, int stageId, int currentPage, int tagId, string leadName, int pageSize, out int totalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<Lead> leads = entities.SSP_GetLeadsByUserIdWeb(userId, companyId, stageId, "", leadName, currentPage, pageSize, false, 0, objParam).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);
            return leads;
        }


        public List<Lead> GetLeadsByUserIdWeb(int userId, int companyId, int stageId, int currentPage, string tagName, string leadName, int pageSize, bool IsLoadMore, int leadId, out int totalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<Lead> leads = entities.SSP_GetLeadsByUserIdWeb(userId, companyId, stageId, tagName, leadName, currentPage, pageSize, IsLoadMore, leadId, objParam).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);
            return leads;
        }

        public List<GetLeadAnalyticData_Result> GetLeadAnalyticData(string Interval, int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<GetLeadAnalyticData_Result> data = new List<GetLeadAnalyticData_Result>();
            data = entities.GetLeadAnalyticData(companyId, Interval).ToList();

            return data;
        }

        public List<SSP_GetYearWiseLeadCount_Result> GetYearWiseLeadCount(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetYearWiseLeadCount_Result> data = new List<SSP_GetYearWiseLeadCount_Result>();
            data = entities.SSP_GetYearWiseLeadCount(companyId).ToList();
            return data;
        }
        public List<ssp_GetMonthWiseLeadCount_Result> GetMonthWiseLeadCount(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<ssp_GetMonthWiseLeadCount_Result> data = new List<ssp_GetMonthWiseLeadCount_Result>();
            data = entities.ssp_GetMonthWiseLeadCount(companyId).ToList();
            return data;
        }
        public List<ssp_GetWeekWiseLeadCount_Result> GetWeekWiseLeadCount(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<ssp_GetWeekWiseLeadCount_Result> data = new List<ssp_GetWeekWiseLeadCount_Result>();
            data = entities.ssp_GetWeekWiseLeadCount(companyId).ToList();
            return data;
        }

        public List<ssp_GetLeadsInPipeLine_Result> GetLeadsInPiplineByStages(int companyId, string dateFilterOption)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<ssp_GetLeadsInPipeLine_Result> data = new List<ssp_GetLeadsInPipeLine_Result>();
            data = entities.ssp_GetLeadsInPipeLine(companyId, dateFilterOption).ToList();
            return data;
        }

        public List<ssp_GetMonthWiseAccountSaleRevenue_Result> GetMonthWiseAccountSaleRevenue(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<ssp_GetMonthWiseAccountSaleRevenue_Result> data = new List<ssp_GetMonthWiseAccountSaleRevenue_Result>();
            data = entities.ssp_GetMonthWiseAccountSaleRevenue(companyId).ToList();
            return data;
        }

        public List<ssp_GetAccountByTopHighestSaleRevenue_Result> GetAccountByTopHighestSaleRevenue(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<ssp_GetAccountByTopHighestSaleRevenue_Result> data = new List<ssp_GetAccountByTopHighestSaleRevenue_Result>();
            data = entities.ssp_GetAccountByTopHighestSaleRevenue(companyId).ToList();
            return data;
        }

        public List<SSP_LeadsByRatingAndStages_Result> GetLeadsByStarRatingPercentage(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_LeadsByRatingAndStages_Result> data = new List<SSP_LeadsByRatingAndStages_Result>();
            data = entities.SSP_LeadsByRatingAndStages(companyId).ToList();
            return data;
        }


        public List<SSP_LeadsListbyUserId_Result> GetLeadsListByUserId(int userId, int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_LeadsListbyUserId_Result> leads = entities.SSP_LeadsListbyUserId(userId,companyId,1,1).ToList();
            return leads;
        }


        public List<SSP_GetCompanyUsersInfoWithLeads_Result> GetCompanyDelayedLeadsInStage(int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetCompanyUsersInfoWithLeads_Result> data = new List<SSP_GetCompanyUsersInfoWithLeads_Result>();
            data = entities.SSP_GetCompanyUsersInfoWithLeads(companyId).ToList();
            return data;
        }
        /// <summary>
        /// for stageLeadDuration 
        /// </summary>
        /// <param name="stageId"></param>
        /// <returns>stageLeadDuration</returns>
        public int GetStageLeadDuration(int stageId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            int stageLeadDuration = entities.Stages.Where(x=>x.StageId == stageId).SingleOrDefault().StageLeadDuration.Value;
            return stageLeadDuration;
        }

    }
}