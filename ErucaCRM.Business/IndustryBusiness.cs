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
    public class IndustryBusiness : IIndustryBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IndustryRepository industryRepository;
        public IndustryBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            industryRepository = new IndustryRepository(unitOfWork);
        }

        public List<IndustryModel> GetAllIndustries()
        {
            List<IndustryModel> industryListModel = new List<IndustryModel>();
            List<Industry> industryList = industryRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(industryList, industryListModel);
            return industryListModel;
        }
        public List<IndustryModel> GetAllIndustriesDropdownList()
        {
            List<IndustryModel> industryListModel = new List<IndustryModel>();
            List<Industry> industryList = industryRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(industryList, industryListModel);
            industryListModel.Insert(0, new IndustryModel { IndustryId = 0, IndustryName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return industryListModel;
        }
    }
}
