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
            return _connection.Query<User>("select * from [User] where Name = @userName;"
                , new { userName })
                .FirstOrDefault();
        }

        public User GetBy(string userName, string password)
        {
            return _connection.Query<User>("select * from [User] where Name = @userName and Password = @password;"
                , new { userName, password })
                .FirstOrDefault();
        }
    }
}