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
using ErucaCRM.Utility.WebClasses;
namespace ErucaCRM.Business
{
    public class StageBusiness : IStageBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly StageRepository stageRepository;
        public StageBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            stageRepository = new StageRepository(unitOfWork);
        }
        StageModel IStageBusiness.GetStage(int stageId)
        {
            StageModel stageModel = new StageModel();
            Stage stage = stageRepository.SingleOrDefault(x => x.StageId == stageId);
            AutoMapper.Mapper.Map(stage, stageModel);
            return stageModel;
        }

        public string SaveStage(StageModel stageModel)
        {
            Stage stage = new Stage();
            bool ifStageAlreadyExists = stageRepository.Count(c => c.CompanyId == stageModel.CompanyId && c.StageName == stageModel.StageName && c.StageId != stageModel.StageId && c.RecordDeleted == false) > 0;
            if (ifStageAlreadyExists)
                return Enums.ResponseResult.NameExist.ToString();
            if (stageModel.StageId != 0)
            {
                StageModel stagemodeledit = new StageModel();
                stage = stageRepository.SingleOrDefault(c => c.StageId == stageModel.StageId);
                stageModel.IsInitialStage = stage.IsInitialStage;
                stageModel.IsLastStage = stage.IsLastStage;
                AutoMapper.Mapper.Map(stageModel, stage);
                stage.ModifiedDate = DateTime.UtcNow;
                stageRepository.Update(stage);
            }
            else
            {
                AutoMapper.Mapper.Map(stageModel, stage);
                stage.CreatedDate = DateTime.UtcNow;
                stageRepository.Insert(stage);
            }
            return Enums.ResponseResult.Success.ToString();
        }

        public List<StageModel> GetStages(int CompanyId)
        {
            List<StageModel> stageModelList = new List<StageModel>();
            List<Stage> stageList = stageRepository.GetAll(r => r.CompanyId == CompanyId && r.RecordDeleted == false).OrderBy(n => n.StageOrder).ToList();
            AutoMapper.Mapper.Map(stageList, stageModelList);
            return stageModelList;
        }

        public bool UpdateStageOrder(List<StageSort> SortArray)
        {
            int[] arrStageIds = SortArray.Select(x => x.StageId).ToArray();
            List<Stage> stageList = stageRepository.GetAll(r => arrStageIds.Contains(r.StageId)).ToList();

            for (int i = 0; i < SortArray.Count(); i++)
            {

                for (int j = 0; j < stageList.Count(); j++)
                {
                    if (stageList[j].StageId == SortArray[i].StageId)
                    {
                        stageList[j].StageOrder = SortArray[i].StageIndex;
                        break;
                    }
                }
            }
            stageRepository.UpdateAll(stageList);
            return true;
        }
        public StageModel GetIntialStage(int CompanyId)
        {
            StageModel stageModel = new StageModel();
            Stage stage = stageRepository.SingleOrDefault(r => r.CompanyId == CompanyId && r.IsInitialStage == true);
            AutoMapper.Mapper.Map(stage, stageModel);
            return stageModel;
        }
        public StageModel GetFinalStage(int companyId)
        {
            StageModel stageModel = new StageModel();
            Stage stage = stageRepository.SingleOrDefault(r => r.IsLastStage == true && r.CompanyId == companyId && r.RecordDeleted == false);
            AutoMapper.Mapper.Map(stage, stageModel);
            return stageModel;
        }
        public bool DeleteStage(int OldStageId, int NewStageId)
        {
            bool result = false;
            result = stageRepository.DeleteStage(OldStageId, NewStageId);
            return result;
        }

        public bool ChangeLeadStage(int leadId, int fromStageId, int toStageId, int CompanyId)
        {

            return true;


        }
    }

}
