using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;

namespace ErucaCRM.Business
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleRepository roleRepository;
        private readonly UserRepository userRepository;
        public RoleBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            roleRepository = new RoleRepository(unitOfWork);
            userRepository = new UserRepository(unitOfWork);
        }

        public bool AddRole(RoleModel roleModel)
        {
            Role role = new Role();

            bool isExists = roleRepository.Exists(r => r.RoleName.Trim() == roleModel.RoleName.Trim() && (r.CompanyId == roleModel.CompanyId || r.CompanyId == null)&&r.RecordDeleted==false);
            if (!isExists)
            {
                role = new Role();

                AutoMapper.Mapper.Map(roleModel, role);
                roleRepository.Insert(role);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateRole(RoleModel roleModel)
        {
            bool isExists = roleRepository.Exists(r => r.RoleName.Trim() == roleModel.RoleName.Trim() && (r.CompanyId == roleModel.CompanyId || r.CompanyId == null) && r.RecordDeleted == false&&r.RoleId!=roleModel.RoleId);
            if (isExists == false)
            {
                Role role = roleRepository.SingleOrDefault(r => r.RoleId == roleModel.RoleId && r.RecordDeleted == false);
                if (role != null)
                {
                    role.RoleName = roleModel.RoleName;
                    role.ParentRoleId = roleModel.ParentRoleId;
                    role.ModifiedDate = roleModel.ModifiedDate;
                    role.ModifiedBy = roleModel.ModifiedBy;
                    roleRepository.Update(role);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteRole(int roleId, int reassignroleId, int companyId)
        {
            try
            {
                List<Role> roleList = new List<Role>();
                Role roleObject = new Role();
                List<User> userList = new List<User>();
                if (reassignroleId > 0)
                {
                    roleList = roleRepository.GetAll(r => r.ParentRoleId == roleId && r.RecordDeleted == false).ToList();
                    if (roleList.Count > 0)
                    {
                        foreach (Role role in roleList)
                        {
                            role.ParentRoleId = reassignroleId;
                        }
                        roleRepository.UpdateAll(roleList);
                    }
                }
                roleObject = roleRepository.SingleOrDefault(r => r.RoleId == roleId && r.RecordDeleted == false);
                roleObject.RecordDeleted = true;
                roleRepository.Update(roleObject);
                //code for updating the roleId in user table
                userList = userRepository.GetAll(r => r.RoleId == roleId &&r.CompanyId==companyId&& r.Active == true).ToList();
                if (userList.Count > 0)
                {
                    foreach (User u in userList)
                    {
                        u.RoleId = reassignroleId;
                    }
                    userRepository.UpdateAll(userList);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<RoleModel> GetRoleByCompanyId(int companyId)
        {
            List<RoleModel> listRoleModel = new List<RoleModel>();
            List<Role> listRole = roleRepository.GetAll(p => p.CompanyId == companyId && p.RecordDeleted == false || p.CompanyId == null).ToList();
            AutoMapper.Mapper.Map(listRole, listRoleModel);
            return listRoleModel;
        }

        public List<RoleModel> GetAllRoleExceptDeleted(int companyId, int roleId)
        {
            throw new NotImplementedException();
        }
    }
}
