using Dapper;
using System.Collections.Generic;
using System.Data;
using VLTest2015.Services;
using System;

namespace VLTest2015.DAL
{
    public class RoleAuthorityRepository : Repository<RoleAuthority>, IRoleAuthorityRepository
    {
        public RoleAuthorityRepository(IDbConnection connection) : base(connection)
        {
        }

        public int DeleteBy(long roleId)
        {
            return _connection.Execute("delete from [User] where RoleId = @roleId;"
                , new { roleId });
        }

        public IEnumerable<RoleAuthority> GetBy(long[] roleIds)
        {
            return _connection.Query<RoleAuthority>("select * from [User] where RoleId in (@roleIds);"
                , new { roleIds });
        }
    }
}