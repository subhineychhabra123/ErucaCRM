//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErucaCRM.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Plan
    {
        public Plan()
        {
            this.CompanyPlans = new HashSet<CompanyPlan>();
            this.PlanModules = new HashSet<PlanModule>();
        }
    
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
    
        public virtual ICollection<CompanyPlan> CompanyPlans { get; set; }
        public virtual ICollection<PlanModule> PlanModules { get; set; }
    }
}
