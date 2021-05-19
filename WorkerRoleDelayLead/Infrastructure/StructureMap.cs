using System;
using StructureMap;
using StructureMap.Configuration.DSL;
using ErucaCRM.Business;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Business.Infrastructure;

namespace ErucaCRM.NotificationManager.Infrastructure
{
    public static class NotificationManagerMapper
    {
        public static void Run()
        {
            ObjectFactory.Initialize(action =>
            {
                action.AddRegistry(new BusinessRegistry());
            });
        }
    }

    
}