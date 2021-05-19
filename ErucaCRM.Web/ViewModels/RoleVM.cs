using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Role")]
    public class RoleVM:BaseModel
    {
        [Required(ErrorMessage = "Role.RoleRequired")]
        public string RoleId { get; set; }
        public string ParentRoleId { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public int CompanyId { get; set; }
        public Nullable<bool> IsDefaultForRegisterdUser { get; set; }
        public Nullable<bool> IsDefaultForStaffUser { get; set; }


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

        public class PageSubHeader
        {
            public string RoleName { get; set; }
            public string EditRole { get; set; }
            public string ReAssignRole { get; set; }
            public string ReplyTo { get; set; }
        }

        public class PageButton
        {
            public string ButtonReAssign { get; set; }

        }
    }
}