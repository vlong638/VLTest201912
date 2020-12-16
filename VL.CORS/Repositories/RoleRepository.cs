using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{

    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public List<Role> GetAllRoles()
        {
            return _connection.Query<Role>("select * from [Role];", transaction: _transaction)
                .ToList();
        }
    }

    public class UserFavoriteRoleModel
    {
        public long RoleId { set; get; }
        public string RoleName { set; get; }
    }
}