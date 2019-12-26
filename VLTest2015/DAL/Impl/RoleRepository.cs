using Dapper;
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

        public Role GetBy(string name)
        {
            return _connection.Query<Role>("select * from [Role] where Name = @name;"
                , new { name }).FirstOrDefault();
        }
    }
}