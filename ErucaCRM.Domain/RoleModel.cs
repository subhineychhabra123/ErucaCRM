using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public Nullable<int> ParentRoleId { get; set; }
        public string RoleName { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public Nullable<bool> IsDefaultForRegisterdUser { get; set; }
        public Nullable<bool> IsDefaultForStaffUser { get; set; }

    }
}
