using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.ServiceSolution;
using VL.Research.Common.Configuration;
using VL.Research.Models;
using VL.Research.Repositories;
using VLTest2015.Common.MD5Solution;

namespace VL.Research.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        DbContext dbContext;
        UserRepository userRepository;
        UserAuthorityRepository userAuthorityRepository;
        UserRoleRepository userRoleRepository;
        RoleRepository roleRepository;
        RoleAuthorityRepository roleAuthorityRepository;
        UserMenuRepository userMenuRepository;

        /// <summary>
        /// 
        /// </summary>
        public UserService(DbContext dbContext)
        {
            this.dbContext = dbContext;
            userRepository = new UserRepository(dbContext);
            userAuthorityRepository = new UserAuthorityRepository(dbContext);
            userRoleRepository = new UserRoleRepository(dbContext);
            roleRepository = new RoleRepository(dbContext);
            roleAuthorityRepository = new RoleAuthorityRepository(dbContext);
            userMenuRepository = new UserMenuRepository(dbContext);
        }

        public ServiceResult<User> Register(string userName, string password)
        {
            var hashPassword = MD5Helper.GetHashValue(password);
            User user = new User()
            {
                Name = userName,
                Password = hashPassword,
            };
            var result = userRepository.GetBy(user.Name);
            if (result != null)
            {
                return Error<User>("用户名已存在");
            }
            user.Id = userRepository.Insert(user);
            return Success(user);
        }

        public ServiceResult<User> PasswordSignIn(string userName, string password, bool shouldLockout)
        {
            var hashPassword = MD5Helper.GetHashValue(password);
            User user = new User()
            {
                Name = userName,
                Password = hashPassword,
            };
            var result = userRepository.GetBy(user.Name, user.Password);
            if (result == null)
            {
                return Error<User>("用户名不存在或与密码不匹配");
            }
            return Success(result);
        }

        //public ServiceResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds)
        //{
        //    return DelegateTransaction(() =>
        //    {
        //        userAuthorityRepository.DeleteBy(userId);
        //        var userAuthorities = authorityIds.Select(c => new UserAuthority() { UserId = userId, AuthorityId = c }).ToArray();
        //        foreach (var userAuthority in userAuthorities)
        //        {
        //            userAuthority.AuthorityId = userAuthorityRepository.Insert(userAuthority);
        //        }
        //        return true;
        //    });
        //}

        //public ServiceResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds)
        //{
        //    return DelegateTransaction(() =>
        //    {
        //        userRoleRepository.DeleteBy(userId);
        //        var userRoles = roleIds.Select(c => new UserRole() { UserId = userId, RoleId = c }).ToArray();
        //        foreach (var userRole in userRoles)
        //        {
        //            userRole.Id = userRoleRepository.Insert(userRole);
        //        }
        //        return true;
        //    });
        //}

        public ServiceResult<IEnumerable<long>> GetAllUserAuthorityIds(long userId)
        {
            var userAuthorities = userAuthorityRepository.GetBy(userId);
            var userRoles = userRoleRepository.GetBy(userId);
            var roleAuthorities = userRoles.Count() == 0 ? new List<RoleAuthority>() : roleAuthorityRepository.GetBy(userRoles.Select(c => c.RoleId).ToArray());
            var allAuthorityIds = userAuthorities.Select(c => c.AuthorityId).Union(roleAuthorities.Select(c => c.AuthorityId)).Distinct();
            return Success(allAuthorityIds);
        }

        //public ServiceResult<PagerResponse<User>> GetUserPageList(GetUserPageListRequest request)
        //{
        //    PagerResponse<User> result = new PagerResponse<User>();
        //    result.TotalCount = userRepository.GetUserPageListCount(request);
        //    result.Data = userRepository.GetUserPageListData(request);
        //    return Success(result);
        //}

        public ServiceResult<IEnumerable<UserRoleInfoModel>> GetRoleInfoByUserIds(params long[] userIds)
        {
            var result = roleRepository.GetUserRoleInfosBy(userIds);
            return Success(result);
        }

        public ServiceResult<long> CreateRole(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName,
            };
            var result = roleRepository.GetBy(role.Name);
            if (result != null)
            {
                return Error<long>("角色名称已存在", 501);
            }
            var id = roleRepository.Insert(role);
            return Success(id);
        }

        //public ServiceResult<bool> EditRoleAuthorities(long roleId, IEnumerable<long> authorityIds)
        //{
        //    return DelegateTransaction(() =>
        //    {
        //        roleAuthorityRepository.DeleteBy(roleId);
        //        var roleAuthorities = authorityIds.Select(c => new RoleAuthority() { RoleId = roleId, AuthorityId = c }).ToArray();
        //        foreach (var roleAuthority in roleAuthorities)
        //        {
        //            roleAuthority.Id = roleAuthorityRepository.Insert(roleAuthority);
        //        }
        //        return true;
        //    });
        //}

        public ServiceResult<IEnumerable<long>> GetRoleAuthorityIds(long roleId)
        {
            var roleAuthorities = roleAuthorityRepository.GetBy(roleId);
            return Success(roleAuthorities.Select(c => c.AuthorityId));
        }

        public ServiceResult<IEnumerable<Role>> GetAllRoles()
        {
            var roles = roleRepository.GetAll();
            return Success(roles);
        }

        public ServiceResult<bool> UpdateUserMenu(UserMenu userMenu)
        {
            var isSuccess = userMenuRepository.Update(userMenu);
            return Success(isSuccess);
        }

        public ServiceResult<long> CreateUserMenu(UserMenu userMenu)
        {
            var id = userMenuRepository.Insert(userMenu);
            return Success(id);
        }

        public ServiceResult<List<UserMenu>> GetUserMenus(long userId)
        {
            var userMenus = userMenuRepository.GetByUserId(userId);
            userMenus.ForEach(c => c.URL = string.Format(c.URL, c.Id));
            return Success(userMenus);
        }

        public ServiceResult<UserMenu> GetUserMenuById(long customConfigId)
        {
            var userMenu = userMenuRepository.GetById(customConfigId);
            return Success(userMenu);
        }
    }
}
