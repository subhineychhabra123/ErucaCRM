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
    public class AccountCaseBusiness : IAccountCaseBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AccountCaseRepository accountcaseRepository;
        private readonly AccountCaseRepository caseStatusRepository;
        // private readonly CaseTypeRespository caseTypeRepository;

        public AccountCaseBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            accountcaseRepository = new AccountCaseRepository(unitOfWork);
            caseStatusRepository = new AccountCaseRepository(unitOfWork);


        }

        public void Case(AccountCaseModel caseModel)
        {
            AccountCase cases = new ErucaCRM.Repository.AccountCase();
            AutoMapper.Mapper.Map(caseModel, cases);
            if (caseModel.AccountCaseId > 0)
                accountcaseRepository.Update(cases);
            else
                accountcaseRepository.Insert(cases);
        }

        public IList<AccountCaseModel> GetCaseStatus()
        {
            IList<AccountCaseModel> caseModelList = new List<AccountCaseModel>();
            IList<AccountCase> caseList = caseStatusRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(caseList, caseModelList);
            return caseModelList;
        }

        public AccountCaseModel GetCaseByCaseId(int caseId)
        {
            AccountCaseModel caseModel = new AccountCaseModel();
            AccountCase cases = accountcaseRepository.GetAll(t => t.AccountCaseId == caseId && t.RecordDeleted == false).FirstOrDefault();
            AutoMapper.Mapper.Map(cases, caseModel);
            return caseModel;
        }

        public String GetCaseOwnerName(int ownerId)
        {
            UserRepository userRepo = new UserRepository(unitOfWork);
            User owner = userRepo.GetAll(u => u.UserId == ownerId && u.RecordDeleted == false).FirstOrDefault();
            return owner.FirstName + " " + owner.LastName;

        }
        public AccountCaseModel CaseDetail(int caseId)
        {
            AccountCaseModel casemodel = null;
            AccountCase cases = accountcaseRepository.SingleOrDefault(u => u.AccountCaseId == caseId && u.RecordDeleted == false);
            casemodel = new AccountCaseModel();
            if (cases != null)
            {
                AutoMapper.Mapper.Map(cases, casemodel);
                //AutoMapper.Mapper.Map(cases.FileAttachments, casemodel.FileAttachmentModels);
            }
            return casemodel;
        }
        public AccountCaseModel GetCase(int caseId)
        {
            AccountCaseModel caseModel = new AccountCaseModel();
            AccountCase cases = accountcaseRepository.GetAll(t => t.AccountCaseId == caseId && t.RecordDeleted == false).FirstOrDefault();
            AutoMapper.Mapper.Map(cases, caseModel);
            return caseModel;
        }
        public IList<AccountCaseModel> GetCases(int accountId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<AccountCaseModel> listCaseModel = new List<AccountCaseModel>();
            List<AccountCase> listCases = new List<AccountCase>();
            totalRecords = accountcaseRepository.Count(x => x.AccountId == accountId && x.RecordDeleted == false);
            listCases = accountcaseRepository.GetPagedRecords(x => x.AccountId == accountId && x.RecordDeleted == false, y => y.Subject, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(listCases, listCaseModel);
            return listCaseModel;
        }
        public List<AccountCaseModel> GetAccountCasesByUserId(int userId, int currentPage, int pageSize, ref int totalRecords, string sortColumnName, string sortDir)
        {
            List<AccountCaseModel> listAccountCaseModel = new List<AccountCaseModel>();
            List<SSP_GetAccountCasesByUserId_Result> listAccountCases = accountcaseRepository.GetAccountCasesByUserId(userId, currentPage, pageSize, out totalRecords, sortColumnName, sortDir);
            AutoMapper.Mapper.Map(listAccountCases, listAccountCaseModel);
            return listAccountCaseModel;
        }

        public List<AccountCaseModel> GetDashBoardAccountCases(int companyId, int userId)
        {
            List<AccountCaseModel> listAccountCaseModel = new List<AccountCaseModel>();
            List<AccountCase> listAccountCases = accountcaseRepository.GetAll(x => x.Account.CompanyId == companyId && x.CaseOwnerId == userId && x.RecordDeleted == false).Select(c => new AccountCase { CreatedDate = c.CreatedDate, Account = c.Account, AccountCaseId = c.AccountCaseId, AccountId = c.AccountId, CaseMessageBoards = c.CaseMessageBoards, CaseNumber = c.CaseNumber, CaseOriginId = c.CaseOriginId, CaseOwnerId = c.CaseOwnerId, CaseTypeId = c.CaseTypeId, CreatedBy = c.CreatedBy, Description = c.Description, FileAttachments = c.FileAttachments, ModifiedBy = c.ModifiedBy, ModifiedDate = c.ModifiedDate, PriorityId = c.PriorityId, RecordDeleted = c.RecordDeleted, StatusId = c.StatusId, Subject = c.Subject, User = c.User }).OrderByDescending(p => p.AccountCaseId).Skip(0).Take(5).ToList();
            AutoMapper.Mapper.Map(listAccountCases, listAccountCaseModel);
            listAccountCaseModel.ForEach(c => c.TotalCaseMessageBoards = c.CaseMessageBoards.Count());
            return listAccountCaseModel;
        }

        public AccountCaseModel AccountCaseDetail(int caseId)
        {
            AccountCaseModel accountcasemodel = null;
            AccountCase accountcase = accountcaseRepository.SingleOrDefault(u => u.AccountCaseId == caseId && u.RecordDeleted == false);
            accountcasemodel = new AccountCaseModel();
            if (accountcase != null)
            {
                AutoMapper.Mapper.Map(accountcase, accountcasemodel);
                //AutoMapper.Mapper.Map(accountcase.FileAttachments, accountcasemodel.FileAttachmentModels);
            }
            return accountcasemodel;
        }

        public void AddUpdateCase(AccountCaseModel caseModel, int companyId)
        {
            AccountCase cases = new AccountCase();



            //if account id is 0 then account will be added
            if (caseModel.AccountCaseId == 0)
            {
                AutoMapper.Mapper.Map(caseModel, cases);
                cases.CreatedDate = DateTime.UtcNow;
                cases.RecordDeleted = false;
                cases.CaseNumber = caseModel.AccountId.ToString("00000");
                cases.CreatedBy = caseModel.CreatedBy;
                accountcaseRepository.Insert(cases);
                cases.CaseNumber = cases.CaseNumber + "-" + cases.AccountCaseId.ToString("00000");

                accountcaseRepository.Update(cases);

            }
            else   //if account id is not 0 then account will be updated
            {
                cases = accountcaseRepository.SingleOrDefault(x => x.AccountCaseId == caseModel.AccountCaseId);
                caseModel.CreatedDate = cases.CreatedDate;
                AutoMapper.Mapper.Map(caseModel, cases);
                cases.ModifiedBy = caseModel.ModifiedBy;
                cases.ModifiedDate = DateTime.UtcNow;
                accountcaseRepository.Update(cases);

            }
        }
        public bool DeleteCase(int caseId, int userId)
        {

            AccountCase cases = accountcaseRepository.SingleOrDefault(x => x.AccountCaseId == caseId && x.RecordDeleted == false);
            if (cases != null)
            {
                cases.RecordDeleted = true;

                cases.ModifiedBy = userId;
                cases.ModifiedDate = DateTime.UtcNow;
                accountcaseRepository.Update(cases);
                return true;
            }
            else
                return false;
        }

        public AccountCaseModel GetAccountCaseInfo(int accountCaseId)
        {
            AccountCaseModel accountCaseModel;//= accountcaseRepository.GetAll(x => x.AccountCaseId == accountCaseId).Select(y => new AccountCaseModel() { AccountCaseId = y.AccountCaseId, CaseNumber = y.CaseNumber, AccountId = y.AccountId.GetValueOrDefault(), AccountName = y.Account.AccountName, }).FirstOrDefault();
            AccountCase accountCase = accountcaseRepository.SingleOrDefault(x => x.AccountCaseId == accountCaseId && x.RecordDeleted == false);
            accountCaseModel = new AccountCaseModel();
            if (accountCase != null)
            {
                accountCaseModel.AccountCaseId = accountCase.AccountCaseId;
                accountCaseModel.CaseNumber = accountCase.CaseNumber;
                accountCaseModel.AccountId = accountCase.AccountId.GetValueOrDefault();
                accountCaseModel.AccountName = accountCase.Account.AccountName;
            }
            return accountCaseModel;

        }
        public bool DeleteAccountCase(int caseId, int accountId, int userId)
        {

            AccountCase taskitem = accountcaseRepository.SingleOrDefault(x => x.AccountCaseId == caseId && x.AccountId == accountId && x.RecordDeleted == false);
            if (taskitem != null)
            {

                taskitem.ModifiedDate = DateTime.UtcNow;
                taskitem.RecordDeleted = true;
                accountcaseRepository.Update(taskitem);
                return true;
            }
            else
                return false;
        }
    }



}
