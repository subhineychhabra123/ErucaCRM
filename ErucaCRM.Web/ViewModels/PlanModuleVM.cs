using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Domain;

namespace ErucaCRM.Web.ViewModels
{
    public class PlanModuleVM
    {
        public string PlanModuleId { get; set; }
        public string PlanId { get; set; }
        public int ModuleId { get; set; }
        public bool HasPermission { get; set; }
        public bool HasPermissionAfterTrail { get; set; }

        public virtual ICollection<ModuleListModel> ModuleList { get; set; }

        public virtual ModuleModel Module { get; set; }
        public virtual PlanModel Plan { get; set; }
    }
}