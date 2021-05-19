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
    public class CaseMessageBoardBusiness : ICaseMessageBoardBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CaseMessageBoardRepository caseMessageBoardRepository;

        public CaseMessageBoardBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            caseMessageBoardRepository = new CaseMessageBoardRepository(unitOfWork);
        }
        public CaseMessageBoardModel Add(CaseMessageBoardModel caseMessageBoardModel)
        {
            CaseMessageBoard caseMessageBoard = new CaseMessageBoard();
            AutoMapper.Mapper.Map(caseMessageBoardModel, caseMessageBoard);
            caseMessageBoard.CreatedDate = DateTime.UtcNow;
            caseMessageBoard = caseMessageBoardRepository.Insert(caseMessageBoard);
            AutoMapper.Mapper.Map(caseMessageBoard, caseMessageBoardModel);
            return caseMessageBoardModel;
        }
        public bool Update(CaseMessageBoardModel caseMessageBoardModel)
        {
            CaseMessageBoard caseMessageBoard = caseMessageBoardRepository.SingleOrDefault(x => x.CaseMessageBoardId == caseMessageBoardModel.CaseMessageBoardId);
            if (caseMessageBoard != null)
            {
                AutoMapper.Mapper.Map(caseMessageBoardModel, caseMessageBoard);
                //caseMessageBoard.FileAttachments = new List< FileAttachment>();
                caseMessageBoardRepository.Update(caseMessageBoard);              
            }
            return true;
        }
        public List<CaseMessageBoardModel> GetCaseMessageBoardMessages(int accountCaseId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<CaseMessageBoardModel> caseMessageBoardModelList = new List<CaseMessageBoardModel>();
            List<CaseMessageBoard> caseMessageBoardList = new List<CaseMessageBoard>();
            totalRecords = caseMessageBoardRepository.Count(x => x.AccountCaseId == accountCaseId );
            caseMessageBoardList = caseMessageBoardRepository.GetPagedRecords(x => x.AccountCaseId == accountCaseId , y => y.CaseMessageBoardId, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(caseMessageBoardList, caseMessageBoardModelList);
            return caseMessageBoardModelList;
        }
    }
}
