using Dapper;
using System.Collections.Generic;
using VL.Consolo_Core.Common.DBSolution;
using VL.Consolo_Core.Common.RepositorySolution;
using BBee.Models;

namespace BBee.Repositories
{
    public class RoleAuthorityRepository : Repository<RoleAuthority>
    {
        public RoleAuthorityRepository(DbContext context) : base(context)
        {
        }

        public int DeleteBy(long roleId)
        {
            return _connection.Execute("delete from [A_RoleAuthority] where RoleId = @roleId;"
                , new { roleId }, _transaction);
        }

        public IEnumerable<RoleAuthority> GetBy(params long[] roleIds)
        {
            return _connection.Query<RoleAuthority>("select * from [A_RoleAuthority] where RoleId in @roleIds;"
                , new { roleIds });
        }
    }
}