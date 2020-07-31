using Dapper;
using System.Collections.Generic;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    public class UserRoleRepository : Repository<UserRole>
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }

        public int DeleteBy(long userId)
        {
            return _connection.Execute($"Delete from [A_UserRole] where UserId = @userId;"
                , new { userId }, _transaction);
        }

        public IEnumerable<UserRole> GetBy(long userId)
        {
            return _connection.Query<UserRole>("select * from [A_UserRole] where UserId = @userId;"
                , new { userId });
        }
    }
}