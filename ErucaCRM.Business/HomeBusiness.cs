using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System.Transactions;
using System.Collections.Generic;
using System.Linq;

namespace ErucaCRM.Business
{
    public class HomeBusiness : IHomeBusiness
    {
        private readonly ILeadAuditBusiness leadAuditBusiness;
        private readonly IUnitOfWork unitOfWork;
        public HomeBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;

            leadAuditBusiness = new LeadAuditBusiness(unitOfWork);
        }
        public List<HomeModel> GetRecentActivitiesForHome(int pageSize, int currentPage,int leadAuditId,bool isLoadMore,int companyId,int UserId)
        {
            List<HomeModel> homeModelList = new List<HomeModel>();
            List<SSP_GetLeadAuditsForHomeRecentActivites_Result> RecentActivities = new List<SSP_GetLeadAuditsForHomeRecentActivites_Result>();
            RecentActivities = leadAuditBusiness.GetLeadsForHomeRecentActivities(currentPage, pageSize, leadAuditId,isLoadMore,companyId,UserId);
           AutoMapper.Mapper.Map(RecentActivities, homeModelList);
            return homeModelList;
        }
       

        public HomeModel GetDashboardData(int companyId, string Interval)
        {
            HomeModel homeModelobj = new HomeModel();
            List<SSP_DasboardAnalyticData_Result> dashBoardAnalyticsData = new List<SSP_DasboardAnalyticData_Result>();
            dashBoardAnalyticsData = leadAuditBusiness.GetDashboardData(companyId, Interval);
           SSP_DasboardAnalyticData_Result objDashboardanalyticsdata=dashBoardAnalyticsData.FirstOrDefault();

           AutoMapper.Mapper.Map(objDashboardanalyticsdata, homeModelobj);
           return homeModelobj;
        }

        public int GetNotification(int pageSize,  int companyId, int UserId,ref int MaxLeadAuditId,bool UpdateNotification,ref int TotalRecords)
        {
            List<HomeModel> homeModelList = new List<HomeModel>();
          //  List<SSP_Notifications_Result> Notification = new List<SSP_Notifications_Result>();
           int Notification = leadAuditBusiness.GetNotification(pageSize, companyId, UserId, ref MaxLeadAuditId, UpdateNotification,ref TotalRecords);
           // AutoMapper.Mapper.Map(Notification, homeModelList);
           return Notification;
        }

    }
}
