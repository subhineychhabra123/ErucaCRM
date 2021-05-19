using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;
using StructureMap.Configuration.DSL;
using ErucaCRM.Business;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Business.Infrastructure;

namespace ErucaCRM.Web.Infrastructure
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController
            GetControllerInstance(RequestContext requestContext,
            Type controllerType)
        {
            try
            {
                if ((requestContext == null) || (controllerType == null))
                    return null;

                return (Controller)ObjectFactory.GetInstance(controllerType);
            }
            catch (StructureMapException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                System.Diagnostics.Debug.WriteLine(ObjectFactory.GetAllInstances<IController>());
                throw;
                //System.Diagnostics.Debug.WriteLine(ObjectFactory.WhatDoIHave());
                //throw new Exception(ObjectFactory.WhatDoIHave());
            }
        }
    }

    public static class StructureMapper
    {
        public static void Run()
        {
            ControllerBuilder.Current
                .SetControllerFactory(new StructureMapControllerFactory());

            ObjectFactory.Initialize(action =>
            {
                action.AddRegistry(new RepositoryRegistry());
                action.AddRegistry(new BusinessRegistry());
            });
        }
    }

    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IUserBusiness>().Use<UserBusiness>();
            For<IRoleBusiness>().Use<RoleBusiness>();
            For<IProfileBusiness>().Use<ProfileBusiness>();
            For<IProfilePermissionBusiness>().Use<ProfilePermissionBusiness>();
            For<ILeadBusiness>().Use<LeadBusiness>();
            For<ILeadSourceBusiness>().Use<LeadSourceBusiness>();
            For<IIndustryBusiness>().Use<IndustryBusiness>();
            For<ILeadStatusBusiness>().Use<LeadStatusBusiness>();
            For<IContactBusiness>().Use<ContactBusiness>();
            For<IFileAttachmentBusiness>().Use<FileAttachmentBusiness>();
            For<IProductBusiness>().Use<ProductBusiness>();
            For<IQuoteBusiness>().Use<QuoteBusiness>();
            For<ITaskItemBusiness>().Use<TaskItemBusiness>();
            For<IProductLeadAssociationBusiness>().Use<ProductLeadAssociationBusiness>();
            For<ISalesOrderBusiness>().Use<SalesOrderBusiness>();
            For<IInvoiceBusiness>().Use<InvoiceBusiness>();
            For<IProductQuoteAssociationBusiness>().Use<ProductQuoteAssociationBusiness>();
            For<ICultureInformationBusiness>().Use<CultureInformationBusiness>();
            For<ITimeZoneBusiness>().Use<TimeZoneBusiness>();
            For<IApplicationPageBusiness>().Use<ApplicationPageBusiness>();
            For<IContentApplicationPageBusiness>().Use<ContentApplicationPageBusiness>();
            For<IAccountBusiness>().Use<AccountBusiness>();
            For<ITagBusiness>().Use<TagBusiness>();
            For<IStageBusiness>().Use<StageBusiness>();
            For<IAccountCaseBusiness>().Use<AccountCaseBusiness>();
            For<ICaseMessageBoardBusiness>().Use<CaseMessageBoardBusiness>();
            For<ILeadAuditBusiness>().Use<LeadAuditBusiness>();
            For<IRatingBusiness>().Use<RatingBusiness>();
            For<IPlanBusiness>().Use<PlanBusiness>();
            For<ICompanyBusiness>().Use<CompanyBusiness>();
            For<IReportBusiness>().Use<ReportBusiness>();
            For<IHomeBusiness>().Use<HomeBusiness>();
            For<ILeadCommentBussiness>().Use<LeadCommentBussiness>();
            For<IRealTimeNotificationBusiness>().Use<RealTimeNotificationBusiness>();
        }
    }
}