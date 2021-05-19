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
    public class TimeZoneBusiness :ITimeZoneBusiness
    {
         private readonly IUnitOfWork unitOfWork;
        private readonly TimeZoneRepository timeZoneRepository;

        public TimeZoneBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            timeZoneRepository = new TimeZoneRepository(unitOfWork);
        }

        public List<TimeZoneModal> GetTimeZones()
        {
            List<TimeZoneModal> listTimeZoneModel = new List<TimeZoneModal>();
            List<ErucaCRM.Repository.TimeZone> listTimeZone = timeZoneRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(listTimeZone, listTimeZoneModel);
            return listTimeZoneModel;
        }



    }
}
