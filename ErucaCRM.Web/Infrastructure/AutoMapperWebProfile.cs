using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Domain;
using ErucaCRM.Utility.WebClasses;
using ErucaCRM.Web.ViewModels;
using ErucaCRM.Utility;
using System.Globalization;

namespace ErucaCRM.Business.Infrastructure
{
    public class AutoMapperWebProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperWebProfile>();
                a.AddProfile<AutoMapperBusinessProfile>();
            });
        }


        protected override void Configure()
        {
            base.Configure();

            #region View Model to Domain Model
            AutoMapper.Mapper.CreateMap<AccountContactVM, AccountContactModel>();
            AutoMapper.Mapper.CreateMap<AccountLeadContactInfo, AccountContactModel>()
             .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.contactId.Decrypt()))
              .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Decrypt()));


            AutoMapper.Mapper.CreateMap<AccountLeadContactInfo, LeadContactModel>()
            .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.contactId.Decrypt()))
             .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.leadId.Decrypt()));

            AutoMapper.Mapper.CreateMap<LeadContactVM, LeadContactModel>();
            AutoMapper.Mapper.CreateMap<AccountVM, AccountModel>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Decrypt()))
                  .ForMember(dest => dest.AccountOwnerId, opt => opt.MapFrom(src => src.AccountOwnerId.Decrypt()))
                 .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address))
                 .ForMember(dest => dest.AddressModel1, opt => opt.MapFrom(src => src.Address1));

            AutoMapper.Mapper.CreateMap<ApplicationPageVM, ApplicationPageModel>().
                ForMember(dest => dest.ApplicationPageId, opt => opt.MapFrom(src => src.ApplicationPageId.Decrypt()));

            AutoMapper.Mapper.CreateMap<AccountCaseVM, AccountCaseModel>()
        .ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => src.AccountCaseId.Decrypt()));

            AutoMapper.Mapper.CreateMap<TagVM, TagModel>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.TagId != "0" ? (src.TagId.Decrypt()) : 0));

            AutoMapper.Mapper.CreateMap<LeadTagVM, LeadTagModel>();

            AutoMapper.Mapper.CreateMap<PlanVM, PlanModel>()
                   .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId.Decrypt()))
                   .ForMember(dest => dest.PlanModulesModel, opt => opt.MapFrom(src => src.PlanModules))
                   .ForMember(dest => dest.CompanyPlansModel, opt => opt.MapFrom(src => src.CompanyPlans));

            AutoMapper.Mapper.CreateMap<PlanModuleVM, PlanModuleModel>()
            .ForMember(dest => dest.PlanModuleId, opt => opt.MapFrom(src => src.PlanModuleId.Decrypt()))
             .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId.Decrypt()));

            AutoMapper.Mapper.CreateMap<AssociationApplicationPageVM, AssociationApplicationPageModel>()
                  .ForMember(dest => dest.ApplicationPageId, opt => opt.MapFrom(src => src.ApplicationPageId.Decrypt()))
             .ForMember(dest => dest.CustomPageId, opt => opt.MapFrom(src => src.CustomPageId.Decrypt()));
            AutoMapper.Mapper.CreateMap<ContentApplicationPageVM, ContentApplicationPageModel>()
                    .ForMember(dest => dest.ApplicationPageId, opt => opt.MapFrom(src => src.ApplicationPageId.Decrypt()))
                    .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.Decrypt()));

            AutoMapper.Mapper.CreateMap<CompanyVM, CompanyModel>();
            AutoMapper.Mapper.CreateMap<CultureInformationVM, CultureInformationModel>()
                .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.Decrypt()));

            AutoMapper.Mapper.CreateMap<TimeZoneVM, TimeZoneModal>()
                .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId.Decrypt()));

            AutoMapper.Mapper.CreateMap<ContactVM, ContactModel>()
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Decrypt()))
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountId)) ? src.AccountId.Decrypt() : (int?)null))
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.LeadId)) ? src.LeadId.Decrypt() : (int?)null))
                .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User));


            AutoMapper.Mapper.CreateMap<RoleVM, RoleModel>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId != "0" ? src.RoleId.Decrypt() : 0))
                   .ForMember(dest => dest.ParentRoleId, opt => opt.MapFrom(src => src.ParentRoleId.Decrypt()));

            AutoMapper.Mapper.CreateMap<ProfileVM, ProfileModel>()
                   .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId.Decrypt()));


            AutoMapper.Mapper.CreateMap<CountryVM, CountryModel>();

            AutoMapper.Mapper.CreateMap<ProfileAddressVM, AddressModel>()
                .ForMember(dest => dest.CountryModel, opt => opt.MapFrom(src => src.Country));

            AutoMapper.Mapper.CreateMap<RegistrationVM, UserModel>()
                .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId.Decrypt()))
            .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.Decrypt()));
            AutoMapper.Mapper.CreateMap<UserProfileVM, UserModel>()
                   .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()))
                   .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId.Decrypt()))
                    .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.Decrypt()))
                .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.RoleModel, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.ProfileModel, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company));

            AutoMapper.Mapper.CreateMap<UserVM, UserModel>()
                 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()))
                  .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.CultureInformationId)) ? src.CultureInformationId.Decrypt() : (int?)null))
               .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.TimeZoneId)) ? src.TimeZoneId.Decrypt() : (int?)null))
                  .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.RoleModel, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.ProfileModel, opt => opt.MapFrom(src => src.Profile))
                .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company));

            AutoMapper.Mapper.CreateMap<LeadSourceVM, LeadSourceModel>();
            AutoMapper.Mapper.CreateMap<IndustryVM, IndustryModel>();
            AutoMapper.Mapper.CreateMap<LeadStatusVM, LeadStatusModel>();
            AutoMapper.Mapper.CreateMap<LeadUserVM, UserModel>();
            AutoMapper.Mapper.CreateMap<FileAttachmentVM, FileAttachmentModel>()
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountId)) ? src.AccountId.Decrypt() : (int?)null))
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.ContactId)) ? src.ContactId.Decrypt() : (int?)null))
                  .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.LeadId)) ? src.LeadId.Decrypt() : (int?)null))
                   .ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountCaseId)) ? src.AccountCaseId.Decrypt() : (int?)null))
                .ForMember(dest => dest.UserModel, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<LeadVM, LeadModel>()
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt()))
                .ForMember(dest => dest.RatingId, opt => opt.MapFrom(src => src.RatingId.Decrypt()))
                .ForMember(dest => dest.LeadTagsModels, opt => opt.MapFrom(src => src.LeadTags))
                .ForMember(dest => dest.FinalStageId, opt => opt.Ignore())
                .ForMember(dest => dest.StageId, opt => opt.Ignore())
                   .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountId)) ? src.AccountId.Decrypt() : (int?)null))


                .ForMember(dest => dest.FileAttachmentModels, opt => opt.MapFrom(src => src.FileAttachments))
                .ForMember(dest => dest.IndustryModel, opt => opt.MapFrom(src => src.Industry))
                .ForMember(dest => dest.LeadSourceModel, opt => opt.MapFrom(src => src.LeadSource))
                .ForMember(dest => dest.LeadStatusModel, opt => opt.MapFrom(src => src.LeadStatus))
                .ForMember(dest => dest.ProductLeadAssociationModels, opt => opt.MapFrom(src => src.ProductLeadAssociations))
                .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User));


            AutoMapper.Mapper.CreateMap<ProductVM, ProductModel>();
            AutoMapper.Mapper.CreateMap<TaskItemVM, TaskItemModel>()
             .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Decrypt()));

            AutoMapper.Mapper.CreateMap<ProductLeadAssociationVM, ProductLeadAssociationModel>()
                        .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt()))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            AutoMapper.Mapper.CreateMap<AssociatedProductVM, AssociatedProductModel>();
            // .ForMember(dest => dest.ProductModel, opt => opt.MapFrom(src => src.Product));

            AutoMapper.Mapper.CreateMap<TaskStatusVM, TaskStatusModel>();

            AutoMapper.Mapper.CreateMap<ProductQuoteAssociationVM, ProductQuoteAssociationModel>()
                .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.QuoteId.Decrypt()))
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<ShippingAddressVM, AddressModel>()
                .ForMember(dest => dest.CountryModel, opt => opt.MapFrom(src => src.Country));

            AutoMapper.Mapper.CreateMap<BillingAddressVM, AddressModel>()
                .ForMember(dest => dest.CountryModel, opt => opt.MapFrom(src => src.Country));

            AutoMapper.Mapper.CreateMap<QuoteVM, QuoteModel>()
                .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.QuoteId.Decrypt()))

                 .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.LeadId) ? src.LeadId.Decrypt() : (int?)null))
                .ForMember(dest => dest.AddressModel1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.ProductQuoteAssociationModels, opt => opt.MapFrom(src => src.ProductQuoteAssociations))
                .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.LeadModel, opt => opt.MapFrom(src => src.Lead))
                .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address));


            AutoMapper.Mapper.CreateMap<SalesOrderVM, SalesOrderModel>()
              .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.Decrypt()))
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.AccountId) ? src.AccountId.Decrypt() : (int?)null))
                 .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.QuoteId) ? src.QuoteId.Decrypt() : (int?)null))
                .ForMember(dest => dest.AddressModel1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.ProductSalesOrderAssociationModels, opt => opt.MapFrom(src => src.ProductSalesOrderAssociations))
                .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.AccountModel, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address));

            AutoMapper.Mapper.CreateMap<SaleOrdersVM, SalesOrderModel>()
             .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.Decrypt()));

            AutoMapper.Mapper.CreateMap<InvoiceVM, InvoiceModel>()
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId.Decrypt()))
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.LeadId) ? src.LeadId.Decrypt() : (int?)null))
                .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.SalesOrderId) ? src.SalesOrderId.Decrypt() : (int?)null))
                .ForMember(dest => dest.AddressModel1, opt => opt.MapFrom(src => src.Address1))
                .ForMember(dest => dest.ProductInvoiceAssociationModels, opt => opt.MapFrom(src => src.ProductInvoiceAssociations))
                .ForMember(dest => dest.UserModel, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.LeadModel, opt => opt.MapFrom(src => src.Lead))
                .ForMember(dest => dest.AddressModel, opt => opt.MapFrom(src => src.Address));


            AutoMapper.Mapper.CreateMap<LeadAuditVM, LeadAuditModel>();


            AutoMapper.Mapper.CreateMap<InvoicesVM, InvoiceModel>()
                 .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId.Decrypt()));
            AutoMapper.Mapper.CreateMap<ProductSalesOrderAssociationVM, ProductSalesOrderAssociationModel>()
                .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.Decrypt()))
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<ProductInvoiceAssociationVM, ProductInvoiceAssociationModel>()
                     .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId.Decrypt()))
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<CaseMessageBoardVM, CaseMessageBoardModel>().ForMember(dest => dest.CaseMessageBoardId, opt => opt.MapFrom(src => src.CaseMessageBoardId.Decrypt())).ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => src.AccountCaseId.Decrypt()));

            #endregion

            #region Domain Model to View Model
            AutoMapper.Mapper.CreateMap<AccountModel, AccountVM>()

                 .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Encrypt()))
                 .ForMember(dest => dest.AccountOwnerId, opt => opt.MapFrom(src => src.AccountOwnerId.Encrypt()))
                 .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountTypeModel.AccountTypeName))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                 .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountTypeModel))
                 .ForMember(dest => dest.AccountCases, opt => opt.MapFrom(src => src.AccountCaseModels))
                 .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                 .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressModel1))
                 .ForMember(dest => dest.AccountTags, opt => opt.MapFrom(src => src.AccountTagModels))
                 .ForMember(dest => dest.Leads, opt => opt.MapFrom(src => src.LeadsModels))
                 .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachmentModels));

            AutoMapper.Mapper.CreateMap<AccountContactModel, AccountContactVM>().
            ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contact));

            AutoMapper.Mapper.CreateMap<LeadContactModel, LeadContactVM>().
            ForMember(dest => dest.Contact, opt => opt.MapFrom(src => src.Contact));
          
            AutoMapper.Mapper.CreateMap<AccountCaseModel, AccountCaseVM>()
            .ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => src.AccountCaseId.Encrypt()))
              .ForMember(dest => dest.AccountIdEncrypted, opt => opt.MapFrom(src => src.AccountId.Encrypt()))
              .ForMember(dest => dest.CaseOwnerName, opt => opt.MapFrom(src => src.CaseOwnerName))
             .ForMember(dest => dest.OwnerImage, opt => opt.MapFrom(src => src.User.ImageURL))
             .ForMember(dest => dest.CaseOwnerIdEncrypted, opt => opt.MapFrom(src => src.CaseOwnerId.Encrypt()));

            AutoMapper.Mapper.CreateMap<TagModel, TagVM>()
                .ForMember(dest => dest.TagId, opt => opt.MapFrom(src => src.TagId.Encrypt()));

            AutoMapper.Mapper.CreateMap<PlanModel, PlanVM>()
                .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId.Encrypt()))
                .ForMember(dest => dest.PlanModules, opt => opt.MapFrom(src => src.PlanModulesModel))
                 .ForMember(dest => dest.CompanyPlans, opt => opt.MapFrom(src => src.CompanyPlansModel));

            AutoMapper.Mapper.CreateMap<PlanModuleModel, PlanModuleVM>()
           .ForMember(dest => dest.PlanModuleId, opt => opt.MapFrom(src => src.PlanModuleId.Encrypt()))
            .ForMember(dest => dest.PlanId, opt => opt.MapFrom(src => src.PlanId.Encrypt()))
            .ForMember(dest => dest.Module, opt => opt.MapFrom(src => src.ModuleModel));

            AutoMapper.Mapper.CreateMap<ApplicationPageModel, ApplicationPageVM>()
            .ForMember(dest => dest.ApplicationPageId, opt => opt.MapFrom(src => src.ApplicationPageId.Encrypt()));
            AutoMapper.Mapper.CreateMap<CultureSpecificSiteContentModel, CultureSpecificSiteContentVM>();
             AutoMapper.Mapper.CreateMap<CultureSpecificSiteContentModel, PublicHomePageVM>();
             AutoMapper.Mapper.CreateMap<CultureSpecificSiteContentModel, PublicFeaturePageVM>();
             AutoMapper.Mapper.CreateMap<CultureSpecificSiteContentModel, PublicAboutUsPageVM>();
             AutoMapper.Mapper.CreateMap<CultureSpecificSiteContentModel, PublicMobileCRMPageVM>();
       
            AutoMapper.Mapper.CreateMap<AssociationApplicationPageModel, AssociationApplicationPageVM>()
                .ForMember(dest => dest.ApplicationPageId, opt => opt.MapFrom(src => src.ApplicationPageId.Encrypt()))
             .ForMember(dest => dest.CustomPageId, opt => opt.MapFrom(src => src.CustomPageId.Encrypt()));
            AutoMapper.Mapper.CreateMap<ContentApplicationPageModel, ContentApplicationPageVM>()
                  .ForMember(dest => dest.ApplicationPageId, opt => opt.MapFrom(src => src.ApplicationPageId.Encrypt()))
                     .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.Encrypt()));

            AutoMapper.Mapper.CreateMap<ContactModel, ContactVM>()
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()))
                  .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.HasValue ? src.AccountId.Value.Encrypt() : null))
                         .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                 .ForMember(dest => dest.ContactTags, opt => opt.MapFrom(src => src.ContactTagModels))
                .ForMember(dest => dest.FileAttachments, opt => opt.MapFrom(src => src.FileAttachmentModels))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel));

            AutoMapper.Mapper.CreateMap<ContactModel, DropDownHelper>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.ContactName))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ContactId.Encrypt()));

            AutoMapper.Mapper.CreateMap<CultureInformationModel, CultureInformationVM>()
                 .ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.Encrypt()));

            AutoMapper.Mapper.CreateMap<TimeZoneModal, TimeZoneVM>()
                .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId.Encrypt()));
            AutoMapper.Mapper.CreateMap<UserModel, RegistrationVM>()
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel));

            AutoMapper.Mapper.CreateMap<RoleModel, RoleVM>()
                 .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => (src.IsDefaultForRegisterdUser.HasValue && src.IsDefaultForRegisterdUser.Value) || (src.IsDefaultForStaffUser.HasValue && src.IsDefaultForStaffUser.Value) ? CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, src.RoleName) : src.RoleName))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId.Encrypt()))
                .ForMember(dest => dest.ParentRoleId, opt => opt.MapFrom(src => src.ParentRoleId.HasValue ? src.ParentRoleId.Value.Encrypt() : null));



            AutoMapper.Mapper.CreateMap<ProfileModel, ProfileVM>()
                  .ForMember(dest => dest.ProfileId, opt => opt.MapFrom(src => src.ProfileId.Encrypt()))
                   .ForMember(dest => dest.ProfileName, opt => opt.MapFrom(src => (src.IsDefaultForRegisterdUser.HasValue && src.IsDefaultForRegisterdUser.Value) || (src.IsDefaultForStaffUser.HasValue && src.IsDefaultForStaffUser.Value) ? CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_DROPDOWNS, src.ProfileName) : src.ProfileName));

            AutoMapper.Mapper.CreateMap<CountryModel, CountryVM>();

            AutoMapper.Mapper.CreateMap<AddressModel, ProfileAddressVM>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryModel));

            AutoMapper.Mapper.CreateMap<UserModel, UserProfileVM>()
                     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()))
                     .ForMember(dest => dest.TimeZoneId, opt => opt.MapFrom(src => src.TimeZoneId.HasValue ? src.TimeZoneId.Value.Encrypt() : null
))
.ForMember(dest => dest.CultureInformationId, opt => opt.MapFrom(src => src.CultureInformationId.HasValue ? src.CultureInformationId.Value.Encrypt() : null))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleModel))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.ProfileModel))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel))
             .ForMember(dest => dest.CultureInformationVM, opt => opt.MapFrom(src => src.CultureInformationModel))
              .ForMember(dest => dest.TimeZoneVM, opt => opt.MapFrom(src => src.TimeZoneModel));
            AutoMapper.Mapper.CreateMap<UserModel, UserVM>()
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleModel))
                .ForMember(dest => dest.Profile, opt => opt.MapFrom(src => src.ProfileModel))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel));
            AutoMapper.Mapper.CreateMap<CompanyModel, CompanyVM>().
                ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId.Encrypt()));

            AutoMapper.Mapper.CreateMap<LeadSourceModel, LeadSourceVM>();

            AutoMapper.Mapper.CreateMap<IndustryModel, IndustryVM>();

            AutoMapper.Mapper.CreateMap<LeadStatusModel, LeadStatusVM>();

            AutoMapper.Mapper.CreateMap<UserModel, LeadUserVM>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()));

            AutoMapper.Mapper.CreateMap<FileAttachmentModel, FileAttachmentVM>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel)).ForMember(dest => dest.CaseMessageBoardFilePath, opt => opt.MapFrom(src => ReadConfiguration.AccountDocumntPath + src.DocumentPath)).ForMember(dest => dest.AccountCaseFilePath, opt => opt.MapFrom(src => ReadConfiguration.AccountDocumntPath + src.DocumentPath)).ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserModel.FullName)).ForMember(dest => dest.UserIdEncrypt, opt => opt.MapFrom(src => src.UserModel.UserId.Encrypt())).ForMember(dest => dest.LeadFilePath, opt => opt.MapFrom(src => ReadConfiguration.LeadDocumentPath + src.DocumentPath));



            AutoMapper.Mapper.CreateMap<LeadModel, LeadVM>()
               .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt()))
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId == null ? "" : src.AccountId.Value.Encrypt()))
                 .ForMember(dest => dest.LeadOwnerName, opt => opt.MapFrom(src => src.Name == null ? "" : src.UserModel.FullName))
                 .ForMember(dest => dest.LeadOwnerImage, opt => opt.MapFrom(src => src.UserModel.ImageURL))
               .ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.StageId == null ? "" : src.StageId.Value.Encrypt()))
               .ForMember(dest => dest.RatingId, opt => opt.MapFrom(src => src.RatingId == null ? "" : src.RatingId.Value.Encrypt()))
               .ForMember(dest => dest.FinalStageId, opt => opt.MapFrom(src => src.FinalStageId == null ? "" : src.FinalStageId.Encrypt()))
                .ForMember(dest => dest.LeadTags, opt => opt.MapFrom(src => src.LeadTagsModels))
               .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
               .ForMember(dest => dest.Stage, opt => opt.MapFrom(src => src.Stage))
               .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.IndustryModel))
               .ForMember(dest => dest.LeadSource, opt => opt.MapFrom(src => src.LeadSourceModel))
               .ForMember(dest => dest.LeadStatus, opt => opt.MapFrom(src => src.LeadStatusModel))
                .ForMember(dest => dest.LeadTags, opt => opt.MapFrom(src => src.LeadTagsModels))
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel));

            AutoMapper.Mapper.CreateMap<ProductModel, ProductVM>();

            AutoMapper.Mapper.CreateMap<TaskItemModel, TaskItemVM>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Encrypt()))
            .ForMember(dest => dest.OwnerIdEncrypted, opt => opt.MapFrom(src => src.OwnerId.Encrypt()));



            AutoMapper.Mapper.CreateMap<ProductLeadAssociationModel, ProductLeadAssociationVM>()
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt()))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            AutoMapper.Mapper.CreateMap<AssociatedProductModel, AssociatedProductVM>();
            // .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductModel));

            AutoMapper.Mapper.CreateMap<TaskStatusModel, TaskStatusVM>()
                   .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => (src.StatusName != null) ? CommonFunctions.GetGlobalizedLabel(Constants.CULTURE_SPECIFIC_SHEET_LABELS, src.StatusName) : src.StatusName));

            AutoMapper.Mapper.CreateMap<ProductQuoteAssociationModel, ProductQuoteAssociationVM>()
                .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => (src.QuoteId.HasValue ? src.QuoteId.Value.Encrypt() : null)))
                .ForMember(dest => dest.AssociatedProduct, opt => opt.MapFrom(src => src.AssociatedProduct));

            AutoMapper.Mapper.CreateMap<ProductSalesOrderAssociationModel, ProductSalesOrderAssociationVM>()
        .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.HasValue ? src.SalesOrderId.Value.Encrypt() : null));

            AutoMapper.Mapper.CreateMap<ProductInvoiceAssociationModel, ProductInvoiceAssociationVM>()
        .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId.HasValue ? src.InvoiceId.Value.Encrypt() : null));

            AutoMapper.Mapper.CreateMap<AddressModel, BillingAddressVM>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryModel));

            AutoMapper.Mapper.CreateMap<AddressModel, ShippingAddressVM>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.CountryModel));

            AutoMapper.Mapper.CreateMap<QuoteModel, QuoteVM>()
                 .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.QuoteId.Encrypt()))
                 .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressModel1))
                .ForMember(dest => dest.ProductQuoteAssociations, opt => opt.MapFrom(src => src.ProductQuoteAssociationModels))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel))
                .ForMember(dest => dest.Lead, opt => opt.MapFrom(src => src.LeadModel))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel));

            AutoMapper.Mapper.CreateMap<QuoteModel, QuotesVM>()
              .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.QuoteId.Encrypt()))
              .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null))
               .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId.HasValue ? src.OwnerId.Value.Encrypt() : null));

            AutoMapper.Mapper.CreateMap<SalesOrderModel, SalesOrderVM>()
                  .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.Encrypt()))
                   .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.HasValue ? src.AccountId.Value.Encrypt() : null))
             .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.QuoteId.HasValue ? src.QuoteId.Value.Encrypt() : null))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressModel1))
                .ForMember(dest => dest.ProductSalesOrderAssociations, opt => opt.MapFrom(src => src.ProductSalesOrderAssociationModels))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.AccountModel))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel));

            AutoMapper.Mapper.CreateMap<SalesOrderModel, SaleOrdersVM>()
                .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.Encrypt()))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.HasValue ? src.AccountId.Value.Encrypt() : null))
               .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId.HasValue ? src.OwnerId.Value.Encrypt() : null));

            AutoMapper.Mapper.CreateMap<InvoiceModel, InvoiceVM>()
                .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId.Encrypt()))
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null))
                  .ForMember(dest => dest.SalesOrderId, opt => opt.MapFrom(src => src.SalesOrderId.HasValue ? src.SalesOrderId.Value.Encrypt() : null))
                .ForMember(dest => dest.Address1, opt => opt.MapFrom(src => src.AddressModel1))
                .ForMember(dest => dest.ProductInvoiceAssociations, opt => opt.MapFrom(src => src.ProductInvoiceAssociationModels))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel))
                .ForMember(dest => dest.Lead, opt => opt.MapFrom(src => src.LeadModel))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.AddressModel));
            ;

            AutoMapper.Mapper.CreateMap<InvoiceModel, InvoicesVM>()
             .ForMember(dest => dest.InvoiceId, opt => opt.MapFrom(src => src.InvoiceId.Encrypt()))
               .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null))
               .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId.HasValue ? src.OwnerId.Value.Encrypt() : null));
            AutoMapper.Mapper.CreateMap<StageModel, StageVM>()
               .ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt()))
               .ForMember(dest => dest.DefaultRatingId, opt => opt.MapFrom(src => src.DefaultRatingId.HasValue ? src.DefaultRatingId.Value.Encrypt() : null))
               .ForMember(dest => dest.IsInitialStage, opt => opt.MapFrom(src => src.IsInitialStage.HasValue ? src.IsInitialStage.Value : false))
               .ForMember(dest => dest.IsLastStage, opt => opt.MapFrom(src => src.IsLastStage.HasValue ? src.IsLastStage.Value : false));
            AutoMapper.Mapper.CreateMap<LeadStagesJSONModel, LeadStagesJSONVm>()
                .ForMember(d => d.Leads, opt => opt.MapFrom(src => src.Leads))
                 .ForMember(d => d.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt()));
            AutoMapper.Mapper.CreateMap<RatingModel, RatingVM>()
                .ForMember(d => d.RatingId, opt => opt.MapFrom(src => src.RatingId.Encrypt()));
            AutoMapper.Mapper.CreateMap<LeadAuditModel, LeadAuditVM>()
                .ForMember(d => d.StageId_encrypted, opt => opt.MapFrom(src => src.StageId.Value.Encrypt()))
                 .ForMember(d => d.LeadId_encrypted, opt => opt.MapFrom(src => src.LeadId.Value.Encrypt()))
                 .ForMember(d => d.RatingId_encrypted, opt => opt.MapFrom(src => src.RatingId.HasValue ? src.RatingId.Value.Encrypt() : ""));
            AutoMapper.Mapper.CreateMap<StageModel, StageVM>().ForMember(d => d.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt())); ;
            AutoMapper.Mapper.CreateMap<CaseMessageBoardModel, CaseMessageBoardVM>().ForMember(dest => dest.CaseMessageBoardId, opt => opt.MapFrom(src => src.CaseMessageBoardId.Encrypt())).ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => src.AccountCaseId.Encrypt()))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.User.FullName)).ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId.Encrypt()))
                .ForMember(dest => dest.CreatedByEncrypted, opt => opt.MapFrom(src => src.CreatedBy.Encrypt()))
            .ForMember(dest => dest.CreatedByUserImg, opt => opt.MapFrom(src => src.User.ImageURL));

            AutoMapper.Mapper.CreateMap<LeadTagModel, LeadTagVM>()
                 .ForMember(dest => dest.TagVM, opt => opt.MapFrom(src => src.Tag));

            AutoMapper.Mapper.CreateMap<HomeModel, HomeVM>()
                .ForMember(dest => dest.LeadAuditId, opt => opt.MapFrom(src => src.LeadAuditId == null ? "0" : src.LeadAuditId.Encrypt()))
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId == null ? "0" : src.LeadId.Value.Encrypt()))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy == null ? "0" : src.CreatedBy.Value.Encrypt()));

            AutoMapper.Mapper.CreateMap<LeadCommentModel, LeadCommentVM>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Encrypt()))
                .ForMember(dest => dest.LeadCommentId, opt => opt.MapFrom(src => src.LeadCommentId.Encrypt()))
                .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt()))
                  .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()))
            .ForMember(dest => dest.AudioPathName, opt => opt.MapFrom(src => ReadConfiguration.CommentAudioPath + src.AudioFileName));
            AutoMapper.Mapper.CreateMap<UserSettingModel, AccountSettingVM>();

            AutoMapper.Mapper.CreateMap<LeadCommentVM, LeadCommentModel>()
 .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Decrypt()))
 .ForMember(dest => dest.LeadCommentId, opt => opt.MapFrom(src => src.LeadCommentId.Decrypt()))
 .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt()))
 .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()));
            #endregion

            //#region View Model to Domain Model

            //AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.User, Domain.UserModel>()
            //    .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company));
            //AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.Company, Domain.CompanyModel>();
            //AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.RequestModel.RequestLead, Domain.LeadModel>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt())).ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.StageId.Decrypt()));
            //AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.Contact, Domain.ContactModel>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt())).ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Decrypt())).ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Decrypt()));
            //AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.Contact, Domain.LeadContactModel>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt())).ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Decrypt()));

            //AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.Account, AccountModel>()
            // .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Decrypt()));
            //AutoMapper.Mapper.CreateMap<AccountSettingVM, UserSettingModel>();



            //AutoMapper.Mapper.CreateMap<LeadComment, LeadCommentModel>()
            //    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Decrypt()))
            //    .ForMember(dest => dest.LeadCommentId, opt => opt.MapFrom(src => src.LeadCommentId.Decrypt()))
            //    .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt()))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()));
            //#endregion

          //  #region Domain Model to View Model

          //  AutoMapper.Mapper.CreateMap<Domain.CompanyModel, ErucaCRM.WCFService.Models.Company>();
          //  AutoMapper.Mapper.CreateMap<Domain.UserModel, ErucaCRM.WCFService.Models.User>().ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel)).
          //      ForMember(dest => dest.CultureDescription, opt => opt.MapFrom(src => src.CultureInformationModel.CultureName))
          //     .ForMember(dest => dest.TimeZoneDescription, opt => opt.MapFrom(src => src.TimeZoneModel.TimeZone_Location + ":" + src.TimeZoneModel.GMT))
          //      ;


          //  AutoMapper.Mapper.CreateMap<Domain.UserModel, ErucaCRM.WCFService.RequestModel.UserProfile>().//checked
          //     ForMember(dest => dest.CultureDescription, opt => opt.MapFrom(src => src.CultureInformationModel.CultureName))
          //    .ForMember(dest => dest.TimeZoneDescription, opt => opt.MapFrom(src => src.TimeZoneModel.TimeZone_Location + ":" + src.TimeZoneModel.GMT))
          //    .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.AddressModel.Street))
          //    .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.AddressModel.CountryId))
          //    .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.AddressModel.City))
          //    .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.AddressModel.Zipcode))
          //    .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png")));
          //  AutoMapper.Mapper.CreateMap<Domain.TimeZoneModal, ErucaCRM.WCFService.TimeZone>()
          //       .ForMember(dest => dest.TimeZoneDescription, opt => opt.MapFrom(src => src.TimeZone_Location + src.GMT));
          //  AutoMapper.Mapper.CreateMap<Domain.CultureInformationModel, ErucaCRM.WCFService.CultureInformation>()
          //       .ForMember(dest => dest.CultureInformationDescription, opt => opt.MapFrom(src => src.Language + "(" + src.CultureName + ")"));
          //  AutoMapper.Mapper.CreateMap<Domain.LeadModel, ErucaCRM.WCFService.Models.Lead>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt())).ForMember(dest => dest.RatingImage, opt => opt.MapFrom(src => src.Rating.Icons)).ForMember(dest => dest.LeadOwnerName, opt => opt.MapFrom(src => src.UserModel.FullName)).ForMember(dest => dest.RatingConstant, opt => opt.MapFrom(src => src.Rating.RatingConstant));
          //  AutoMapper.Mapper.CreateMap<Domain.UserModel, ErucaCRM.WCFService.Models.Users>().ForMember(dest => dest.UserIdEncrypted, opt => opt.MapFrom(src => src.UserId.Encrypt()));
          //  AutoMapper.Mapper.CreateMap<Domain.LeadStagesJSONModel, ErucaCRM.WCFService.Models.LeadStagesJSON>();
          //  AutoMapper.Mapper.CreateMap<Domain.StageModel, ErucaCRM.WCFService.Models.Stages>().ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt()))
          //     .ForMember(dest => dest.DefaultRatingId, opt => opt.MapFrom(src => src.DefaultRatingId.HasValue ? src.DefaultRatingId.Value.Encrypt() : null))
          //     .ForMember(dest => dest.IsInitialStage, opt => opt.MapFrom(src => src.IsInitialStage.HasValue ? src.IsInitialStage.Value : false))
          //     .ForMember(dest => dest.IsLastStage, opt => opt.MapFrom(src => src.IsLastStage.HasValue ? src.IsLastStage.Value : false));

          //  AutoMapper.Mapper.CreateMap<Domain.ContactModel, ErucaCRM.WCFService.Models.Contact>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null)).ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()));
          //  AutoMapper.Mapper.CreateMap<Domain.RatingModel, ErucaCRM.WCFService.Models.Rating>();
          //  AutoMapper.Mapper.CreateMap<Domain.OwnerListModel, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.OwnerId));

          //  AutoMapper.Mapper.CreateMap<Domain.TaskStatusModel, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.StatusName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.TaskStatusId));

          //  AutoMapper.Mapper.CreateMap<Priority, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.PriorityName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.PriorityId));

          //  AutoMapper.Mapper.CreateMap<AssociatedModuleResponses, ErucaCRM.WCFService.Models.DDLEncrypt>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.value)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.Encrypt()));

          //  AutoMapper.Mapper.CreateMap<ModuleModel, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.ModuleName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ModuleId));

          //  AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.TimeZone, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.TimeZoneDescription)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.TimeZoneId));

          //  AutoMapper.Mapper.CreateMap<CountryModel, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CountryName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.CountryId));


          //  AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.CultureInformation, ErucaCRM.WCFService.Models.DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CultureInformationDescription)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.CultureInformationId));

          //  AutoMapper.Mapper.CreateMap<Domain.LeadAuditModel, ErucaCRM.WCFService.Models.LeadAudit>();

          //  AutoMapper.Mapper.CreateMap<Domain.TaskItemModel, ErucaCRM.WCFService.Models.TaskItem>()
          //      .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Encrypt()))
          //     .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate == null ? "" : Convert.ToString(src.DueDate.ToString("dd/MM/yyyy"))))
          //        .ForMember(dest => dest.AssociatedModuleValue, opt => opt.MapFrom(src => src.AssociatedModuleValue.Encrypt()))
          //        ;
          //  ///Today Activity mapping  
          //  AutoMapper.Mapper.CreateMap<Domain.DashboarActivitiesModel, ErucaCRM.WCFService.Models.TaskItem>()
          //      .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Encrypt()))
          //      .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate == null ? "" : string.Format("{0:dd/MM/yyyy}", src.DueDate)))
          //      .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png"))); //(string.IsNullOrEmpty(src.User.ImageURL) ? src.User.ImageURL : "noimage.png"  )));

          //  AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.TaskItem, Domain.TaskItemModel>()
          //      .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Decrypt()))
          //      .ForMember(dest => dest.AssociatedModuleValue, opt => opt.MapFrom(src => src.AssociatedModuleValue.Decrypt()))
          //   .ForMember(dest => dest.DueDate, opt => opt.Ignore());



          //  AutoMapper.Mapper.CreateMap<Domain.FileAttachmentModel, ErucaCRM.WCFService.Models.FileAttachment>()
          //      .ForMember(dest => dest.DocumentPath, opt => opt.MapFrom(src => ReadConfiguration.LeadDocumentPath + src.DocumentPath))
          //      .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? "" : Convert.ToString(src.CreatedDate.Value.ToString("dd/MM/yyyy"))));
          //  AutoMapper.Mapper.CreateMap<ErucaCRM.WCFService.Models.FileAttachment, FileAttachmentModel>()
          //       .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountId)) ? src.AccountId.Decrypt() : (int?)null))
          //      .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.ContactId)) ? src.ContactId.Decrypt() : (int?)null))
          //        .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.LeadId)) ? src.LeadId.Decrypt() : (int?)null))
          //         .ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountCaseId)) ? src.AccountCaseId.Decrypt() : (int?)null))
          //      .ForMember(dest => dest.UserModel, opt => opt.Ignore());


          //  AutoMapper.Mapper.CreateMap<AccountModel, ErucaCRM.WCFService.Models.Account>()
          //     .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Encrypt()))
          //     .ForMember(dest => dest.AccountOwnerId, opt => opt.MapFrom(src => src.AccountOwnerId.Encrypt()))
          //     .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountTypeModel.AccountTypeName))
          //     .ForMember(dest => dest.AccountOwner, opt => opt.MapFrom(src => src.UserModel.FullName));

          //  AutoMapper.Mapper.CreateMap<HomeModel, ErucaCRM.WCFService.Models.HomeRecentActivites>()//cheked
          //      .ForMember(dest => dest.LeadAuditId, opt => opt.MapFrom(src => src.LeadAuditId.Encrypt()))
          //      .ForMember(dest => dest.ImageFullPath, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png")));


          //  AutoMapper.Mapper.CreateMap<HomeModel, ErucaCRM.WCFService.Models.DashboardData>()
          //    .ForMember(dest => dest.LeadsInStages, opt => opt.MapFrom(src => src.LeadsInStages ?? 0))
          //    .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.TotalRevenue ?? 0))
          //    .ForMember(dest => dest.TotalLead, opt => opt.MapFrom(src => src.TotalLead ?? 0));

          //  AutoMapper.Mapper.CreateMap<HomeModel, ErucaCRM.WCFService.Models.Notification>()
          //  .ForMember(dest => dest.LeadAuditId, opt => opt.MapFrom(src => src.LeadAuditId.Encrypt()))
          //  .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png")));


          //  AutoMapper.Mapper.CreateMap<LeadStagesJSONModel, ErucaCRM.WCFService.Models.LeadStagesJSON>()
          //       .ForMember(d => d.Leads, opt => opt.MapFrom(src => src.Leads))
          //       .ForMember(d => d.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt()));

          //  AutoMapper.Mapper.CreateMap<LeadContactModel, ErucaCRM.WCFService.Models.Contact>()
          //    .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()))
          //  .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.Contact.ContactName))
          //  .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Contact.EmailAddress))
          //  .ForMember(dest => dest.ContactCompanyName, opt => opt.MapFrom(src=> src.Contact.ContactCompanyName))
          //  .ForMember(dest =>dest.OwnerName, opt => opt.MapFrom(src => src.Contact.OwnerName))
          //  .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Contact.Phone))
          //  .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Contact.Mobile))
          //  .ForMember(dest => dest.OtherPhone, opt => opt.MapFrom(src => src.Contact.OtherPhone))
          //  .ForMember(dest => dest.HomePhone, opt => opt.MapFrom(src => src.Contact.HomePhone))
          //  .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Contact.AccountId))
          //  .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
          //  .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName));

          //  AutoMapper.Mapper.CreateMap<TaskItemModel, ErucaCRM.WCFService.Models.TaskItem>()
          //     .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId.Encrypt()));
          //    //.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
          //    //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
          //    //.ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
          //    //.ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId))
          //    //.ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
          //    //.ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.PriorityName))
          //    //.ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus))
          //    //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
          //    //.ForMember(dest => dest.AssociatedModuleId, opt => opt.MapFrom(src => src.AssociatedModuleId))
          //    //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

          //  AutoMapper.Mapper.CreateMap<ContactModel, ErucaCRM.WCFService.Models.Contact>()
          //   .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()));

          //  AutoMapper.Mapper.CreateMap<AppVersionModel, ErucaCRM.Web.Models.AppVersion>()
          //      .ForMember(dest => dest.AppId, opt => opt.MapFrom(src => src.KeyId))
          //       .ForMember(dest => dest.VersionName, opt => opt.MapFrom(src => src.KeyName))
          //      .ForMember(dest=>dest.VersionCode,opt=>opt.MapFrom(src=>src.KeyValue));

          //  //AutoMapper.Mapper.CreateMap<FileAttachmentModel, FileAttachmentVM>()
          //  //  .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel)).ForMember(dest => dest.CaseMessageBoardFilePath, opt => opt.MapFrom(src => Constants.ACCOUNT_DOCS_PATH + src.DocumentPath)).ForMember(dest => dest.AccountCaseFilePath, opt => opt.MapFrom(src => Constants.ACCOUNT_DOCS_PATH + src.DocumentPath)).ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserModel.FullName)).ForMember(dest => dest.UserIdEncrypt, opt => opt.MapFrom(src => src.UserModel.UserId.Encrypt())).ForMember(dest => dest.LeadFilePath, opt => opt.MapFrom(src => Constants.LEAD_DOCS_PATH + src.DocumentPath));
          //  AutoMapper.Mapper.CreateMap<LeadCommentModel, LeadComment>()
          //    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Encrypt()))
          //    .ForMember(dest => dest.LeadCommentId, opt => opt.MapFrom(src => src.LeadCommentId.Encrypt()))
          //    .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt()))
          //      .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()))
          //.ForMember(dest => dest.AudioFileName, opt => opt.MapFrom(src => ReadConfiguration.CommentAudioPath + src.AudioFileName));
           
          //  #endregion

        }
    }
}