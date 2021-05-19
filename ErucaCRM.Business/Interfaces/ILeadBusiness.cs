using ErucaCRM.Domain;
using ErucaCRM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
    public interface ILeadBusiness
    {
        LeadModel AddLead(LeadModel leadModel);
        List<LeadModel> GetLeads(int userId, int companyId, int stageId, int tagId, string leadName, int currentPage, int pageSize, ref int totalRecords);
        LeadModel GetLeadDetail(int leadId);
        LeadModel GetLeadDetailForEdit(int leadId);
        void DeleteLead(LeadModel leadModel);
        LeadModel UpdateLead(LeadModel leadModel);
        List<LeadModel> GetLeadDropDownList(int companyId);
        IList<LeadModel> GetAllLeadByOwnerId(int companyId, int ownerId);
        LeadModel UpdateLead(int leadId, int stageId, int ratingId);
        LeadModel UpdateLeadStage(LeadModel leadModel);
        LeadModel UpdateLeadRating(int leadId, int ratingId, int ModifiedBy);
        List<LeadStagesJSONModel> GetLeadsByStageGroup(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords);
        List<GetLeadAnalyticData_Result> GetLeadAnalyticData(string Interval, int CompanyId);
        List<YearWiseLeadModel> GetWeekWiseLeadCount(int CompanyId);
        List<YearWiseLeadModel> GetMonthWiseLeadCount(int companyId);
        List<YearWiseLeadModel> GetYearWiseLeadCount(int companyId);
        LeadByStarRatingPercentageModel GetLeadsByStarRatingPercentage(int CompanyId);
        List<LeadModel> GetLeadsbyStageId(int userId, int companyId, int stageId, int currentPage, int tagId, string LeadName, int pageSize,ref int totalRecords);
        List<LeadModel> GetLeadsbyStageIdWeb(int userId, int companyId, int stageId, int currentPage, string tagName, string LeadName, int pageSize, bool IsLoadMore, int leadId, ref int totalRecords);
        List<LeadModel> GetLeadsWeb(int userId, int companyId, int stageId, string tagName, string leadName, int currentPage, int pageSize, ref int totalRecords);
        List<TagModel> GetLeadTags(int companyId);
        List<LeadModel> GetTaggedLead(int tagId, int companyId, int currentPage, Int16 pageSize, ref int totalRecords);
    }
      
}
