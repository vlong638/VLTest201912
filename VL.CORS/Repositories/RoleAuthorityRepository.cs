using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class RoleAuthorityRepository : Repository<RoleAuthority>
    {
        public RoleAuthorityRepository(DbContext context) : base(context)
        {
        }

        public int DeleteByRoleId(long roleId)
        {
            return _connection.Execute("delete from [RoleAuthority] where RoleId = @roleId;"
                , new { roleId }, transaction: _transaction);
        }

        public List<RoleAuthority> GetByRoleIds(params long[] roleIds)
        {
            return _connection.Query<RoleAuthority>("select * from [RoleAuthority] where RoleId in @roleIds;"
                , new { roleIds }, transaction: _transaction).ToList();
        }

        public int BatchInsert(long roleId, List<long> authorityIds)
        {
            return _connection.Execute(@$"insert into RoleAuthority (RoleId,AuthorityId) values 
{string.Join(",", authorityIds.Select(c => $"({roleId},{c})"))}"
                , transaction: _transaction);
        }
    }
}