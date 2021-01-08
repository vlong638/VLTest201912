using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.PagerSolution;
using Autobots.Infrastracture.Common.ServiceSolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using ResearchAPI.CORS.Common;
using ResearchAPI.CORS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountService
    {
        APIContext APIContext { get; set; }
        DbContext ResearchDbContext { set; get; }

        UserDepartmentRepository UserDepartmentRepository { set; get; }
        UserRepository UserRepository { set; get; }
        UserRoleRepository UserRoleRepository { set; get; }
        RoleAuthorityRepository RoleAuthorityRepository { set; get; }
        RoleRepository RoleRepository { set; get; }

        /// <summary>
        /// 
        /// </summary>
        internal AccountService(DbContext DbContext)
        {
            ResearchDbContext = DbContext;
            Init(DbContext);
        }
        /// <summary>
        /// 
        /// </summary>
        public AccountService(APIContext apiContext)
        {
            APIContext = apiContext;
            ResearchDbContext = APIContext?.GetDBContext(APIContraints.ResearchDbContext);
            Init(ResearchDbContext);
        }

        private void Init(DbContext DbContext)
        {
            //repositories
            UserDepartmentRepository = new UserDepartmentRepository(DbContext);
            UserRepository = new UserRepository(DbContext);
            UserRoleRepository = new UserRoleRepository(DbContext);
            RoleAuthorityRepository = new RoleAuthorityRepository(DbContext);
            RoleRepository = new RoleRepository(DbContext);
        }

        internal ServiceResult<User> PasswordSignIn(string userName, string password)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var user = UserRepository.GetByUserNameAndPassword(userName, password.ToMD5());
                if (user == null)
                {
                    throw new NotImplementedException("用户不存在或密码错误");
                }
                return user;
            });
        }

        internal ServiceResult<Dictionary<long, string>> GetAllRolesIdAndName()
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var result = RoleRepository.GetAllSystemRoles();
                return result.ToDictionary(c => c.Id, c => c.Name);
            });
        }

        internal ServiceResult<VLPagerResult<List<GetUserModel>>> GetPagedUsers(int page, int limit, string username, string nickname)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var list = UserRepository.GetPagedUsers(page, limit, username, nickname).Select(c =>
                {
                    var m = new GetUserModel()
                    {
                        UserId = c.Id,
                        UserName = c.Name,
                    };
                    c.MapTo(m);
                    m.Sex_Text = m.Sex.GetDescription();
                    return m;
                }).ToList();
                var userRoles = RoleRepository.GetSystemRolesByUserIds(list.Select(c => c.UserId).ToList());
                list.ForEach(c =>
                {
                    c.Roles = userRoles.Where(ur => ur.UserId == c.UserId).Select(ur => new GetUserRoleModel() { RoleId = ur.RoleId, RoleName = ur.RoleName }).ToList();
                });
                var count = UserRepository.GetPagedUserCount(username, nickname);
                return new VLPagerResult<List<GetUserModel>>() { List = list, Count = count };
            });
        }

        internal ServiceResult<long> CreateUser(User user, List<long> roleIds, List<long> departmentIds)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                user.CreatedAt = DateTime.Now;
                user.Password = user.Password.ToMD5();
                var repeat = UserRepository.GetByName(user.Name);
                if (repeat != null)
                {
                    throw new NotImplementedException("用户名已存在");
                }
                var userId = UserRepository.InsertOne(user);
                UserRoleRepository.BatchInsert(userId, roleIds);
                UserDepartmentRepository.BatchInsert(userId, departmentIds);
                return userId;
            });
        }

        internal ServiceResult<VLPagerResult<List<GetRoleModel>>> GetPagedSystemRoles(int page, int limit, string roleName)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var list = RoleRepository.GetPagedSystemRoles(page, limit, roleName).Select(c => new GetRoleModel()
                {
                    RoleId = c.Id,
                    RoleName = c.Name,
                }).ToList();
                var count = RoleRepository.GetPagedRolesCount(roleName);
                return new VLPagerResult<List<GetRoleModel>>() { List = list, Count = count };
            });
        }

        internal ServiceResult<bool> UpdateUserStatus(long userId, bool currentStatus)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var result = UserRepository.UpdateUserStatus(userId, fromStatus: currentStatus, toStatus: !currentStatus);
                return result > 0;
            });
        }

        internal ServiceResult<bool> EditUser(User newUser, List<long> roleIds, List<long> departmentIds)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                newUser.CreatedAt = DateTime.Now;
                var user = UserRepository.GetById(newUser.Id);
                if (user == null)
                {
                    throw new Exception("用户不存在");
                }
                user.NickName = newUser.NickName;
                user.Sex = newUser.Sex;
                user.Phone = newUser.Phone;
                var result = UserRepository.UpdateUserInfo(user);
                if (result == 0)
                {
                    throw new Exception("用户信息更新失败");
                }
                UserRoleRepository.DeleteByUserId(newUser.Id);
                UserRoleRepository.BatchInsert(newUser.Id, roleIds);
                UserDepartmentRepository.DeleteByUserId(newUser.Id);
                UserDepartmentRepository.BatchInsert(newUser.Id, departmentIds);
                return true;
            });
        }

        internal ServiceResult<bool> EditRole(Role newRole)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var role = RoleRepository.GetByName(newRole.Name);
                if (role != null)
                {
                    throw new Exception("角色名已被使用");
                }
                var result = RoleRepository.Update(newRole);
                return result;
            });
        }

        internal ServiceResult<bool> DeleteRole(long roleId)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var result1 = UserRoleRepository.DeleteByRoleId(roleId);
                var result2 = RoleRepository.DeleteById(roleId);
                return result2;
            });
        }

        internal ServiceResult<long> CreateRole(Role role)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var repeat = RoleRepository.GetByName(role.Name);
                if (repeat != null)
                {
                    throw new NotImplementedException("角色名称已存在");
                }
                var id = RoleRepository.InsertOne(role);
                return id;
            });
        }

        internal ServiceResult<List<RoleAuthority>> GetRoleAuthorities(long roleId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                var roleAuthorities = RoleAuthorityRepository.GetByRoleIds(roleId);
                return roleAuthorities;
            });
        }

        internal ServiceResult<bool> EditRoleAuthorities(long roleId, List<long> authorityIds)
        {
            return ResearchDbContext.DelegateTransaction(c =>
            {
                var result = RoleAuthorityRepository.DeleteByRoleId(roleId);
                if (authorityIds.Count > 0)
                    result = RoleAuthorityRepository.BatchInsert(roleId, authorityIds);
                return true;
            });
        }

        internal ServiceResult<List<long>> GetSystemAuthorityIds(long userId)
        {
            return ResearchDbContext.DelegateNonTransaction(c =>
            {
                return UserRoleRepository.GetSystemAuthorityIds(userId);
            });
        }
    }
}
