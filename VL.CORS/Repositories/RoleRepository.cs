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

        public List<Role> GetAllProjectRoles()
        {
            return _connection.Query<Role>("select * from [Role] where Category = @Category;"
                , new { Category = RoleCategory.ProjectRole }, transaction: _transaction)
                .ToList();
        }

        /// <summary>
        /// 根据用户Id获取用户角色集合
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<UserRoleModel> GetSystemRolesByUserIds(List<long> userIds)
        {
            return _connection.Query<UserRoleModel>($@"select ur.UserId,ur.RoleId,r.Name as RoleName
from UserRole ur
left join Role r on ur.roleId = r.id
where r.Category = @Category
and ur.userId in @userIds", new { Category = RoleCategory.SystemRole, userIds }, transaction: _transaction)
                .ToList();
        }
    }

    public class UserRoleModel
    {
        public long UserId { set; get; }
        public long RoleId { set; get; }
        public string RoleName { set; get; }
    }


}