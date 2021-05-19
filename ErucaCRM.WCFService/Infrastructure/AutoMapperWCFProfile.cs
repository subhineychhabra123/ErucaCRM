using ErucaCRM.Business.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.WCFService;
using Domain = ErucaCRM.Domain;
using ErucaCRM.Utility;
using ErucaCRM.Utility.WebClasses;
using ErucaCRM.Domain;
using ErucaCRM.Mobile.Models.Model;
using ErucaCRM.Mobile.Models.RequestModel;
using ErucaCRM.Mobile.Models.ResponseModel;

namespace ErucaCRM.WCFService.Infrastructure
{
    public class AutoMapperWCFProfile : AutoMapper.Profile
    {
        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutoMapperWCFProfile>();
                a.AddProfile<AutoMapperBusinessProfile>();
            });

        }

        protected override void Configure()
        {
            base.Configure();

            #region View Model to Domain Model

            AutoMapper.Mapper.CreateMap<User, Domain.UserModel>()
                .ForMember(dest => dest.CompanyModel, opt => opt.MapFrom(src => src.Company));
            AutoMapper.Mapper.CreateMap<Company, Domain.CompanyModel>();
            AutoMapper.Mapper.CreateMap<RequestLead, Domain.LeadModel>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt())).ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.StageId.Decrypt()));
            AutoMapper.Mapper.CreateMap<Contact, Domain.ContactModel>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt())).ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Decrypt())).ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Decrypt()));
            AutoMapper.Mapper.CreateMap<Contact, Domain.LeadContactModel>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt())).ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Decrypt()));

            AutoMapper.Mapper.CreateMap<Account, AccountModel>()
             .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Decrypt()));
            //AutoMapper.Mapper.CreateMap<AccountSettingVM, UserSettingModel>();

            //AutoMapper.Mapper.CreateMap<LeadCommentVM, LeadCommentModel>()
            // .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Decrypt()))
            // .ForMember(dest => dest.LeadCommentId, opt => opt.MapFrom(src => src.LeadCommentId.Decrypt()))
            // .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Decrypt()))
            // .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Decrypt()));
   

            #endregion

            #region Domain Model to View Model

            AutoMapper.Mapper.CreateMap<Domain.CompanyModel, Company>();
            AutoMapper.Mapper.CreateMap<Domain.UserModel, User>().ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.CompanyModel)).
                ForMember(dest => dest.CultureDescription, opt => opt.MapFrom(src => src.CultureInformationModel.CultureName))
               .ForMember(dest => dest.TimeZoneDescription, opt => opt.MapFrom(src => src.TimeZoneModel.TimeZone_Location + ":" + src.TimeZoneModel.GMT))
                ;


            AutoMapper.Mapper.CreateMap<Domain.UserModel, UserProfile>().//checked
               ForMember(dest => dest.CultureDescription, opt => opt.MapFrom(src => src.CultureInformationModel.CultureName))
              .ForMember(dest => dest.TimeZoneDescription, opt => opt.MapFrom(src => src.TimeZoneModel.TimeZone_Location + ":" + src.TimeZoneModel.GMT))
              .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.AddressModel.Street))
              .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.AddressModel.CountryId))
              .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.AddressModel.City))
              .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.AddressModel.Zipcode))
              .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png")));
            AutoMapper.Mapper.CreateMap<Domain.TimeZoneModal, ErucaCRM.Mobile.Models.ResponseModel.TimeZone>()
                 .ForMember(dest => dest.TimeZoneDescription, opt => opt.MapFrom(src => src.TimeZone_Location + src.GMT));
            AutoMapper.Mapper.CreateMap<Domain.CultureInformationModel, CultureInformation>()
                 .ForMember(dest => dest.CultureInformationDescription, opt => opt.MapFrom(src => src.Language + "(" + src.CultureName + ")"));
            AutoMapper.Mapper.CreateMap<Domain.LeadModel, Lead>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt())).ForMember(dest => dest.RatingImage, opt => opt.MapFrom(src => src.Rating.Icons)).ForMember(dest => dest.LeadOwnerName, opt => opt.MapFrom(src => src.UserModel.FullName)).ForMember(dest => dest.RatingConstant, opt => opt.MapFrom(src => src.Rating.RatingConstant));
            AutoMapper.Mapper.CreateMap<Domain.UserModel, Users>().ForMember(dest => dest.UserIdEncrypted, opt => opt.MapFrom(src => src.UserId.Encrypt()));
            AutoMapper.Mapper.CreateMap<Domain.LeadStagesJSONModel, LeadStagesJSON>();
            AutoMapper.Mapper.CreateMap<Domain.StageModel, Stages>().ForMember(dest => dest.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt()))
               .ForMember(dest => dest.DefaultRatingId, opt => opt.MapFrom(src => src.DefaultRatingId.HasValue ? src.DefaultRatingId.Value.Encrypt() : null))
               .ForMember(dest => dest.IsInitialStage, opt => opt.MapFrom(src => src.IsInitialStage.HasValue ? src.IsInitialStage.Value : false))
               .ForMember(dest => dest.IsLastStage, opt => opt.MapFrom(src => src.IsLastStage.HasValue ? src.IsLastStage.Value : false));

            AutoMapper.Mapper.CreateMap<Domain.ContactModel, Contact>().ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.HasValue ? src.LeadId.Value.Encrypt() : null)).ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()));
            AutoMapper.Mapper.CreateMap<Domain.RatingModel, Rating>();
            AutoMapper.Mapper.CreateMap<Domain.OwnerListModel, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.OwnerId));

            AutoMapper.Mapper.CreateMap<Domain.TaskStatusModel, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.StatusName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.TaskStatusId));

            AutoMapper.Mapper.CreateMap<Priority, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.PriorityName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.PriorityId));

            AutoMapper.Mapper.CreateMap<AssociatedModuleResponses, DDLEncrypt>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.value)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.Encrypt()));

            AutoMapper.Mapper.CreateMap<ModuleModel, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.ModuleName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ModuleId));

            AutoMapper.Mapper.CreateMap<ErucaCRM.Mobile.Models.ResponseModel.TimeZone, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.TimeZoneDescription)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.TimeZoneId));

            AutoMapper.Mapper.CreateMap<CountryModel, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CountryName)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.CountryId));


            AutoMapper.Mapper.CreateMap<CultureInformation, DDL>().ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CultureInformationDescription)).ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.CultureInformationId));

            AutoMapper.Mapper.CreateMap<Domain.LeadAuditModel, LeadAudit>();

            AutoMapper.Mapper.CreateMap<Domain.TaskItemModel, TaskItem>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Encrypt()))
               .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate == null ? "" : Convert.ToString(src.DueDate.ToString("MM/dd/yyyy"))))
                  .ForMember(dest => dest.AssociatedModuleValue, opt => opt.MapFrom(src => src.AssociatedModuleValue.Encrypt()))
                  ;
            ///Today Activity mapping  
            AutoMapper.Mapper.CreateMap<Domain.DashboarActivitiesModel, TaskItem>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Encrypt()))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate == null ? "" : string.Format("{0:MM/dd/yyyy}", src.DueDate)))
                .ForMember(dest => dest.UserImage, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png"))); //(string.IsNullOrEmpty(src.User.ImageURL) ? src.User.ImageURL : "noimage.png"  )));

            AutoMapper.Mapper.CreateMap<TaskItem, Domain.TaskItemModel>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId.Decrypt()))
                .ForMember(dest => dest.AssociatedModuleValue, opt => opt.MapFrom(src => src.AssociatedModuleValue.Decrypt()))
             .ForMember(dest => dest.DueDate, opt =>opt.MapFrom(src=>src.DueDate.ConvertToDateByCulture(src.CultureName)))
              .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src =>Convert.ToInt32(src.OwnerId)));



            AutoMapper.Mapper.CreateMap<Domain.FileAttachmentModel, FileAttachment>()
                .ForMember(dest => dest.DocumentPath, opt => opt.MapFrom(src => ReadConfiguration.LeadDocumentPath + src.DocumentPath))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate == null ? "" : Convert.ToString(src.CreatedDate.Value.ToString("dd/MM/yyyy"))));
            AutoMapper.Mapper.CreateMap<FileAttachment, FileAttachmentModel>()
                 .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountId)) ? src.AccountId.Decrypt() : (int?)null))
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.ContactId)) ? src.ContactId.Decrypt() : (int?)null))
                  .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.LeadId)) ? src.LeadId.Decrypt() : (int?)null))
                   .ForMember(dest => dest.AccountCaseId, opt => opt.MapFrom(src => !(string.IsNullOrEmpty(src.AccountCaseId)) ? src.AccountCaseId.Decrypt() : (int?)null))
                .ForMember(dest => dest.UserModel, opt => opt.Ignore());


            AutoMapper.Mapper.CreateMap<AccountModel, Account>()
               .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId.Encrypt()))
               .ForMember(dest => dest.AccountOwnerId, opt => opt.MapFrom(src => src.AccountOwnerId.Encrypt()))
               .ForMember(dest => dest.AccountTypeName, opt => opt.MapFrom(src => src.AccountTypeModel.AccountTypeName))
               .ForMember(dest => dest.AccountOwner, opt => opt.MapFrom(src => src.UserModel.FullName));

            AutoMapper.Mapper.CreateMap<HomeModel, HomeRecentActivites>()//cheked
                .ForMember(dest => dest.LeadAuditId, opt => opt.MapFrom(src => src.LeadAuditId.Encrypt()))
                .ForMember(dest => dest.ImageFullPath, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png")));


            AutoMapper.Mapper.CreateMap<HomeModel, DashboardData>()
              .ForMember(dest => dest.LeadsInStages, opt => opt.MapFrom(src => src.LeadsInStages ?? 0))
              .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.TotalRevenue ?? 0))
              .ForMember(dest => dest.TotalLead, opt => opt.MapFrom(src => src.TotalLead ?? 0));

            AutoMapper.Mapper.CreateMap<HomeModel, Notification>()
            .ForMember(dest => dest.LeadAuditId, opt => opt.MapFrom(src => src.LeadAuditId.Encrypt()))
            .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => ReadConfiguration.UsersImageUrl + (src.ImageURL ?? "no_image.png")));


            AutoMapper.Mapper.CreateMap<LeadStagesJSONModel, LeadStagesJSON>()
                 .ForMember(d => d.Leads, opt => opt.MapFrom(src => src.Leads))
                 .ForMember(d => d.StageId, opt => opt.MapFrom(src => src.StageId.Encrypt()));

            AutoMapper.Mapper.CreateMap<LeadContactModel, Contact>()
              .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()))
            .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.Contact.ContactName))
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Contact.EmailAddress))
            .ForMember(dest => dest.ContactCompanyName, opt => opt.MapFrom(src => src.Contact.ContactCompanyName))
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Contact.OwnerName))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Contact.Phone))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.Contact.Mobile))
            .ForMember(dest => dest.OtherPhone, opt => opt.MapFrom(src => src.Contact.OtherPhone))
            .ForMember(dest => dest.HomePhone, opt => opt.MapFrom(src => src.Contact.HomePhone))
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Contact.AccountId))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Contact.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Contact.LastName));

            AutoMapper.Mapper.CreateMap<TaskItemModel, TaskItem>()
               .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId.Encrypt()));
            //.ForMember(dest => dest.Subject, opt => opt.MapFrom(src => src.Subject))
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            //.ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            //.ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId))
            //.ForMember(dest => dest.PriorityId, opt => opt.MapFrom(src => src.PriorityId))
            //.ForMember(dest => dest.PriorityName, opt => opt.MapFrom(src => src.PriorityName))
            //.ForMember(dest => dest.TaskStatus, opt => opt.MapFrom(src => src.TaskStatus))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            //.ForMember(dest => dest.AssociatedModuleId, opt => opt.MapFrom(src => src.AssociatedModuleId))
            //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

            AutoMapper.Mapper.CreateMap<ContactModel, Contact>()
             .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId.Encrypt()));

            AutoMapper.Mapper.CreateMap<AppVersionModel, AppVersion>()
                .ForMember(dest => dest.AppId, opt => opt.MapFrom(src => src.KeyId))
                 .ForMember(dest => dest.VersionName, opt => opt.MapFrom(src => src.KeyName))
                .ForMember(dest => dest.VersionCode, opt => opt.MapFrom(src => src.KeyValue));

            //AutoMapper.Mapper.CreateMap<FileAttachmentModel, FileAttachmentVM>()
            //  .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.UserModel)).ForMember(dest => dest.CaseMessageBoardFilePath, opt => opt.MapFrom(src => Constants.ACCOUNT_DOCS_PATH + src.DocumentPath)).ForMember(dest => dest.AccountCaseFilePath, opt => opt.MapFrom(src => Constants.ACCOUNT_DOCS_PATH + src.DocumentPath)).ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserModel.FullName)).ForMember(dest => dest.UserIdEncrypt, opt => opt.MapFrom(src => src.UserModel.UserId.Encrypt())).ForMember(dest => dest.LeadFilePath, opt => opt.MapFrom(src => Constants.LEAD_DOCS_PATH + src.DocumentPath));
            AutoMapper.Mapper.CreateMap<LeadCommentModel, LeadComment>()
              .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Encrypt()))
              .ForMember(dest => dest.LeadCommentId, opt => opt.MapFrom(src => src.LeadCommentId.Encrypt()))
              .ForMember(dest => dest.LeadId, opt => opt.MapFrom(src => src.LeadId.Encrypt()))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.Encrypt()))
          .ForMember(dest => dest.AudioFileName, opt => opt.MapFrom(src => ReadConfiguration.CommentAudioPath + src.AudioFileName));

            #endregion

        }
    }
}