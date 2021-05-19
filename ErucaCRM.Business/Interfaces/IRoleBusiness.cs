using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IRoleBusiness
    {
        bool AddRole(RoleModel roleModel);
        bool UpdateRole(RoleModel roleModel);
        List<RoleModel> GetRoleByCompanyId(int companyId);
        bool DeleteRole(int roleId, int reassignroleId, int companyId);
    }
}
