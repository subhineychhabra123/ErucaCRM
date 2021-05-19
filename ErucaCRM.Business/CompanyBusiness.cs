using ErucaCRM.Business.Interfaces;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
using ErucaCRM.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business
{
    public class CompanyBusiness : ICompanyBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CompanyRepository companyRepository;
        private readonly UserRepository userRepository;
        private readonly RoleRepository roleRepository;

        public CompanyBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            companyRepository = new CompanyRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
            roleRepository = new RoleRepository(unitOfWork);
        }
        public List<CompanyModel> GetOrgnaisations(int currentPage, Boolean companyStatus, Int16 pageSize,int AdminCompanyId, ref long totalRecords)
        {
            List<CompanyModel> listCompanyModel = new List<CompanyModel>();
            List<Company> listCompany = new List<Company>();
            totalRecords = companyRepository.Count(x => x.RecordDeleted == false && (x.IsActive == companyStatus));
            listCompany = companyRepository.GetPagedRecords(x => x.IsActive == companyStatus &&x.CompanyId!=AdminCompanyId&& x.RecordDeleted == false, y => y.CompanyName, currentPage, pageSize).ToList();
            AutoMapper.Mapper.Map(listCompany, listCompanyModel);
            return listCompanyModel;

        }



        public bool UpdateCompanyStatus(int[] CompanyIds, Boolean userStatus, int adminUserID)
        {

            List<Company> _listCompany = companyRepository.GetAll(x => CompanyIds.Contains(x.CompanyId) && x.RecordDeleted == false).ToList();
            for (int i = 0; i < _listCompany.Count; i++)
            {
                _listCompany[i].IsActive = userStatus;
                _listCompany[i].ModifiedBy = adminUserID;
            }
            companyRepository.UpdateAll(_listCompany);
            return true;
        }

        public CompanyModel GetCompanyDetail(int CompanyId)
        {
            string CreatedBy = String.Empty;
            Company company = new Company();
            CompanyModel companyModel = new CompanyModel();
            company = companyRepository.SingleOrDefault(r => r.CompanyId == CompanyId);
            AutoMapper.Mapper.Map(company, companyModel);
            int RoleId = roleRepository.SingleOrDefault(r=>r.IsDefaultForRegisterdUser == true).RoleId;
            User user = new User();
            user = userRepository.SingleOrDefault(r => r.CompanyId == CompanyId && r.RoleId == RoleId);
            if (user != null)
            {
               
                CreatedBy = user.FirstName + " " + user.LastName;
                companyModel.CreatedBy = CreatedBy;
            }
                return companyModel;

        }
    }
}
