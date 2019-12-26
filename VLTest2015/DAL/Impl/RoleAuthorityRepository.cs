using Dapper;
using System.Collections.Generic;
using System.Data;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public class RoleAuthorityRepository : Repository<RoleAuthority>, IRoleAuthorityRepository
    {
        public RoleAuthorityRepository(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<RoleAuthority> GetBy(long[] roleIds)
        {
            return _connection.Query<RoleAuthority>("select * from [User] where RoleId in (@RoleIds);"
                , new { RoleIds = roleIds });
        }
    }
}