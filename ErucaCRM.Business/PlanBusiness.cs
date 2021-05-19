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
using System.Transactions;

namespace ErucaCRM.Business
{
    public class PlanBusiness : IPlanBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PlanRepository planRepository;
        private readonly ModuleRepository moduleRepository;
        private readonly PlanModuleRepository planModuleRepository;
        public PlanBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            planRepository = new PlanRepository(unitOfWork);
            moduleRepository = new ModuleRepository(unitOfWork);
            planModuleRepository = new PlanModuleRepository(unitOfWork);
        }

        public bool AddUpdatePlan(PlanModel planModel)
        {
            Plan objPlan = new Plan();

            if (planModel.PlanId == 0)
            {
                AutoMapper.Mapper.Map(planModel, objPlan);

                planRepository.Insert(objPlan);

            }
            else
            {
                objPlan = planRepository.SingleOrDefault(x => x.PlanId == planModel.PlanId);
                if (objPlan != null)
                {
                    bool planchanged = false;

                    if (planModel.PlanName.Trim().ToLower() != objPlan.PlanName.Trim().ToLower())
                    {

                        planchanged = true;
                    }
                    else if (!planchanged && planModel.Price != objPlan.Price)
                        planchanged = true;

                    else if (!planchanged && planModel.NoOfUsers != objPlan.NoOfUsers)
                        planchanged = true;

                    if (planchanged && objPlan.CompanyPlans.Count > 0)
                    {
                        UpdatePlanWithNewPlanId(planModel);

                        //DeaActivate the old plan 

                        objPlan.Active = false;
                        objPlan.ModifiedBy = planModel.ModifiedBy;
                        objPlan.ModifiedDate = DateTime.UtcNow;
                    }
                    else
                    {

                        //DeaActivate the old plan 
                        objPlan.PlanName = planModel.PlanName;
                        objPlan.Description = planModel.Description;
                        objPlan.Price = planModel.Price;
                        objPlan.NoOfUsers = planModel.NoOfUsers;
                        objPlan.Active = planModel.Active;
                        objPlan.ModifiedBy = planModel.ModifiedBy;
                        objPlan.ModifiedDate = DateTime.UtcNow;

                        planRepository.Update(objPlan);
                    }
                }


            }

            return true;

        }

        private int UpdatePlanWithNewPlanId(PlanModel existingPlanModel)
        {
            Plan objNewPlan = new Plan();


            objNewPlan.PlanName = existingPlanModel.PlanName;
            objNewPlan.Price = existingPlanModel.Price;
            objNewPlan.NoOfUsers = existingPlanModel.NoOfUsers;

            objNewPlan.CreatedBy = existingPlanModel.CreatedBy;
            objNewPlan.CreatedDate = DateTime.UtcNow;
            objNewPlan.ModifiedBy = existingPlanModel.ModifiedBy;
            objNewPlan.ModifiedDate = DateTime.UtcNow;
            objNewPlan.PlanModules = new List<PlanModule>();

            if (existingPlanModel.PlanModulesModel != null)
            {
                List<PlanModuleModel> listPlanModulesModel = existingPlanModel.PlanModulesModel.ToList();

                for (int i = 0; i < listPlanModulesModel.Count(); i++)
                {
                    PlanModule objPlanModule = new PlanModule();
                    objPlanModule.PlanId = listPlanModulesModel[i].PlanId;
                    objPlanModule.ModuleId = listPlanModulesModel[i].ModuleId;
                    objPlanModule.HasPermission = listPlanModulesModel[i].HasPermission;
                    objPlanModule.HasPermissionAfterTrail = listPlanModulesModel[i].HasPermissionAfterTrail;

                    objNewPlan.PlanModules.Add(objPlanModule);
                }

            }

            planRepository.Insert(objNewPlan);
            return objNewPlan.PlanId;


        }

        public bool IsDuplicateModuleInPlan(int planId, int moduleId, int planModuleId)
        {
            if (planModuleRepository.Count(x => x.PlanId == planId && x.ModuleId == moduleId &&x.PlanModuleId!=planModuleId) > 0)
               return true;
           else
                return false;

        }

        public bool AddUpdatePlanModulePermission(PlanModuleModel planModuleModel, int userId)
        {
            PlanModule planModule = null;
            if (planModuleModel.PlanModuleId == 0)
            {
                Plan objPlan = planRepository.SingleOrDefault(x => x.PlanId == planModuleModel.PlanId);

                planModule = new PlanModule();

                if (objPlan.CompanyPlans.Count > 0)
                {
                    PlanModel objPlanModel = new PlanModel();

                    AutoMapper.Mapper.Map(objPlan, objPlanModel);

                    int newPlanId = UpdatePlanWithNewPlanId(objPlanModel);
                    planModule.PlanId = newPlanId;

                }



                AutoMapper.Mapper.Map(planModuleModel, planModule);

                planModuleRepository.Insert(planModule);

            }
            else
            {
                planModule = planModuleRepository.SingleOrDefault(x => x.PlanModuleId == planModuleModel.PlanModuleId);


                bool planchanged = false;


                Plan objPlan = planRepository.SingleOrDefault(x => x.PlanId == planModuleModel.PlanId);


                if (planModule.ModuleId != planModuleModel.ModuleId)
                    planchanged = true;
                else if (planModule.HasPermission != planModuleModel.HasPermission)
                    planchanged = true;
                else if (planModule.HasPermissionAfterTrail = planModuleModel.HasPermissionAfterTrail)
                    planchanged = true;

                if (planchanged && objPlan.CompanyPlans.Count > 0)
                {
                    PlanModel objPlanModel = new PlanModel();

                    AutoMapper.Mapper.Map(objPlan, objPlanModel);

                    int newPlanId = UpdatePlanWithNewPlanId(objPlanModel);

                    planModule.PlanId = newPlanId;

                    //Deactivate Existig Plan

                    objPlan.Active = false;
                    objPlan.ModifiedBy = userId;
                    objPlan.ModifiedDate = DateTime.UtcNow;


                }


                planModule.ModuleId = planModuleModel.ModuleId;
                planModule.HasPermission = planModuleModel.HasPermission;
                planModule.HasPermissionAfterTrail = planModuleModel.HasPermissionAfterTrail;

                planModuleRepository.Update(planModule);
            }

            return true;


        }

        public PlanModuleModel GetPlanModulePermission(int planModuleId)
        {
            PlanModule planModule = planModuleRepository.SingleOrDefault(x => x.PlanModuleId == planModuleId);
            PlanModuleModel planModuleModel = new PlanModuleModel();

            AutoMapper.Mapper.Map(planModule, planModuleModel);

            return planModuleModel;
        }

        public List<ModuleListModel> GetAllModules()
        {
            List<ModuleListModel> listPlanModules = moduleRepository.GetAll(x => x.IsPlanModule == true).Select(y => new ModuleListModel { ModuleId = y.ModuleId, ModuleName = y.ModuleName }).ToList();

            return listPlanModules;

        }

        public List<PlanModel> GetAllPlans()
        {
            List<PlanModel> planListModel = new List<PlanModel>();
            List<Plan> listPlan = planRepository.GetAll(x => x.RecordDeleted == false).OrderBy(y => y.ModifiedDate).ToList();


            AutoMapper.Mapper.Map(listPlan, planListModel);



            return planListModel;
        }

        public PlanModel GetPlanById(int planId)
        {
            PlanModel planModel = new PlanModel();
            Plan objPlan = planRepository.SingleOrDefault(x => x.PlanId == planId);

            if (objPlan != null)
            {
                AutoMapper.Mapper.Map(objPlan, planModel);


            }
            return planModel;

        }
    }
}
