using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class PlanModuleModel
    {
        public int PlanModuleId { get; set; }
        public int PlanId { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public bool HasPermission { get; set; }
        public bool HasPermissionAfterTrail { get; set; }

        public virtual ModuleModel ModuleModel { get; set; }
        public virtual PlanModel PlanModel { get; set; }
    }
}
