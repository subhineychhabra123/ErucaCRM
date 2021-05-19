using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace WorkerRoleRecentActivity.Infrastructure
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
          AutoMapper.Mapper.CreateMap<SSP_GetRecentActivitesForEmailNotification_Result,RecentActivitieModel>();
          AutoMapper.Mapper.CreateMap<CultureInformation, CultureInformationModel>();
        }
    }
}