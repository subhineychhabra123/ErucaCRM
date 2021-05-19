using System;
using StructureMap;
using StructureMap.Configuration.DSL;
using ErucaCRM.Business;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Business.Infrastructure;
namespace WorkerRoleRecentActivity.Infrastructure
{
    public static class StructureMap
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