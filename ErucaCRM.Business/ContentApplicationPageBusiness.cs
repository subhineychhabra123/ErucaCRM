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
    public class ContentApplicationPageBusiness : IContentApplicationPageBusiness
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly ContentApplicationPageRepository contentApplicationPageRepository;

        public ContentApplicationPageBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            contentApplicationPageRepository = new ContentApplicationPageRepository(unitOfWork);

        }
        public CultureSpecificSiteContentModel GetPageCultureSpecificContent(string sitePageName)
        {
            string cultureName = CultureInformationManagement.CurrentUserCulture;
            CultureSpecificSiteContentModel pageCultureSpecificSiteContentModel = new CultureSpecificSiteContentModel();
          

            
ContentApplicationPage contentApplicationPage = contentApplicationPageRepository.GetAll(x => x.ApplicationPage.PageTitle == sitePageName && ((x.CultureInformation.CultureName == cultureName && x.UseDefaultContent == false))).FirstOrDefault();

            //if user culture specific page is not configured to use default content then 
            if (contentApplicationPage == null)
            {
                string defaultCultureName = CultureInformationManagement.ApplicationDefaultCulture;
                contentApplicationPage = contentApplicationPageRepository.GetAll(x => x.ApplicationPage.PageTitle == sitePageName && (x.CultureInformation.CultureName == defaultCultureName)).FirstOrDefault();
            }

            // otherwise use default laguage specific content for current user culture
            if (contentApplicationPage != null)
            {
                pageCultureSpecificSiteContentModel.CultureSpecificPageMetaTitle = contentApplicationPage.MetaTitle;
                pageCultureSpecificSiteContentModel.CultureSpecificPageMetaTags = contentApplicationPage.MetaDescription;
                pageCultureSpecificSiteContentModel.CultureSpecificPageContent = contentApplicationPage.PageContent;

            }


            return pageCultureSpecificSiteContentModel;

        }



    }
}
