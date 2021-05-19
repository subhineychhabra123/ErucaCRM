using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Domain;
using System.ComponentModel.DataAnnotations;

namespace ErucaCRM.Web.ViewModels
{
    public class PlanVM
    {
        public string PlanId { get; set; }
        [Required(ErrorMessage = "Plan name is required")]
        [Display(Name = "Plan Name")]
        public string PlanName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Plan price is required")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "No. of user is required")]
        [Display(Name = "No of Users")]
        public int NoOfUsers { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }


        public virtual ICollection<CompanyPlanModel> CompanyPlans { get; set; }
        public virtual ICollection<PlanModuleModel> PlanModules { get; set; }
    }
}