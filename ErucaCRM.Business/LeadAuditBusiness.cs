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
    public class LeadAuditBusiness : ILeadAuditBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly LeadAuditRepository leadAuditRepository;
        public LeadAuditBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leadAuditRepository = new LeadAuditRepository(unitOfWork);
        }
        void ILeadAuditBusiness.Add(LeadAuditModel leadAuditModel)
        {
            LeadAudit leadAudit = new LeadAudit();
            AutoMapper.Mapper.Map(leadAuditModel, leadAudit);

            if (leadAudit.RatingId == 0)
                leadAudit.RatingId = null;
           
            leadAuditRepository.Insert(leadAudit);
        }

        void ILeadAuditBusiness.Update(LeadAuditModel leadAuditModel)
        {
            LeadAudit leadAudit = new LeadAudit();
            leadAudit = leadAuditRepository.GetAll(c => c.LeadId == leadAuditModel.LeadId).LastOrDefault();
            AutoMapper.Mapper.Map(leadAuditModel, leadAudit);
            leadAuditRepository.Update(leadAudit);
        }

        public List<LeadAuditModel> GetLeadAudits(int companyId)
        {
            List<LeadAuditModel> leadAuditModelList = new List<LeadAuditModel>();
            List<LeadAudit> leadAuditList = leadAuditRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(leadAuditList, leadAuditModelList);
            return leadAuditModelList;
        }


        public LeadAuditModel GetLeadAuditByLeadId(int LeadId)
        {
            LeadAudit leadAudit = new LeadAudit();
            LeadAuditModel leadAuditModel = new LeadAuditModel();
            leadAudit = leadAuditRepository.GetAll().Where(c => c.LeadId == LeadId).OrderBy(x => x.LeadAuditId).LastOrDefault();
            AutoMapper.Mapper.Map(leadAudit, leadAuditModel);
            return leadAuditModel;
        }
        public List<LeadAuditModel> GetAutoLeadsAudits(int companyId,int userId,string tagName,string LeadName)
        {  
        
            List<LeadAuditModel> leadAuditModellist = new List<LeadAuditModel>();
            List<SSP_GetLeadAudits_Result> result = leadAuditRepository.GetRefreshAudits(userId, companyId, tagName, LeadName);
           //leadAuditModellist = leadAuditRepository.GetAll(c => c.CompanyId == companyId && c.CreatedDate >= previousTime && c.CreatedBy!=userId).Select(x => new LeadAuditModel { StageId = x.StageId, ActivityType = x.ActivityType, StageName = x.Stage.StageName, RatingIcon = x.Rating == null ? "" : x.Rating.Icons, LeadId = x.Lead.LeadId, IsClosedWin = x.Lead.IsClosedWin, Title = x.Lead.Title, RatingId = x.RatingId }).OrderBy(x => x.LeadAuditId).ToList();
           AutoMapper.Mapper.Map(result, leadAuditModellist);
            return leadAuditModellist;
        }


        public List<LeadAuditModel> GetLeadHistorybyLeadId(int leadId, int currentpage, Int16 pagesize, ref int totalRecords)
        {
            List<ssp_GetLeadHistory_Result> GetLeadHistory_Result = new List<ssp_GetLeadHistory_Result>();
            List<LeadAuditModel> leadAuditModel = new List<LeadAuditModel>();
            GetLeadHistory_Result = leadAuditRepository.GetLeadHistory(leadId, currentpage, pagesize, out totalRecords);
            AutoMapper.Mapper.Map(GetLeadHistory_Result, leadAuditModel);


            return leadAuditModel;
        }

        public List<SSP_GetLeadAuditsForHomeRecentActivites_Result> GetLeadsForHomeRecentActivities(int currentpage, int pagesize, int leadAuditId, bool isLoadMore, int companyId,int UserId)
        {
            List<SSP_GetLeadAuditsForHomeRecentActivites_Result> RecentActivitiesResult = new List<SSP_GetLeadAuditsForHomeRecentActivites_Result>();
            RecentActivitiesResult = leadAuditRepository.RecentActivities(currentpage, pagesize, leadAuditId, isLoadMore, companyId,UserId);
            return RecentActivitiesResult;
        }
        public List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivitiesForEmail(int pageSize, int companyId, int UserId, ref int TotalRecords)
        {
            List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivitiesResult = new List<SSP_GetRecentActivitesForEmailNotification_Result>();
            RecentActivitiesResult=leadAuditRepository.RecentActivitiesForEmail(pageSize, companyId,  UserId, ref TotalRecords);
            return RecentActivitiesResult;
        }
        //public List<HomeModel> GetAllRecentActivitiesForEmail(int currentPage, int pageSize, int leadAuditId, bool isLoadMore, int companyId, int UserId, bool loadAll, ref int TotalRecords)
        //{
        //    List<HomeModel> listHomeModel = new List<HomeModel>();

        //    List<SSP_GetAllRecentActivitesForEmailNotification_Result> RecentActivitiesResult = new List<SSP_GetAllRecentActivitesForEmailNotification_Result>();
        //    RecentActivitiesResult = leadAuditRepository.GetAllRecentActivitiesForEmail(currentPage, pageSize, leadAuditId, isLoadMore, companyId, UserId, loadAll, ref TotalRecords);
        //    AutoMapper.Mapper.Map(RecentActivitiesResult, listHomeModel);
        //    return listHomeModel;
        //}
   

        public List<LeadHistoryChartModel> GetLeadHistoryChartDetails(int leadId)
        {
            List<ssp_GetLeadHistoryChartDetails_Result> GetLeadHistoryChartDetails_Result = new List<ssp_GetLeadHistoryChartDetails_Result>();
            List<LeadHistoryChartModel> leadHistoryChartModel = new List<LeadHistoryChartModel>();
            GetLeadHistoryChartDetails_Result = leadAuditRepository.GetLeadHistoryChartDetails(leadId);

            string[] Stages = GetLeadHistoryChartDetails_Result.Select(x => x.StageName).Distinct().ToArray();
            string Duration = "";

            for (int i = 0; i < Stages.Count(); i++)
            {

                ssp_GetLeadHistoryChartDetails_Result objssp_GetLeadHistoryChartDetails_Result = new ssp_GetLeadHistoryChartDetails_Result();

                LeadHistoryChartModel objLeadHistoryChartModel = new LeadHistoryChartModel();

                int StageDays = 0;
                int StageHours = 0;
                int StageMinutes = 0;

                List<ssp_GetLeadHistoryChartDetails_Result> StageList = GetLeadHistoryChartDetails_Result.Where(x => x.StageName == Stages[i]).ToList();

                if (StageList.Count > 0)
                {
                    objLeadHistoryChartModel = new LeadHistoryChartModel();

                    objLeadHistoryChartModel.StageId = StageList[0].StageId;
                    objLeadHistoryChartModel.StageName = StageList[0].StageName;
                    objLeadHistoryChartModel.TotalNumberOfMinutes = StageList[0].TotalNumberOfMinutes;
                    objLeadHistoryChartModel.Duration = "";

                    objLeadHistoryChartModel.NumberOfMinutes = (int)StageList.Sum(x => x.NumberOfMinutes);
                    StageDays = (int)StageList.Sum(x => x.StageDays);
                    StageHours = (int)StageList.Sum(x => x.StageHours);
                    StageMinutes = (int)StageList.Sum(x => x.StageMinutes);


                    if (StageHours >= 24)
                    {
                        StageHours = StageHours - 24;
                        StageDays = StageDays + 1;
                    }

                    if (StageMinutes >= 60)
                    {
                        StageMinutes = StageMinutes - 60;
                        StageHours = StageHours + 1;

                    }

                    if (StageDays > 0)
                        Duration = StageDays + " Day";

                    if (StageHours > 0)

                        Duration = (Duration != "" ? Duration + ", " : "") + (+StageHours + " Hour ");

                    if (StageMinutes > 0)

                        Duration = (Duration != "" ? Duration + ", " : "") + StageMinutes + " Minute ";



                    if (Duration != "")
                    {
                        objLeadHistoryChartModel.Duration = Duration;
                        Duration = "";
                    }

                    leadHistoryChartModel.Add(objLeadHistoryChartModel);
                }

            }

            //   AutoMapper.Mapper.Map(GetLeadHistoryChartDetails_Result, leadHistoryChartModel);


            return leadHistoryChartModel;
        }
        public List<SSP_DasboardAnalyticData_Result> GetDashboardData(int CompanyId, string Interval)
        {
            List<SSP_DasboardAnalyticData_Result>  GetDashboardData_Result = leadAuditRepository.GetDashboardData(CompanyId, Interval);
            return GetDashboardData_Result;
        }

      
        public int GetNotification(int pageSize, int companyId, int UserId, ref int MaxLeadAuditId, bool UpdateNotification, ref int TotalRecords)
        {
            int leadAuditNotifications = leadAuditRepository.GetNotification(pageSize, companyId, UserId, ref MaxLeadAuditId, UpdateNotification, ref TotalRecords);
            return leadAuditNotifications;
        }
    }

}
