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

        public UserService()
        {
            _userRepository = new UserRepository(_connection);
            _userAuthorityRepository = new UserAuthorityRepository(_connection);
            _userRoleRepository = new UserRoleRepository(_connection);
            _roleRepository = new RoleRepository(_connection);
            _roleAuthorityRepository = new RoleAuthorityRepository(_connection);
        }

        public ResponseResult<User> Register(string userName, string password)
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
                return new ResponseResult<User>()
                {
                    ErrorCode = -1,
                    ErrorMessage = "用户名已存在",
                };
            }
            user.Id = _userRepository.Insert(user);     
            return new ResponseResult<User>(user);
        }

        public ResponseResult<User> PasswordSignIn(string userName, string password, bool shouldLockout)
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
                return new ResponseResult<User>()
                {
                    Status= false,
                    ErrorCode = -1,
                    ErrorMessage = "用户名不存在或与密码不匹配",
                };
            }
            return new ResponseResult<User>(result);
        }

        public ResponseResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds)
        {
            return _connection.DelegateTransaction(() =>
            {
                _userAuthorityRepository.DeleteBy(userId);
                var userAuthorities = authorityIds.Select(c => new UserAuthority() { UserId = userId, AuthorityId = c }).ToArray();
                _userAuthorityRepository.Insert(userAuthorities);
                return true;
            });
        }

        public ResponseResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds)
        {
            return _connection.DelegateTransaction(() =>
            {
                _userRoleRepository.DeleteBy(userId);
                var userRoles = roleIds.Select(c => new UserRole() { UserId = userId, RoleId = c }).ToArray();
                _userRoleRepository.Insert(userRoles);
                return true;
            });
        }

        public ResponseResult<IEnumerable<long>> GetAllUserAuthorityIds(long userId)
        {
            var userAuthorities = _userAuthorityRepository.GetBy(userId);
            var userRoles = _userRoleRepository.GetBy(userId);
            var roleAuthorities = userRoles.Count() == 0 ? new List<RoleAuthority>() : _roleAuthorityRepository.GetBy(userRoles.Select(c => c.RoleId).ToArray());
            var allAuthorityIds = userAuthorities.Select(c => c.AuthorityId).Union(roleAuthorities.Select(c => c.AuthorityId)).Distinct();
            return new ResponseResult<IEnumerable<long>>(allAuthorityIds);
        }
    }
}
