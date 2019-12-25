using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;

namespace VLTest2015.Services
{
    public class UserService : IUserService
    {
        public long CreateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public SignInStatus PasswordSignIn(string userName, string password, bool rememberMe, bool shouldLockout)
        {
            throw new NotImplementedException();
        }

        public bool EditUserAuthorities(long userId, IEnumerable<long> authorityIds)
        {
            throw new NotImplementedException();
        }

        public bool EditUserRoles(long userId, IEnumerable<long> roleIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<long> GetUserAuthorities(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
