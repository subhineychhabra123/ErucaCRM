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
    public class LeadAuditRepository : BaseRepository<LeadAudit>
    {
        public LeadAuditRepository(IUnitOfWork unit)
            : base(unit)
        {

        }

        public List<ssp_GetLeadHistory_Result> GetLeadHistory(int leadId, int currentPage, Int16 pageSize, out int totalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<ssp_GetLeadHistory_Result> LeadHistory = entities.ssp_GetLeadHistory(leadId, currentPage, pageSize, objParam).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);
            return LeadHistory;
        }

        public List<SSP_GetLeadAudits_Result> GetRefreshAudits(int userId, int companyId, string TagName, string LeadName)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;

            List<SSP_GetLeadAudits_Result> result = entities.SSP_GetLeadAudits(userId, companyId, TagName, LeadName).ToList();
            return result;
        }


        public List<SSP_GetLeadAuditsForHomeRecentActivites_Result> RecentActivities(int currentPage, int pageSize,int leadAuditId,bool isLoadMore,int companyId,int UserId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetLeadAuditsForHomeRecentActivites_Result> RecentActivities = entities.SSP_GetLeadAuditsForHomeRecentActivites(pageSize, currentPage, leadAuditId, isLoadMore, companyId, UserId).ToList();
            return RecentActivities;
        }
        public List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivitiesForEmail(int pageSize, int companyId, int UserId , ref int TotalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivities = entities.SSP_GetRecentActivitesForEmailNotification(pageSize, companyId, UserId, objParam).ToList();
            TotalRecords = Convert.ToInt32(objParam.Value);
            return RecentActivities;
        }
        //public List<SSP_GetAllRecentActivitesForEmailNotification_Result> GetAllRecentActivitiesForEmail(int currentPage, int pageSize, int leadAuditId, bool isLoadMore, int companyId, int UserId, bool loadAll, ref int TotalRecords)
        //{
        //    Entities entities = (Entities)this.UnitOfWork.Db;
        //    ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
        //    List<SSP_GetAllRecentActivitesForEmailNotification_Result> RecentActivities = entities.SSP_GetAllRecentActivitesForEmailNotification(pageSize, currentPage, leadAuditId, isLoadMore, companyId, UserId, loadAll, objParam).ToList();
        //    TotalRecords = Convert.ToInt32(objParam.Value);
        //    return RecentActivities;
        //}
        public List<ssp_GetLeadHistoryChartDetails_Result> GetLeadHistoryChartDetails(int leadId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;

            List<ssp_GetLeadHistoryChartDetails_Result> LeadHistoryChartDetails = entities.ssp_GetLeadHistoryChartDetails(leadId).ToList();

            return LeadHistoryChartDetails;
        }
        public List<SSP_DasboardAnalyticData_Result> GetDashboardData(int CompanyId,string Interval)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;

            List<SSP_DasboardAnalyticData_Result> GetDashboardData = entities.SSP_DasboardAnalyticData(CompanyId,Interval).ToList();

            return GetDashboardData;
        }

    

        public int GetNotification(int pageSize, int companyId, int UserId,ref int MaxLeadAuditId, bool UpdateNotification,ref int TotalRecords )
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            ObjectParameter objParam1 = new ObjectParameter("MaxLeadAuditId", MaxLeadAuditId);
           int data = entities.SSP_Notifications(pageSize, companyId, UserId, objParam1, UpdateNotification, objParam);           
            TotalRecords = Convert.ToInt32(objParam.Value);
            MaxLeadAuditId = Convert.ToInt32(objParam1.Value);
            return data;
        }
    
    }
}
