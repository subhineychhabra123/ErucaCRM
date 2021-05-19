using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "PublicPageAboutUs")]
    public class PublicAboutUsPageVM : CultureSpecificSiteContentVM
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


        public FrequentlyAskedQuestionAnswer FAQ
        {
            get
            {
                return new FrequentlyAskedQuestionAnswer();
            }

        }
        public class PageLabel
        {
            public string ChatLive { get; set; }
            public string Contact { get; set; }
            public string MoreAboutCRM { get; set; }
            public string WaterMarkSearchFAQ { get; set; }
        }

        public class PageSubHeader
        {
            public string PagSubHeaderAboutUs { get; set; }
            public string PageHeaderContactAndSupport { get; set; }

            public string PageHeaderQuestion { get; set; }
            public string PageHeaderFrequentlyAskedQuestions { get; set; }
            public string PageSubHeaderSale { get; set; }
            public string PageSubHeaderCustomerService { get; set; }
            public string PageSubHeaderReportBug { get; set; }
            public string PageSubHeaderBilling { get; set; }
            public string PageSubHeaderEnhancementRequest { get; set; }

        }

        public class PageContent
        {

            public string AboutUsContent { get; set; }

            public string ContactAndSupportLine1Content { get; set; }

            public string ContactAndSupportLine2Content { get; set; }

            public string PageHeaderQuestionContent { get; set; }

            public string ChatLiveContent { get; set; }
            public string SaleContent { get; set; }

            public string CustomerServiceContent { get; set; }
            public string ReportBugContent { get; set; }
            public string BillingContent { get; set; }
            public string EnhancementRequestContent { get; set; }
        }


        public class FrequentlyAskedQuestionAnswer
        {


            public string FrequentlyAskedQuestion1 { get; set; }
            public string FrequentlyAskedQuestion1Answer { get; set; }

            public string FrequentlyAskedQuestion2 { get; set; }
            public string FrequentlyAskedQuestion2AnswerHelpText { get; set; }
            public string FrequentlyAskedQuestion2Answer { get; set; }

            public string FrequentlyAskedQuestion3 { get; set; }
            public string FrequentlyAskedQuestion3Answer { get; set; }

            public string FrequentlyAskedQuestion4 { get; set; }
            public string FrequentlyAskedQuestion4Answer { get; set; }

            public string FrequentlyAskedQuestion5 { get; set; }
            public string FrequentlyAskedQuestion5Answer { get; set; }

            public string FrequentlyAskedQuestion6 { get; set; }
            public string FrequentlyAskedQuestion6Answer { get; set; }

            public string FrequentlyAskedQuestion7 { get; set; }
            public string FrequentlyAskedQuestion7Answer { get; set; }

            public string FrequentlyAskedQuestion8 { get; set; }
            public string FrequentlyAskedQuestion8Answer { get; set; }

            public string FrequentlyAskedQuestion9 { get; set; }
            public string FrequentlyAskedQuestion9Answer { get; set; }

            public string FrequentlyAskedQuestion10 { get; set; }
            public string FrequentlyAskedQuestion10Answer { get; set; }

            public string FrequentlyAskedQuestion11 { get; set; }
            public string FrequentlyAskedQuestion11Answer { get; set; }

            public string FrequentlyAskedQuestion12 { get; set; }
            public string FrequentlyAskedQuestion12Answer { get; set; }


        }

        public class PageButton
        {

            public string ButtonSubmit { get; set; }


        }

    }
}