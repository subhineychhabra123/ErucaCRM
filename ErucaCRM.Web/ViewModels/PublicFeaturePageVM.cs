using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "PublicPageFeature")]
    public class PublicFeaturePageVM : CultureSpecificSiteContentVM
    {

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }

        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }
        public PageContent PageContents
        {
            get
            {
                return new PageContent();
            }

        }

        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }
        public PageLinkButton PageLinkButtons
        {
            get
            {
                return new PageLinkButton();
            }

        }

        public class PageSubHeader
        {

            public string PageSubHeaderCRMFeatures { get; set; }
            public string PageSubHeaderDashboard { get; set; }
            public string PageSubHeaderTagManagement { get; set; }
            public string PageSubHeaderCustomerRelationship { get; set; }
            public string PageSubHeaderLeadsOpportunities { get; set; }
          
            public string PageSubHeaderAllFeatures { get; set; }


        }

        public class PageContent
        {

            public string DashBoardContent { get; set; }
            public string TagManagementContent { get; set; }
            public string CustomerRelationshipContent { get; set; }
            public string LeadsOpportunitiesContent { get; set; }
            public string TabDashBordPara1Content{ get; set; }
            public string TabDashBordPara2Content { get; set; }
            public string TabTagManagementContent { get; set; }
            public string TabCustomerRelationshipContent { get; set; }
            public string TabLeadsContent { get; set; }
            public string TabOtherFeaturesContent { get; set; }
            public string TabContactManagementContent { get; set; }
            public string BenefitsDashbordContent { get; set; }
            public string BenefitsTagManagementContent { get; set; }
            public string BenefitsCustomerRelationshipContent { get; set; }
            public string BenefitsLeadsContent { get; set; }
            public string BenefitsOtherFeaturesContent { get; set; }
            public string BenefitsContactManagementContent { get; set; }


        }

        public class PageLabel
        {
            public string TabDashBord { get; set; }
            public string TabTagManagement { get; set; }
            public string TabCustomerRelationship { get; set; }
            public string TabLeads { get; set; }
            public string TabOtherFeatures { get; set; }
            public string TabContactManagement { get; set; }
            public string BenefitsDashbord { get; set; }
            public string BenefitsTagManagement { get; set; }
            public string BenefitsCustomerRelationship { get; set; }
            public string BenefitsLeads { get; set; }
            public string BenefitsOtherFeatures { get; set; }
            public string BenefitsContactManagement { get; set; }
            public string StartCRMFeaturesToday { get; set; }


        }
        public class PageLinkButton
        {
            public string PageLinkButtonDiscoverDashBoard { get; set; }
            public string PageLinkButtonDiscoverTagManagement { get; set; }
            public string PageLinkButtonDiscoverCustomerRelationship { get; set; }
            public string PageLinkButtonDiscoverLeads { get; set; }


        }

        public class PageButton
        {
            public string ButtonJoinCRM { get; set; }

        }

    }
}