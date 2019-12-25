using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using VLTest2015.Common;
using VLTest2015.DAL;
using VLTest2015.Utils;

namespace VLTest2015.Services
{
    public class UserService : IUserService
    {
        IUserRepository _userRepository;

        public UserService()
        {
            var connection = DBHelper.GetDbConnection();
            _userRepository = new UserRepository(connection);
        }

        public ResponseResult<long> CreateUser(string userName, string password)
        {
            var hashPassword = MD5Helper.GetHashValue(password);
            TUser user = new TUser()
            {
                Name = userName,
                Password = hashPassword,
            };
            var result = _userRepository.GetBy(userName);
            if (result != null)
            {
                return new ResponseResult<long>()
                {
                    ErrorCode = -1,
                    ErrorMessage = "用户名已存在",
                };
            }
            var id = _userRepository.Insert(user);
            return new ResponseResult<long>(id);
        }

        public ResponseResult<long> PasswordSignIn(string userName, string password, bool rememberMe, bool shouldLockout)
        {
            throw new NotImplementedException();
        }

        public ResponseResult<bool> EditUserAuthorities(long userId, IEnumerable<long> authorityIds)
        {
            throw new NotImplementedException();
        }

        public ResponseResult<bool> EditUserRoles(long userId, IEnumerable<long> roleIds)
        {
            throw new NotImplementedException();
        }

        public ResponseResult<IEnumerable<long>> GetUserAuthorities(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
