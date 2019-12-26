using System.Data;
using VLTest2015.Services;

namespace VLTest2015.DAL
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnection connection) : base(connection)
        {
        }
    }
}