using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class ModuleModel
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleCONSTANT { get; set; }
        public Nullable<int> ParentModuleId { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public string RenderType { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        public virtual ICollection<ModuleModel> ModuleModel1 { get; set; }
        public virtual ModuleModel ModuleModel2 { get; set; }

        private UserModel _UserModel;
        public virtual UserModel UserModel
        {
            get
            {
                if (this._UserModel == null)
                    this._UserModel = new UserModel();
                return this._UserModel;
            }
            set { this._UserModel = value; }
        }

        private UserModel _UserModel1;
        public virtual UserModel UserModel1
        {
            get
            {
                if (this._UserModel1 == null)
                    this._UserModel1 = new UserModel();
                return this._UserModel1;
            }
            set { this._UserModel1 = value; }
        }

        private ICollection<ModulePermissionModel> _ModulePermissionModels;
        public virtual ICollection<ModulePermissionModel> ModulePermissionModels
        {
            get
            {
                if (this._ModulePermissionModels == null)
                    this._ModulePermissionModels = new List<ModulePermissionModel>();
                return this._ModulePermissionModels;
            }
            set { this._ModulePermissionModels = value; }
        }

        private ICollection<TaskItemModel> _TaskModels;
        public virtual ICollection<TaskItemModel> TaskModels
        {
            get
            {
                if (this._TaskModels == null)
                    this._TaskModels = new List<TaskItemModel>();
                return this._TaskModels;
            }
            set { this._TaskModels = value; }
        }

        private ICollection<TaskItemModel> _TaskModels1;
        public virtual ICollection<TaskItemModel> TaskModels1
        {
            get
            {
                if (this._TaskModels1 == null)
                    this._TaskModels1 = new List<TaskItemModel>();
                return this._TaskModels1;
            }
            set { this._TaskModels1 = value; }
        }
    }
}
