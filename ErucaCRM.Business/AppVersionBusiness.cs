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
    public class AppVersionBusiness : IAppVersion
    {
         private readonly IUnitOfWork unitOfWork;
         private readonly AppVersionRepository AppVersionRepository;

         public AppVersionBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            AppVersionRepository = new AppVersionRepository(unitOfWork);
           
        }

        public AppVersionModel GetVersion()
        {
            AppVersionModel appVersionModel = new AppVersionModel();
            GlobalSetting globalSettings =  AppVersionRepository.GetAll().SingleOrDefault();
            AutoMapper.Mapper.Map(globalSettings, appVersionModel);
            return appVersionModel;
         
        }
    }
}
