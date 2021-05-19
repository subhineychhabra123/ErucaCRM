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
    public class LeadNotifcationBusiness : ILeadNotifcationBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CompanyRepository companyRepository;
        private readonly LeadRepository leadRepository;
        private readonly LeadEmailNotificationRepository leadEmailRepository;
        private readonly ILeadAuditBusiness leadAuditBusiness;
        private readonly LeadAuditRepository leadAuditRepository;
      
        public LeadNotifcationBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            companyRepository = new CompanyRepository(unitOfWork);
            leadRepository = new LeadRepository(unitOfWork);
            leadEmailRepository = new LeadEmailNotificationRepository(unitOfWork);
            leadAuditBusiness = new LeadAuditBusiness(unitOfWork);
            leadAuditRepository = new LeadAuditRepository(unitOfWork);
            
        }


        public List<int> GetAllActiveCompanies()
        {

            List<int> CompanyIds = companyRepository.GetAll(x => x.IsActive == true).Select(x => x.CompanyId).ToList();
            return CompanyIds;

        }

        public List<LeadEmailNotificationModel> GetCompanyDataForEmail(int CompanyId)
        {
            List<LeadEmailNotificationModel> objleadnotification = new List<LeadEmailNotificationModel>();
            List<SSP_GetCompanyUsersInfoWithLeads_Result> objgetcompanyuser = new List<SSP_GetCompanyUsersInfoWithLeads_Result>();
            objgetcompanyuser = leadRepository.GetCompanyDelayedLeadsInStage(CompanyId).ToList();
         
            AutoMapper.Mapper.CreateMap<SSP_GetCompanyUsersInfoWithLeads_Result, LeadEmailNotificationModel>();
            AutoMapper.Mapper.Map(objgetcompanyuser, objleadnotification);
            return objleadnotification;
        }

        /// <summary>
        /// For Saving Notification Email Data to Table-LeadEmailNotification 
        /// </summary>
        /// <param name="leadModel"></param>
        public void SaveEmailNotificationDetail(LeadEmailNotificationModel leadModel)
        {

            LeadEmailNotification leadEmail = new LeadEmailNotification();
            leadEmail.StartDate = leadModel.StartDate;
            leadEmail.EndDate = leadModel.EndDate;
            leadEmail.LogData = leadModel.LogData;        
            leadEmail.HasError = leadModel.HasError;            
            leadEmail.LogType = leadModel.LogType;
         
            leadEmailRepository.Insert(leadEmail);
        }
        public List<SSP_GetRecentActivitesForEmailNotification_Result> GetRecentActivitiesForNotification(int pageSize, int companyId, int UserId, ref int TotalRecords)
        {

            List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivities = new List<SSP_GetRecentActivitesForEmailNotification_Result>();
            RecentActivities = leadAuditBusiness.RecentActivitiesForEmail(pageSize, companyId, UserId, ref  TotalRecords);

            return RecentActivities;
        }
      
        //public void UpdateRecentActivitiesForNotification(List<string> leadAuditIds, List<SSP_GetRecentActivitesForEmailNotification_Result> RecentActivities,int userId)
        //{
        //    foreach (SSP_GetRecentActivitesForEmailNotification_Result RecentActivitie in RecentActivities)
        //    {
        //        RecentActivitieEmailNotification objRecentActivitieEmailNotification = new RecentActivitieEmailNotification();
        //        if (leadAuditIds.Contains(RecentActivitie.LeadAuditId.ToString()))
        //        {
        //            objRecentActivitieEmailNotification.IsDisplay = true;
        //            objRecentActivitieEmailNotification.LeadAuditId = RecentActivitie.LeadAuditId;
        //            objRecentActivitieEmailNotification.UserId = userId;
        //            objRecentActivitieEmailNotification.Date = DateTime.UtcNow;
        //            recentActivitieEmailNotificationRepository.Insert(objRecentActivitieEmailNotification);
                   
        //        }
        //        else

        //        {
        //            objRecentActivitieEmailNotification.IsDisplay = false;
        //            objRecentActivitieEmailNotification.LeadAuditId = RecentActivitie.LeadAuditId;
        //            objRecentActivitieEmailNotification.UserId = userId;
        //            objRecentActivitieEmailNotification.Date = DateTime.UtcNow;
        //            recentActivitieEmailNotificationRepository.Insert(objRecentActivitieEmailNotification);
        //        }
        //    }
        //}


        public int GetStageLeadDuration(int stageId)
        {
            int GetStageLeadDuration = leadRepository.GetStageLeadDuration(stageId);
            return GetStageLeadDuration;
        }
    
    }
}
