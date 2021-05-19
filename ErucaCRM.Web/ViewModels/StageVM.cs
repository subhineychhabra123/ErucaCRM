using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Stage")]
    public class StageVM : BaseModel
    {

        public string StageId { get; set; }
        public string StageName { get; set; }
        public Nullable<bool> IsInitialStage { get; set; }
        public Nullable<bool> IsLastStage { get; set; }
        public int StageLeadDuration { get; set; }
        public int StageOrder { get; set; }
        public string DefaultRatingId { get; set; }
        private RatingVM _RatingVM;
        public virtual RatingVM Rating
        {
            get
            {
                if (this._RatingVM == null)
                    this._RatingVM = new RatingVM();
                return this._RatingVM;
            }
            set { this._RatingVM = value; }
        }
        public GridHeader GridHeaders
        {
            get
            {
                return new GridHeader();
            }

        }


        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

        }

        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }

        public class PageLabel
        {
            public string StageName { get; set; }
            public string StageLeadDuration { get; set; }
            public string StageOrderNumber { get; set; }
            public string StageRating { get; set; }
            public string Stages { get; set; }
        }

        public class PageSubHeader
        {
            public string PageSubHeaderMoveLeads { get; set; }
        }

        public class GridHeader
        {
            public string GridHeaderStageName { get; set; }
            public string GridHeaderStageOrderNumber { get; set; }
            public string GridHeaderStageRating { get; set; }
        }

        public class PageButton
        {
            public string ButtonSave { get; set; }
            public string ButtonCancel { get; set; }
            public string ButtonCreate { get; set; }
        }
    }
}