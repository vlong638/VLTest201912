using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using VLTest2015.Common;

namespace VLTest2015.Services
{
    public class UserService : IUserService
    {
        public ResponseResult<long> CreateUser(string userName, string password)
        {
            throw new NotImplementedException();
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
