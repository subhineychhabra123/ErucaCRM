using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
using AutoMapper;
using ErucaCRM.Utility;

namespace ErucaCRM.Business.Infrastructure
{
    public class AutoMapperBusinessProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperBusinessProfile>();
            });
        }

        protected override void Configure()
        {
            base.Configure();
            AutoMapper.Mapper.CreateMap<CompanyModel, Company>();
            AutoMapper.Mapper.CreateMap<Company, CompanyModel>();
            AutoMapper.Mapper.CreateMap<Stage, StageModel>();
            AutoMapper.Mapper.CreateMap<Rating, RatingModel>().ForMember(dest => dest.IsLastRating, opt => opt.MapFrom(src => src.RatingConstant == (int)Enums.Rating.LastRating ? true : false));
            AutoMapper.Mapper.CreateMap<AccountType, AccountTypeModel>();
            AutoMapper.Mapper.CreateMap<AccountTypeModel, AccountType>();
            AutoMapper.Mapper.CreateMap<Tag, TagModel>();
            AutoMapper.Mapper.CreateMap<TagModel, Tag>();
            AutoMapper.Mapper.CreateMap<ContactTagModel, ContactTag>();
            AutoMapper.Mapper.CreateMap<ContactTag, ContactTagModel>()
           .ForMember(dest => dest.TagModel, opt => opt.MapFrom(src => src.Tag));

            AutoMapper.Mapper.CreateMap<PlanModel, Plan>()
              .ForMember(dest => dest.PlanModules, opt => opt.MapFrom(src => src.PlanModulesModel));

            AutoMapper.Mapper.CreateMap<Plan, PlanModel>()
                 .ForMember(dest => dest.PlanModulesModel, opt => opt.MapFrom(src => src.PlanModules));

            AutoMapper.Mapper.CreateMap<CompanyPlanModel, CompanyPlan>();
            AutoMapper.Mapper.CreateMap<CompanyPlan, CompanyPlanModel>();

            AutoMapper.Mapper.CreateMap<PlanModuleModel, PlanModule>()
            .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.ModuleModel));

            AutoMapper.Mapper.CreateMap<PlanModule, PlanModuleModel>()
                  .ForMember(dest => dest.ModuleModel, opt => opt.MapFrom(src => src.Module));

            AutoMapper.Mapper.CreateMap<AccountTagModel, AccountTag>();
            AutoMapper.Mapper.CreateMap<AccountTag, AccountTagModel>()
           .ForMember(dest => dest.TagModel, opt => opt.MapFrom(src => src.Tag));

            AutoMapper.Mapper.CreateMap<AccountModel, Account>()
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                .ForMember(dest => dest.Address1, opt => opt.Ignore())
                 .ForMember(dest => dest.BillingAddressId, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingAddressId, opt => opt.Ignore())
                .ForMember(dest => dest.AccountType, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                //.ForMember(dest => dest.Contacts, opt => opt.Ignore())
                .ForMember(dest => dest.Industry, opt => opt.Ignore())
                .ForMember(dest => dest.Leads, opt => opt.Ignore())
                .ForMember(dest => dest.FileAttachments, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<AccountContactModel, AccountContact>();
            AutoMapper.Mapper.CreateMap<LeadContactModel, LeadContact>();

            AutoMapper.Mapper.CreateMap<Account, AccountModel>()
                .ForMember(dest => dest.AccountOwner, opt => opt.MapFrom(src => src.User != null ? CommonFunctions.ConcatenateStrings(src.User.FirstName, src.User.LastName) : ""))
              .ForMember(dest => dest.AccountTypeModel, opt => opt.MapFrom(src => src.AccountType));


            AutoMapper.Mapper.CreateMap<AccountCase, AccountCaseModel>()
                .ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.Account.AccountName));

            AutoMapper.Mapper.CreateMap<AccountCaseModel, AccountCase>().ForMember(dest => dest.CreatedBy, opt => opt.Ignore()).ForMember(dest => dest.User, opt => opt.Ignore()).ForMember(dest => dest.CaseMessageBoards, opt => opt.Ignore()).ForMember(dest => dest.FileAttachments, opt => opt.Ignore());






            AutoMapper.Mapper.CreateMap<CultureInformationModel, CultureInformation>();
            AutoMapper.Mapper.CreateMap<CultureInformation, CultureInformationModel>();

            AutoMapper.Mapper.CreateMap<TimeZoneModal, ErucaCRM.Repository.TimeZone>();
            AutoMapper.Mapper.CreateMap<ErucaCRM.Repository.TimeZone, TimeZoneModal>();

            AutoMapper.Mapper.CreateMap<Country, CountryModel>();
            AutoMapper.Mapper.CreateMap<CountryModel, Country>();
            AutoMapper.Mapper.CreateMap<AccountContact, AccountContactModel>();
            AutoMapper.Mapper.CreateMap<LeadContact, LeadContactModel>();

            AutoMapper.Mapper.CreateMap<Contact, ContactModel>()
               .ForMember(dest => dest.ContactTagModels, opt => opt.MapFrom(src => src.ContactTags));

            AutoMapper.Mapper.CreateMap<ContactModel, Contact>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FileAttachments, opt => opt.Ignore())
                 .ForMember(dest => dest.ContactTags, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<Contact, ContactBulkUploadModel>();
            AutoMapper.Mapper.CreateMap<Address, AddressModel>()
                .ForMember(dest => dest.CountryModel, opt => opt.MapFrom(src => src.Country));
            AutoMapper.Mapper.CreateMap<AddressModel, Address>()
                .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                .ForMember(dest => dest.Country, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Role, RoleModel>();
            AutoMapper.Mapper.CreateMap<RoleModel, Role>();

            AutoMapper.Mapper.CreateMap<ApplicationPage, ApplicationPageModel>();
            AutoMapper.Mapper.CreateMap<ApplicationPageModel, ApplicationPage>();

            AutoMapper.Mapper.CreateMap<AssociationApplicationPage, AssociationApplicationPageModel>();
            AutoMapper.Mapper.CreateMap<AssociationApplicationPageModel, AssociationApplicationPage>();

            AutoMapper.Mapper.CreateMap<ContentApplicationPage, ContentApplicationPageModel>();
            AutoMapper.Mapper.CreateMap<ContentApplicationPageModel, ContentApplicationPage>();

            AutoMapper.Mapper.CreateMap<Repository.Profile, ProfileModel>();
            AutoMapper.Mapper.CreateMap<ProfileModel, Repository.Profile>()
                .ForMember(dest => dest.ProfilePermissions, opt => opt.MapFrom(src => src.ProfilePermissionModels));

            AutoMapper.Mapper.CreateMap<UserModel, User>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleModel))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.ProfileModel))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel));

            AutoMapper.Mapper.CreateMap<User, UserModel>()
                .ForMember(dest => dest.CultureInformationModel, opt => opt.MapFrom(src => src.CultureInformation));

            AutoMapper.Mapper.CreateMap<Module, ModuleModel>();
            AutoMapper.Mapper.CreateMap<ModuleModel, Module>()
                .ForMember(dest => dest.ModulePermissions, opt => opt.MapFrom(src => src.ModulePermissionModels))
                .ForMember(dest => dest.TaskItems, opt => opt.MapFrom(src => src.TaskModels))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel))
                .ForMember(dest => dest.User1, opt => opt.MapFrom(src => src.UserModel1));

            AutoMapper.Mapper.CreateMap<Permission, PermissionModel>();
            AutoMapper.Mapper.CreateMap<PermissionModel, Permission>()
                .ForMember(dest => dest.ModulePermissions, opt => opt.MapFrom(src => src.ModulePermissionModels));

            AutoMapper.Mapper.CreateMap<ModulePermission, ModulePermissionModel>();
            AutoMapper.Mapper.CreateMap<ModulePermissionModel, ModulePermission>()
                .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.Module))
                .ForMember(dest => dest.Permission, opt => opt.MapFrom(src => src.Permission));

            AutoMapper.Mapper.CreateMap<ProfilePermission, ProfilePermissionModel>();
            AutoMapper.Mapper.CreateMap<ProfilePermissionModel, ProfilePermission>()
                .ForMember(dest => dest.ModulePermission, opt => opt.MapFrom(src => src.ModulePermission))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.ProfileModel))
                .ForMember(dest => dest.User1, opt => opt.MapFrom(src => src.UserModel1))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel));

            AutoMapper.Mapper.CreateMap<LeadSource, LeadSourceModel>();
            AutoMapper.Mapper.CreateMap<LeadSourceModel, LeadSource>();

            AutoMapper.Mapper.CreateMap<Industry, IndustryModel>();
            AutoMapper.Mapper.CreateMap<IndustryModel, Industry>();

            AutoMapper.Mapper.CreateMap<Lead, LeadAuditModel>()
            .ForMember(dest => dest.Rating, opt => opt.Ignore())
            .ForMember(dest => dest.ClosingDate, opt => opt.Ignore());
            AutoMapper.Mapper.CreateMap<LeadAuditModel, Lead>();

            AutoMapper.Mapper.CreateMap<LeadAudit, LeadAuditModel>();
            AutoMapper.Mapper.CreateMap<LeadAuditModel, LeadAudit>()
                .ForMember(dest => dest.Rating, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Lead, LeadModel>()
                         .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User))
                          .ForMember(dest => dest.LeadContactModel, opt => opt.MapFrom(src => src.LeadContacts))
                           .ForMember(dest => dest.LeadTagsModels, opt => opt.MapFrom(src => src.LeadTags));
            AutoMapper.Mapper.CreateMap<LeadModel, Lead>()
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.AddressId, opt => opt.Ignore())
                .ForMember(dest => dest.FileAttachments, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Contact, opt => opt.Ignore())
                // .ForMember(dest => dest.ContactId, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.Ignore())
                // .ForMember(dest => dest.RatingId, opt => opt.Ignore())
             .ForMember(dest => dest.Rating, opt => opt.Ignore())
                .ForMember(dest => dest.Stage, opt => opt.Ignore());




            AutoMapper.Mapper.CreateMap<User, OwnerListModel>()
                .ForMember(dest => dest.LeadOwnerId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.UserId));

            AutoMapper.Mapper.CreateMap<FileAttachment, FileAttachmentModel>()
            .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User));
            AutoMapper.Mapper.CreateMap<FileAttachmentModel, FileAttachment>()
                .ForMember(dest => dest.Lead, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());


            AutoMapper.Mapper.CreateMap<LeadStatu, LeadStatusModel>();
            AutoMapper.Mapper.CreateMap<LeadStatusModel, LeadStatu>();

            AutoMapper.Mapper.CreateMap<Product, ProductModel>();
            AutoMapper.Mapper.CreateMap<ProductModel, Product>();


            AutoMapper.Mapper.CreateMap<TaskItem, TaskItemModel>()
             .ForMember(dest => dest.FileAttachmentModels, opt => opt.MapFrom(src => src.FileAttachments))
                .ForMember(dest => dest.OwnerImage, opt => opt.MapFrom(src => src.User.ImageURL))
                 .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.User != null ? CommonFunctions.ConcatenateStrings(src.User.FirstName, src.User.LastName) : ""));

            AutoMapper.Mapper.CreateMap<TaskItemModel, TaskItem>();
            AutoMapper.Mapper.CreateMap<TaskItem,DashboarActivitiesModel>();

            AutoMapper.Mapper.CreateMap<AssociatedProduct, AssociatedProductModel>();
            AutoMapper.Mapper.CreateMap<AssociatedProductModel, AssociatedProduct>();
            //.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductModel));

            AutoMapper.Mapper.CreateMap<TaskStatu, TaskStatusModel>();
            AutoMapper.Mapper.CreateMap<TaskStatusModel, TaskStatu>();

            AutoMapper.Mapper.CreateMap<ProductLeadAssociation, ProductLeadAssociationModel>();
            AutoMapper.Mapper.CreateMap<ProductLeadAssociationModel, ProductLeadAssociation>()
                                .ForMember(dest => dest.Product, opt => opt.Ignore())
                                 .ForMember(dest => dest.Lead, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<ProductQuoteAssociation, ProductQuoteAssociationModel>();
            AutoMapper.Mapper.CreateMap<ProductQuoteAssociationModel, ProductQuoteAssociation>()
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<ProductSalesOrderAssociation, ProductSalesOrderAssociationModel>();
            AutoMapper.Mapper.CreateMap<ProductSalesOrderAssociationModel, ProductSalesOrderAssociation>()
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<ProductInvoiceAssociation, ProductInvoiceAssociationModel>();
            AutoMapper.Mapper.CreateMap<ProductInvoiceAssociationModel, ProductInvoiceAssociation>()
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<Quote, QuoteModel>()
                 .AfterMap((src, dest) =>
                 {
                     //if (src.Lead != null)
                     //{
                     //    dest.LeadModel.FirstName = src.Lead.FirstName;
                     //    dest.LeadModel.LastName = src.Lead.LastName;
                     //}
                     if (src.User != null)
                     {
                         dest.UserModel.FirstName = src.User.FirstName;
                         dest.UserModel.LastName = src.User.LastName;
                     }
                 });
            //.ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User))
            //.ForMember(dest => dest.LeadModel, opt => opt.MapFrom(src => src.Lead));
            AutoMapper.Mapper.CreateMap<QuoteModel, Quote>()
            .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId > 0 ? src.LeadId : (int?)null))
             .ForMember(dest => dest.Address, opt => opt.Ignore())
             .ForMember(dest => dest.Address1, opt => opt.Ignore())
              .ForMember(dest => dest.Lead, opt => opt.Ignore())
              .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.ProductQuoteAssociations, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<SalesOrder, SalesOrderModel>()
                 .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User))
                .ForMember(src => src.AccountModel, opt => opt.MapFrom(src => src.Account));
            //.AfterMap((src, dest) =>
            //{
            //    if (src.Lead != null)
            //    {
            //        dest.LeadModel.FirstName = src.Lead.FirstName;
            //        dest.LeadModel.LastName = src.Lead.LastName;
            //    }
            //    if (src.User != null)
            //    {
            //        dest.UserModel.FirstName = src.User.FirstName;
            //        dest.UserModel.LastName = src.User.LastName;
            //    }



            //});
            AutoMapper.Mapper.CreateMap<SalesOrderModel, SalesOrder>()
               .ForMember(dest => dest.BillingAddressId, opt => opt.Ignore())
               .ForMember(dest => dest.ShippingAddressId, opt => opt.Ignore())
              .ForMember(dest => dest.Address, opt => opt.Ignore())
             .ForMember(dest => dest.Address1, opt => opt.Ignore())
              .ForMember(dest => dest.Account, opt => opt.Ignore())
              .ForMember(dest => dest.User, opt => opt.Ignore())
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId > 0 ? src.AccountId : (int?)null))
               .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.QuoteId > 0 ? src.QuoteId : (int?)null))
            .ForMember(dest => dest.ProductSalesOrderAssociations, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Invoice, InvoiceModel>()
                  .AfterMap((src, dest) =>
                 {

                     if (src.User != null)
                     {
                         dest.UserModel.FirstName = src.User.FirstName;
                         dest.UserModel.LastName = src.User.LastName;
                     }
                 });
            AutoMapper.Mapper.CreateMap<InvoiceModel, Invoice>()
            .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId > 0 ? src.LeadId : (int?)null))
            .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId > 0 ? src.SalesOrderId : (int?)null))
             .ForMember(dest => dest.BillingAddressId, opt => opt.Ignore())
             .ForMember(dest => dest.ShippingAddressId, opt => opt.Ignore())
             .ForMember(dest => dest.Address, opt => opt.Ignore())
             .ForMember(dest => dest.Address1, opt => opt.Ignore())
             .ForMember(dest => dest.Lead, opt => opt.Ignore())
             .ForMember(dest => dest.User, opt => opt.Ignore())
             .ForMember(dest => dest.ProductInvoiceAssociations, opt => opt.Ignore());

            //MAppers for Complex type
            AutoMapper.Mapper.CreateMap<SSP_GetYearWiseLeadCount_Result, YearWiseLeadModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetMonthWiseLeadCount_Result, YearWiseLeadModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetWeekWiseLeadCount_Result, YearWiseLeadModel>();


            AutoMapper.Mapper.CreateMap<SSP_GetAllVisibleCulture_Result, CultureInformationModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetLeadHistory_Result, LeadAuditModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetLeadHistoryChartDetails_Result, LeadHistoryChartModel>();
            AutoMapper.Mapper.CreateMap<StageModel, Stage>()
                .ForMember(dest => dest.Rating, opt => opt.Ignore())
                  .ForMember(dest => dest.LeadAudits, opt => opt.Ignore())
                  .ForMember(dest => dest.Leads, opt => opt.Ignore())
             .ForMember(dest => dest.DefaultRatingId, opt => opt.MapFrom(src => src.DefaultRatingId.Value == 0 ? null : src.DefaultRatingId));
            AutoMapper.Mapper.CreateMap<RatingModel, Rating>();

            AutoMapper.Mapper.CreateMap<CaseMessageBoard, CaseMessageBoardModel>();
            AutoMapper.Mapper.CreateMap<CaseMessageBoardModel, CaseMessageBoard>().ForMember(dest => dest.CreatedDate, opt => opt.Ignore()).ForMember(dest => dest.AccountCase, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<LeadTag, LeadTagModel>();

            AutoMapper.Mapper.CreateMap<LeadTagModel, LeadTag>();

            AutoMapper.Mapper.CreateMap<LeadComment, LeadCommentModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + "" + src.User.LastName))
                .ForMember(dest => dest.UserImg, opt => opt.MapFrom(src => src.User.ImageURL))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId));

            AutoMapper.Mapper.CreateMap<LeadCommentModel, LeadComment>();
            AutoMapper.Mapper.CreateMap<RealTimeNotificationModel, RealTimeNotificationConnection>();
            AutoMapper.Mapper.CreateMap<RealTimeNotificationConnection, RealTimeNotificationModel>();
            //Report Store Precedures Mapping Classes
            AutoMapper.Mapper.CreateMap<ssp_GetLeadsInPipeLine_Result, LeadsInPipeLineModel>();

            AutoMapper.Mapper.CreateMap<ssp_GetMonthWiseAccountSaleRevenue_Result, AccountSaleRevenueModel>();
            AutoMapper.Mapper.CreateMap<ssp_GetAccountByTopHighestSaleRevenue_Result, AccountSaleRevenueModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetLeadAuditsForHomeRecentActivites_Result, HomeModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetLeadAudits_Result, LeadAuditModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetContactsByUserId_Result, ContactModel>();
            AutoMapper.Mapper.CreateMap<SSP_DasboardAnalyticData_Result, HomeModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetContactForLeadAccountContacts_Result, ContactModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetContactForLeadAccountContacts_Result, AccountContactModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetContactsList_Result, ContactModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetContactsByLeadId_Result, ContactModel>();
           // AutoMapper.Mapper.CreateMap<SSP_Notifications_Result, HomeModel>();
            AutoMapper.Mapper.CreateMap<SSP_NonAssociatedContactList_Result, ContactModel>();
            AutoMapper.Mapper.CreateMap<SSP_LeadsListbyUserId_Result, LeadModel>(); AutoMapper.Mapper.CreateMap<SSP_GetAccountsListbyUserId_Result, AccountListModel>(); AutoMapper.Mapper.CreateMap<SSP_SalesOrderListbyUserId_Result, SalesOrderModel>(); AutoMapper.Mapper.CreateMap<SSP_GetAccountsListbyUserId_Result, AccountModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetActivities_Result, DashboarActivitiesModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetActivitiesByUserId_Result, TaskItemModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetAccountCasesByUserId_Result, AccountCaseModel>().ForMember(dest => dest.
CaseOwnerName, opt => opt.MapFrom(src => src.OwnerName)).ForMember(dest => dest.AccountName, opt => opt.MapFrom(src => src.AccountName));
            AutoMapper.Mapper.CreateMap<GlobalSetting, AppVersionModel>();
            AutoMapper.Mapper.CreateMap<UserSetting, UserSettingModel>();
            AutoMapper.Mapper.CreateMap<SSP_GetRecentActivitesForEmailNotification_Result, HomeModel>();
            //AutoMapper.Mapper.CreateMap<SSP_GetAllRecentActivitesForEmailNotification_Result, HomeModel>();
        }
    }
}