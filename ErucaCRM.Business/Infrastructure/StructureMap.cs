using System;
using StructureMap;
using StructureMap.Configuration.DSL;
using ErucaCRM.Business;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;

namespace ErucaCRM.Business.Infrastructure
{
    //public static class BusinessStructureMapper
    // {
    //     public static void Run()
    //     {
    //         ObjectFactory.Initialize(action =>
    //         {
    //             action.AddRegistry(new BusinessRegistry());
    //         });
    //     }
    // }

    public class BusinessRegistry : Registry
    {
        public BusinessRegistry()
        {
            For<IUnitOfWork>().Use<UnitOfWork>();
        }
    }
}