using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;

namespace ErucaCRM.Business
{
    public class ApplicationPageBusiness : IApplicationPageBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationPageRepository applicationPageRepository;
        private readonly ContentApplicationPageRepository contentApplicationRepository;
        private readonly AssociationApplicationPageRepository associationApplicationRepository;
        private readonly CultureInformationRepository cultureInformationPageRepository;
        public ApplicationPageBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            applicationPageRepository = new ApplicationPageRepository(unitOfWork);
            contentApplicationRepository = new ContentApplicationPageRepository(unitOfWork);
            associationApplicationRepository = new AssociationApplicationPageRepository(unitOfWork);
            cultureInformationPageRepository = new CultureInformationRepository(unitOfWork);
        }


        public ApplicationPageModel GetPageDetail(int cultureInformationId, int pageId)
        {
            ApplicationPage page = applicationPageRepository.SingleOrDefault(r => r.ApplicationPageId == pageId && r.RecordDeleted == false);
            ApplicationPageModel pageModel = new ApplicationPageModel();
            AutoMapper.Mapper.Map(page, pageModel);
            ContentApplicationPage contentpage = page.ContentApplicationPages.SingleOrDefault(r => r.CultureInformationId == Convert.ToInt32(cultureInformationId) && r.ApplicationPageId == pageId);
            if (contentpage != null)
            {
                ContentApplicationPageModel contentpageModel = new ContentApplicationPageModel();
                AutoMapper.Mapper.Map(contentpage, contentpageModel);
                pageModel.ContentApplicationPage = contentpageModel;
            }
            return pageModel;

        }

        public List<AssociationApplicationPageModel> GetCustomPages1(int applicationPageId, int cultureInformationId, bool viewAll)
        {
            List<AssociationApplicationPageModel> associationApplicationPageModel = new List<AssociationApplicationPageModel>();

            List<AssociationApplicationPage> associationApplicationPage = associationApplicationRepository.GetAll(r => r.ApplicationPage1.IsApplicationPage == false && r.ApplicationPageId == applicationPageId).ToList();

            for (int i = 0; i < associationApplicationPage.Count; i++)
            {
                associationApplicationPage[i].ApplicationPage1.ContentApplicationPages = associationApplicationPage[i].ApplicationPage1.ContentApplicationPages.Where(x => x.CultureInformationId == cultureInformationId).ToList();

            }


            AutoMapper.Mapper.Map(associationApplicationPage, associationApplicationPageModel);
            return associationApplicationPageModel;

        }

        public List<ContentApplicationPageModel> GetCustomPages(int applicationPageId, int cultureInformationId, bool viewAll)
        {
            List<ContentApplicationPageModel> listcontentApplicationPageModel = new List<ContentApplicationPageModel>();


            List<ContentApplicationPage> listContentApplicationPage;

            listContentApplicationPage = contentApplicationRepository.GetAll(x => x.ApplicationPage.IsApplicationPage == false && x.CultureInformationId == cultureInformationId).ToList();


            //else
            //{
            //    listContentApplicationPage = contentApplicationRepository.GetAll(x => x.ApplicationPage.IsApplicationPage == false && x.CultureInformationId == cultureInformationId && x.ApplicationPage.AssociationApplicationPages1 != null && x.ApplicationPage.AssociationApplicationPages1.Where(y => y.ApplicationPageId == applicationPageId).Count() > 0).ToList();
            //}




            AutoMapper.Mapper.Map(listContentApplicationPage, listcontentApplicationPageModel);
            return listcontentApplicationPageModel;

        }


        public bool RemoveCustomPage(int applicationPageId, int customPageId)
        {
            try
            {
                associationApplicationRepository.Delete(x => x.ApplicationPageId == applicationPageId && x.CustomPageId == customPageId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
        public bool AddCustomPage(int applicationPageId, int customPageId)
        {
            try
            {
                AssociationApplicationPage associationApplicationPage = new AssociationApplicationPage();
                associationApplicationPage.ApplicationPageId = applicationPageId;
                associationApplicationPage.CustomPageId = customPageId;
                associationApplicationRepository.Insert(associationApplicationPage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public void UpdatePageContent(ApplicationPageModel pageModel)
        {
            ContentApplicationPage contentpage = new ContentApplicationPage();
            ApplicationPage page = new ApplicationPage();
            page = applicationPageRepository.SingleOrDefault(r => r.ApplicationPageId == pageModel.ApplicationPageId);
            if (page != null)
            {
                page.PageTitle = pageModel.PageTitle;
                page.PageUrl = pageModel.PageUrl;
                page.IsActive = pageModel.IsActive;
                page.PageDescription = pageModel.PageDescription;
                applicationPageRepository.Update(page);
            }
            if (pageModel.ContentApplicationPage.UseDefaultContent == true)
            {
                contentpage = contentApplicationRepository.SingleOrDefault(r => r.ApplicationPageId == pageModel.ApplicationPageId && r.CultureInformationId == pageModel.ContentApplicationPage.CultureInformationId);
                CultureInformation cultureInformation = cultureInformationPageRepository.SingleOrDefault(r => r.IsDefault == true);
                ContentApplicationPage defaultContentPage = new ContentApplicationPage();
                defaultContentPage = contentApplicationRepository.SingleOrDefault(r => r.CultureInformationId == cultureInformation.CultureInformationId && r.ApplicationPageId == pageModel.ApplicationPageId);
                contentpage.MetaDescription = defaultContentPage.MetaDescription;
                contentpage.PageContent = defaultContentPage.PageContent;
                contentpage.MetaTitle = defaultContentPage.MetaTitle;
                contentpage.UseDefaultContent = true;
                contentApplicationRepository.Update(contentpage);
            }
            else
            {
                contentpage = contentApplicationRepository.SingleOrDefault(r => r.ApplicationPageId == pageModel.ApplicationPageId && r.CultureInformationId == pageModel.ContentApplicationPage.CultureInformationId);
                if (contentpage != null)
                {
                    contentpage.MetaDescription = pageModel.ContentApplicationPage.MetaDescription;
                    contentpage.PageContent = pageModel.ContentApplicationPage.PageContent;
                    contentpage.MetaTitle = pageModel.ContentApplicationPage.MetaTitle;
                    contentpage.UseDefaultContent = false;
                    contentApplicationRepository.Update(contentpage);
                }
            }
        }

        public List<ApplicationPageModel> GetPublicPages()
        {
            List<ApplicationPageModel> applicationPageModel = new List<ApplicationPageModel>();
            List<ApplicationPage> applicationPage = new List<ApplicationPage>();
            applicationPage = applicationPageRepository.GetAll(r => r.IsApplicationPage == true).ToList();
            AutoMapper.Mapper.Map(applicationPage, applicationPageModel);
            return applicationPageModel;

        }


        public void AddCustomPage(ApplicationPageModel pageModel)
        {
            ContentApplicationPage contentpage;
            ContentApplicationPage defaultContentData = new ContentApplicationPage();
            int defaultCultureInformationId = cultureInformationPageRepository.SingleOrDefault(r => r.IsDefault == true).CultureInformationId;
            defaultContentData = contentApplicationRepository.SingleOrDefault(r => r.CultureInformationId == defaultCultureInformationId && r.ApplicationPageId == pageModel.ApplicationPageId);
            ApplicationPage page = new ApplicationPage();
            page.PageTitle = pageModel.PageTitle;
            page.PageDescription = pageModel.PageDescription;
            page.PageUrl = pageModel.PageUrl;
            page.IsApplicationPage = false;
            page.IsActive = pageModel.IsActive;
            applicationPageRepository.Insert(page);
            int ApplicationPageId = page.ApplicationPageId;

            //Code for making an entry for custom page for all the visible language.
            List<CultureInformation> cultureInformationList = cultureInformationPageRepository.GetAll(r => r.IsVisible == true).ToList();
            foreach (CultureInformation cultureInfo in cultureInformationList)
            {
                contentpage = new ContentApplicationPage();
                if (cultureInfo.CultureInformationId == pageModel.ContentApplicationPage.CultureInformationId)
                {
                    contentpage.ApplicationPageId = ApplicationPageId;
                    contentpage.CultureInformationId = pageModel.ContentApplicationPage.CultureInformationId;
                    contentpage.MetaTitle = pageModel.ContentApplicationPage.MetaTitle;
                    contentpage.PageContent = pageModel.ContentApplicationPage.PageContent;
                    contentpage.MetaDescription = pageModel.ContentApplicationPage.MetaDescription;
                    contentpage.UseDefaultContent = pageModel.ContentApplicationPage.UseDefaultContent;
                    contentApplicationRepository.Insert(contentpage);
                }
                else
                {
                    if (defaultContentData != null)
                    {
                        contentpage.ApplicationPageId = ApplicationPageId;
                        contentpage.CultureInformationId = cultureInfo.CultureInformationId;
                        contentpage.MetaTitle = defaultContentData.MetaTitle;
                        contentpage.PageContent = defaultContentData.PageContent;
                        contentpage.MetaDescription = defaultContentData.MetaDescription;
                        contentpage.UseDefaultContent = true;
                        contentApplicationRepository.Insert(contentpage);
                    }
                    else
                    {
                        contentpage.ApplicationPageId = ApplicationPageId;
                        contentpage.CultureInformationId = cultureInfo.CultureInformationId;
                        contentpage.MetaTitle = string.Empty;
                        contentpage.PageContent = string.Empty;
                        contentpage.MetaDescription = string.Empty;
                        contentpage.UseDefaultContent = false;
                        contentApplicationRepository.Insert(contentpage);
                    }

                }
            }

            AssociationApplicationPage associationApplication = new AssociationApplicationPage();
            associationApplication.ApplicationPageId = pageModel.ApplicationPageId;
            associationApplication.CustomPageId = ApplicationPageId;
            associationApplicationRepository.Insert(associationApplication);


        }


        public void DeleteCustomPage(int applicationPageId)
        {
            contentApplicationRepository.Delete(r => r.ApplicationPageId == applicationPageId);
            associationApplicationRepository.Delete(r => r.CustomPageId == applicationPageId);
            applicationPageRepository.Delete(r => r.ApplicationPageId == applicationPageId);

        }
    }
}
