using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;

namespace ErucaCRM.Business.Interfaces
{
    public interface IPlanBusiness
    {
        bool AddUpdatePlan(PlanModel planModel);
        PlanModel GetPlanById(int planId);
        List<PlanModel> GetAllPlans();
        List<ModuleListModel> GetAllModules();
         PlanModuleModel GetPlanModulePermission(int planModuleId);
         bool AddUpdatePlanModulePermission(PlanModuleModel planModuleModel, int userId);
         bool IsDuplicateModuleInPlan(int planId, int moduleId, int planModuleId);
    }
}
