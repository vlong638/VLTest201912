using Dapper;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using VL.Research.Models;

namespace VL.Research.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<UserRoleInfoModel> GetUserRoleInfosBy(long[] userIds)
        {
            return _connection.Query<UserRoleInfoModel>(@"select ur.UserId,ur.RoleId,r.Name as RoleName
from [A_Role] r
left join [A_UserRole] ur on ur.RoleId = r.Id
where ur.UserId in @userIds;"
                , new { userIds });
        }

        public Role GetBy(string name)
        {
            return _connection.Query<Role>("select * from [A_Role] where Name = @name;"
                , new { name }).FirstOrDefault();
        }

        public IEnumerable<Role> GetAll()
        {
            return _connection.Query<Role>("select * from [A_Role] order by Id desc;");
        }
    }
}