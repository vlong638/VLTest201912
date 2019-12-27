using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<UserRoleInfo> GetUserRoleInfosBy(long[] userIds)
        {
            return _connection.Query<UserRoleInfo>(@"select ur.UserId,r.Name as RoleName
from [Role] r
left join[UserRole] ur on ur.RoleId = r.Id
where ur.UserId in @userIds"
                , new { userIds });
        }

        public Role GetBy(string name)
        {
            return _connection.Query<Role>("select * from [Role] where Name = @name;"
                , new { name }).FirstOrDefault();
        }
    }
}