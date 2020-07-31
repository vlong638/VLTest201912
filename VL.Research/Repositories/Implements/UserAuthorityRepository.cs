using Dapper;
using System.Collections.Generic;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    public class UserAuthorityRepository : Repository<UserAuthority>
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