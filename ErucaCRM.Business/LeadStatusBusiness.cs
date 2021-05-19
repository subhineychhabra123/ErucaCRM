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
    public class LeadStatusBusiness : ILeadStatusBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly LeadStatusRepository leadStatusRepository;
        public LeadStatusBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leadStatusRepository = new LeadStatusRepository(unitOfWork);
        }

        public List<LeadStatusModel> GetLeadStatusDropdownList()
        {
            List<LeadStatusModel> leadStatusModelList = new List<LeadStatusModel>();
            List<LeadStatu> leadSourceList = leadStatusRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(leadSourceList, leadStatusModelList);
            leadStatusModelList.Insert(0, new LeadStatusModel { LeadStatusId = 0, LeadStatusName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return leadStatusModelList;
        }

    }
}
