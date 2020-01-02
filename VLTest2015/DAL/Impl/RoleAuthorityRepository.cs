using Dapper;
using System.Collections.Generic;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public class RoleAuthorityRepository : Repository<RoleAuthority>, IRoleAuthorityRepository
    {
        public RoleAuthorityRepository(DbContext context) : base(context)
        {
        }

        public int DeleteBy(long roleId)
        {
            return _connection.Execute("delete from [RoleAuthority] where RoleId = @roleId;"
                , new { roleId }, _transaction);
        }

        public IEnumerable<RoleAuthority> GetBy(long[] roleIds)
        {
            return _connection.Query<RoleAuthority>("select * from [RoleAuthority] where RoleId in @roleIds;"
                , new { roleIds });
        }
    }
}