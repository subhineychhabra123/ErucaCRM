using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class CompanyPlanModel
    {

        public int CompanyPlanId { get; set; }
        public int CompanyId { get; set; }
        public int PlanId { get; set; }
        public System.DateTime PlanStartDate { get; set; }
        public System.DateTime PlanEndDate { get; set; }
        public bool IsTrialPlan { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        public virtual CompanyModel CompanyModel { get; set; }
        public virtual PlanModel PlanModel { get; set; }
    }
}
