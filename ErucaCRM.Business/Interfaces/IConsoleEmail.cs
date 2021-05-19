using ErucaCRM.Domain;
using ErucaCRM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
    public interface ILeadNotifcationBusiness
    {
        List<int> GetAllActiveCompanies();
        List<LeadEmailNotificationModel> GetCompanyDataForEmail(int CompanyId);
        void SaveEmailNotificationDetail(LeadEmailNotificationModel leadModel);
        List<SSP_GetRecentActivitesForEmailNotification_Result> GetRecentActivitiesForNotification(int pageSize, int companyId, int UserId, ref int TotalRecords);
    }
}
