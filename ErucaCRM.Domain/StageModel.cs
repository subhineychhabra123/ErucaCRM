using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class StageModel
    {
        public int StageId { get; set; }
        public string StageName { get; set; }
        public int StageOrder { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> DefaultRatingId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public int StageLeadDuration { get; set; }
        public Nullable<bool> IsInitialStage { get; set; }
        public Nullable<bool> IsLastStage { get; set; }
       private RatingModel _RatingModel;
        public virtual RatingModel Rating
        {
            get
            {
                if (this._RatingModel == null)
                    this._RatingModel = new RatingModel();
                return this._RatingModel;
            }
            set { this._RatingModel = value; }
        }
    }
}
