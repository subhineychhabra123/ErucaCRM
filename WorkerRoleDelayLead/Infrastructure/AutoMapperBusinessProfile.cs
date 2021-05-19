using ErucaCRM.Domain;
using ErucaCRM.Repository;
using ErucaCRM.Utility;

namespace WorkerRoleDelayLead.Infrastructure
{
    public class AutoMapperNotificationManager : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperNotificationManager>();
                
            });
        }

        protected override void Configure()
        {
            base.Configure();
            AutoMapper.Mapper.CreateMap<CompanyModel, Company>();
        
           AutoMapper.Mapper.CreateMap<CultureInformation, CultureInformationModel>();
           AutoMapper.Mapper.CreateMap<MailHelper, Message>();
        }
    }
}