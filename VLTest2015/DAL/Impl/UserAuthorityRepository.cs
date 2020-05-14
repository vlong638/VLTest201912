using Dapper;
using System.Collections.Generic;
using System.Data;
using VLTest2015.Services;
using System;

namespace VLTest2015.DAL
{
    public class UserAuthorityRepository : Repository<UserAuthority>, IUserAuthorityRepository
    {
        public UserAuthorityRepository(DbContext context) : base(context)
        {
        }

        public int DeleteBy(long userId)
        {
            return _connection.Execute("delete from [A_UserAuthority] where UserId = @userId;"
                , new { userId }
                , _transaction);
        }

        public IEnumerable<UserAuthority> GetBy(long userId)
        {
            return _connection.Query<UserAuthority>("select * from [A_UserAuthority] where UserId = @userId;"
                , new { userId });
        }
    }
}