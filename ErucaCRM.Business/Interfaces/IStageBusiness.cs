using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
using ErucaCRM.Utility.WebClasses;

namespace ErucaCRM.Business.Interfaces
{
    public interface IStageBusiness
    {
        List<StageModel> GetStages(int CompanyId);
        StageModel GetStage(int stageId);
        bool ChangeLeadStage(int leadId, int FromStage, int ToStage, int CompanyId);
        StageModel GetIntialStage(int CompanyId);
        string SaveStage(StageModel stageModel);
        bool UpdateStageOrder(List<StageSort> SortArray);
        bool DeleteStage(int OldStageId, int NewStageId);
        StageModel GetFinalStage(int companyId);
    }
}