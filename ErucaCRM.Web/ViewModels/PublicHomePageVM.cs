using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "PublicPageHome")]
    public class PublicHomePageVM : CultureSpecificSiteContentVM
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

            public string PageSubHeaderIncreaseSalesWithCRMSalesApps { get; set; }
            public string PageSubHeaderHowToGrowYourSales { get; set; }
            public string PageSubHeaderEffectiveMarketing { get; set; }
            public string PageSubHeaderSalesTracking { get; set; }
            public string PageSubHeaderCustomerSupport { get; set; }
            public string PageSubHeaderMobileCRM { get; set; }
            public string PageSubHeaderWhySalesTeamsNeedCRM { get; set; }
            public string PageSubHeaderLeadStagesViewErucaCRMVisualization { get; set; }
            public string PageSubHeaderSliderSalesByStages { get; set; }
            public string PageSubHeaderSliderErucaTechMobileCRM { get; set; }

        }

        public class PageContent
        {

            public string EffectiveMarketingContent { get; set; }
            public string SalesTrackingContent { get; set; }
            public string CustomerSupportContent { get; set; }
            public string MobileCRMContent { get; set; }
            public string WhySalesTeamsNeedCRMContent { get; set; }
            public string LeadStagesViewErucaCRMVisualizationContent { get; set; }
            public string SliderStalesByStagesContent { get; set; }
            public string SliderErucaTechMobileCRMContent { get; set; }


        }

        public class PageLabel
        {
            public string PageLabelLeadsByStages { get; set; }
        }
        public class PageLinkButton
        {
            public string PageLinkButtonViewMore { get; set; }
        }

        public class PageButton
        {
            public string ButtonStartYourFreeTrail { get; set; }
            public string ButtonTakeTour { get; set; }

        }

    }
}