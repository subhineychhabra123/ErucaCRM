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
    public class LeadSourceBusiness : ILeadSourceBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly LeadSourceRepository leadSourceRepository;
        public LeadSourceBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leadSourceRepository = new LeadSourceRepository(unitOfWork);
        }
        public List<LeadSourceModel> GetAllLeadSources()
        {
            List<LeadSourceModel> leadSourceListModel = new List<LeadSourceModel>();
            List<LeadSource> leadSourceList = leadSourceRepository.GetAll().ToList();          
            AutoMapper.Mapper.Map(leadSourceList, leadSourceListModel);
            return leadSourceListModel;
        }
        public List<LeadSourceModel> GetLeadSourceDropdownList()
        {
            List<LeadSourceModel> leadSourceModelList = new List<LeadSourceModel>();  
            List<LeadSource> leadSourceList = leadSourceRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(leadSourceList, leadSourceModelList);
            leadSourceModelList.Insert(0, new LeadSourceModel { LeadSourceId = 0, LeadSourceName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return leadSourceModelList;
        }

    }
}
