using System;
using System.Collections.Generic;
using System.Linq;
using VLTest2015.DAL;
using VLTest2015.Utils;

namespace VLTest2015.Services
{
    public class UserService : BaseService, IUserService
    {
        IUserRepository _userRepository;
        IUserAuthorityRepository _userAuthorityRepository;
        IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        IRoleAuthorityRepository _roleAuthorityRepository;
        UserMenuRepository _userMenuRepository;

        public UserService()
        {
            _userRepository = new UserRepository(_context);
            _userAuthorityRepository = new UserAuthorityRepository(_context);
            _userRoleRepository = new UserRoleRepository(_context);
            _roleRepository = new RoleRepository(_context);
            _roleAuthorityRepository = new RoleAuthorityRepository(_context);
            _userMenuRepository = new UserMenuRepository(_context);
        }

        public ServiceResult<User> Register(string userName, string password)
        {
            var hashPassword = MD5Helper.GetHashValue(password);
            User user = new User()
            {
                Name = userName,
                Password = hashPassword,
            };
            var result = _userRepository.GetBy(user.Name);
            if (result != null)
            {
                return Error<User>("用户名已存在");
            }
            user.Id = _userRepository.Insert(user);
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
            var result = _userRepository.GetBy(user.Name, user.Password);
            if (result == null)
            {
                return Error<User>("用户名不存在或与密码不匹配");
            }
            return Success(result);
        }

        public ServiceResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds)
        {
            return DelegateTransaction(() =>
            {
                _userAuthorityRepository.DeleteBy(userId);
                var userAuthorities = authorityIds.Select(c => new UserAuthority() { UserId = userId, AuthorityId = c }).ToArray();
                foreach (var userAuthority in userAuthorities)
                {
                    userAuthority.AuthorityId = _userAuthorityRepository.Insert(userAuthority);
                }
                return true;
            });
        }

        public ServiceResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds)
        {
            return DelegateTransaction(() =>
            {
                _userRoleRepository.DeleteBy(userId);
                var userRoles = roleIds.Select(c => new UserRole() { UserId = userId, RoleId = c }).ToArray();
                foreach (var userRole in userRoles)
                {
                    userRole.Id = _userRoleRepository.Insert(userRole);
                }
                return true;
            });
        }

        public ServiceResult<IEnumerable<long>> GetAllUserAuthorityIds(long userId)
        {
            var userAuthorities = _userAuthorityRepository.GetBy(userId);
            var userRoles = _userRoleRepository.GetBy(userId);
            var roleAuthorities = userRoles.Count() == 0 ? new List<RoleAuthority>() : _roleAuthorityRepository.GetBy(userRoles.Select(c => c.RoleId).ToArray());
            var allAuthorityIds = userAuthorities.Select(c => c.AuthorityId).Union(roleAuthorities.Select(c => c.AuthorityId)).Distinct();
            return Success(allAuthorityIds);
        }

        public ServiceResult<PagerResponse<User>> GetUserPageList(GetUserPageListRequest request)
        {
            PagerResponse<User> result = new PagerResponse<User>();
            result.TotalCount = _userRepository.GetUserPageListCount(request);
            result.Data = _userRepository.GetUserPageListData(request);
            return Success(result);
        }

        public ServiceResult<IEnumerable<UserRoleInfo>> GetRoleInfoByUserIds(params long[] userIds)
        {
            var result = _roleRepository.GetUserRoleInfosBy(userIds);
            return Success(result);
        }

        public ServiceResult<long> CreateRole(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName,
            };
            var result = _roleRepository.GetBy(role.Name);
            if (result != null)
            {
                return Error<long>("角色名称已存在", 501);
            }
            var id = _roleRepository.Insert(role);
            return Success(id);
        }

        public ServiceResult<bool> EditRoleAuthorities(long roleId, IEnumerable<long> authorityIds)
        {
            return DelegateTransaction(() =>
            {
                _roleAuthorityRepository.DeleteBy(roleId);
                var roleAuthorities = authorityIds.Select(c => new RoleAuthority() { RoleId = roleId, AuthorityId = c }).ToArray();
                foreach (var roleAuthority in roleAuthorities)
                {
                    roleAuthority.Id = _roleAuthorityRepository.Insert(roleAuthority);
                }
                return true;
            });
        }

        public ServiceResult<IEnumerable<long>> GetRoleAuthorityIds(long roleId)
        {
            var roleAuthorities = _roleAuthorityRepository.GetBy(roleId);
            return Success(roleAuthorities.Select(c => c.AuthorityId));
        }

        public ServiceResult<IEnumerable<Role>> GetAllRoles()
        {
            var roles = _roleRepository.GetAll();
            return Success(roles);
        }

        public ServiceResult<bool> UpdateUserMenu(UserMenu userMenu)
        {
            var isSuccess = _userMenuRepository.Update(userMenu);
            return Success(isSuccess);
        }

        public ServiceResult<long> CreateUserMenu(UserMenu userMenu)
        {
            var id = _userMenuRepository.Insert(userMenu);
            return Success(id);
        }

        public ServiceResult<List<UserMenu>> GetUserMenus(long userId)
        {
            var userMenus = _userMenuRepository.GetByUserId(userId);
            userMenus.ForEach(c => c.URL = string.Format(c.URL, c.Id));
            return Success(userMenus);
        }

        public ServiceResult<UserMenu> GetUserMenuById(long customConfigId)
        {
            var userMenu = _userMenuRepository.GetById(customConfigId);
            return Success(userMenu);
        }
    }
}
