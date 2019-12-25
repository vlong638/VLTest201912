using Dapper;
using System;
using System.Data;
using System.Linq;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public class UserRepository : Repository<TUser>, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        {
        }

        public TUser GetBy(string userName)
        {
            return _connection.Query<TUser>("select * from [User] where Name = @UserName;", new { UserName = userName }).FirstOrDefault();
        }
    }
}