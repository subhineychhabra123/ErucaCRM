using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface ILeadAuditBusiness
    {
        void Add(LeadAuditModel leadAuditModel);
        void Update(LeadAuditModel leadAuditModel);
        List<LeadAuditModel> GetLeadAudits(int companyId);
        LeadAuditModel GetLeadAuditByLeadId(int LeadId);
        List<LeadAuditModel> GetAutoLeadsAudits(int companyId, int userId, string tagName, string LeadName);
        List<LeadAuditModel> GetLeadHistorybyLeadId(int leadId, int currentpage, Int16 pagesize, ref int totalrecords);
        List<LeadHistoryChartModel> GetLeadHistoryChartDetails(int leadId);
        List<SSP_GetLeadAuditsForHomeRecentActivites_Result> GetLeadsForHomeRecentActivities(int currentpage, int pagesize, int leadAuditId, bool isLoadMore, int companyId,int UserId);
        List<SSP_DasboardAnalyticData_Result> GetDashboardData(int CompanyId, string Interval);
        List<SSP_GetRecentActivitesForEmailNotification_Result>RecentActivitiesForEmail(int pageSize,  int companyId, int UserId, ref int TotalRecords);
     //   int GetNotification(int currentpage, int pagesize, int leadAuditId, bool isLoadMore, int companyId, int UserId,ref int MaxLeadAuditId, bool UpdateNotification, ref int TotalRecords);
        //List<HomeModel> GetAllRecentActivitiesForEmail(int currentPage, int pageSize, int leadAuditId, bool isLoadMore, int companyId, int UserId, bool loadAll, ref int TotalRecords);
        int GetNotification(int pageSize, int companyId, int UserId, ref int MaxLeadAuditId, bool UpdateNotification, ref int TotalRecords);

    }
}
