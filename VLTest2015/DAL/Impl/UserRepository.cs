using Dapper;
using System;
using System.Data;
using System.Linq;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        {
        }

        public User GetBy(string userName)
        {
            return _connection.Query<User>("select * from [User] where Name = @UserName;"
                , new { UserName = userName })
                .FirstOrDefault();
        }

        public User GetBy(object userName, string password)
        {
            return _connection.Query<User>("select * from [User] where Name = @UserName and Password = @Password;"
                , new { UserName = userName, Password = password })
                .FirstOrDefault();
        }
    }
}