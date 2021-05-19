using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class PlanModel
    {
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int NoOfUsers { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        public virtual ICollection<CompanyPlanModel> CompanyPlansModel { get; set; }
        public virtual ICollection<PlanModuleModel> PlanModulesModel { get; set; }
    }
}
