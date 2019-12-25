using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VLTest2015.Services
{
    public class UserService : IUserService
    {
        public long CreateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public User GetUserBy(string userName, string password)
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
