using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;
using ErucaCRM.Domain;
namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Profile")]
    public class ProfileVM : BaseModel
    {
        public string ProfileId { get; set; }
        [Required(ErrorMessage = "Profile.ProfileNameRequired")]
        [Display(Name = "Profile Name")]
        public string ProfileName { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Nullable<bool> IsDefaultForRegisterdUser { get; set; }
        public Nullable<bool> IsDefaultForStaffUser { get; set; }


        private ICollection<ProfilePermissionModel> _ProfilePermissionModels;
        public virtual ICollection<ProfilePermissionModel> ProfilePermissionModels
        {
            get
            {
                if (this._ProfilePermissionModels == null)
                    this._ProfilePermissionModels = new List<ProfilePermissionModel>();
                return this._ProfilePermissionModels;
            }
            set { this._ProfilePermissionModels = value; }
        }

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
            }

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
        public class GridHeader
        {
            public string GridHeaderProfileName { get; set; }
            public string GridHeaderProfileDescription { get; set; }
            public string GridHeaderProfilePermissionRowText { get; set; }

            public string GridHeaderModuleName { get; set; }
            public string GridHeaderView { get; set; }
            public string GridHeaderCreate { get; set; }
            public string GridHeaderEdit { get; set; }
            public string GridHeaderDelete { get; set; }
        }

        public class PageSubHeader
        {
            public string PageSubHeaderProfileDetail { get; set; }
            public string PageSubHeaderDocumentsPermissions { get; set; }
            public string PageSubHeaderReAssignProfile { get; set; }
            public string PageSubHeaderAssignTo { get; set; }
            public string PageSubHeaderProfileCannotEdited { get; set; }
        }

        public class PageButton
        {
            public string ButtonReAssign { get; set; }
            public string EditButtonToolTip { get; set; }
            public string DeleteButtonToolTip { get; set; }


        }



    }
}