using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Autobots.Infrastracture.Common.ValuesSolution;
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

        public List<Role> GetAllSystemRoles()
        {
            return _connection.Query<Role>("select * from [Role] where Category = @Category;"
                , new { Category = RoleCategory.SystemRole }, transaction: _transaction)
                .ToList();
        }

        internal Role GetByName(string name)
        {
            return _connection.Query<Role>("select * from [Role] where Name = @Name;"
                , new { name }
                , transaction: _transaction)
                .FirstOrDefault();
        }

        internal long InsertOne(Role role)
        {
            return _connection.ExecuteScalar<long>(@"INSERT INTO [Role]([Name],Category) VALUES (@name,@Category);SELECT @@IDENTITY;"
                , role, transaction: _transaction);
        }

        internal List<Role> GetPagedSystemRoles(int page, int limit, string name)
        {
            var sql = @$"
select *
from [Role]
where 1=1
and Category = @Category
{ (name.IsNullOrEmpty() ? "" : " and name like @name")}
order by id desc
offset {(page - 1) * limit} rows fetch next {limit} rows only
";
            return _connection.Query<Role>(sql
                , new { page, limit, name = $"%{name}%", Category= RoleCategory.SystemRole }
                , transaction: _transaction)
                .ToList();
        }
        internal int GetPagedRolesCount( string name)
        {
            var sql = @$"
select count(*)
from [Role]
where 1=1
{ (name.IsNullOrEmpty() ? "" : " and name like @name")}
";
            return _connection.ExecuteScalar<int>(sql
                , new { name = $"%{name}%" }
                , transaction: _transaction);
        }
    }

    public class UserRoleModel
    {
        public long UserId { set; get; }
        public long RoleId { set; get; }
        public string RoleName { set; get; }
    }


}