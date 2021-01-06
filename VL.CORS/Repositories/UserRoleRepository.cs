using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    internal class UserRoleRepository : Repository<UserRole>
    {
        internal UserRoleRepository(DbContext context) : base(context)
        {
        }

        internal UserRole GetBy(string UserRoleName)
        {
            return _connection.Query<UserRole>("select * from [UserRole] where Name = @UserRoleName;"
                , new { UserRoleName }, transaction: _transaction)
                .FirstOrDefault();
        }

        internal List<UserRole> GetAllUserRoles()
        {
            return _connection.Query<UserRole>("select * from [UserRole] ;"
                , transaction: _transaction)
                .ToList();
        }

        internal List<UserRole> GetAllUserRolesIdAndName()
        {
            return _connection.Query<UserRole>("select Id,Name from [UserRole] ;" 
                , transaction: _transaction)
                .ToList();
        }

        internal UserRole GetByUserRoleNameAndPassword(string UserRoleName, string password)
        {
            return _connection.Query<UserRole>("select * from [UserRole] where Name = @UserRoleName and Password = @password and IsDeleted = 0;"
                , new { UserRoleName, password }
                , transaction: _transaction)
                .FirstOrDefault();
        }

        internal List<UserRole> GetPagedUserRoles(int page, int limit, string UserRolename, string nickname)
        {
            var sql = @$"
select *
from [UserRole]
where 1=1
{ (UserRolename.IsNullOrEmpty()?"":" and name like @name")}
{ (nickname.IsNullOrEmpty()?"": " and nickname like @nickname")}
order by id desc
offset {(page - 1) * limit} rows fetch next {limit} rows only
";
            return _connection.Query<UserRole>(sql
                , new { page, limit, name=$"%{UserRolename}%" }
                , transaction: _transaction)
                .ToList();
        }

        internal int GetPagedUserRoleCount(string UserRolename, string nickname)
        {
            var sql = @$"
select count(*) 
from [UserRole]
where 1=1
{ (UserRolename.IsNullOrEmpty() ? "" : " and name like @name")}
{ (nickname.IsNullOrEmpty() ? "" : " and nickname like @nickname")}
";
            return _connection.ExecuteScalar<int>(sql
                , new { name = $"%{UserRolename}%" }
                , transaction: _transaction);
        }

        internal int DeleteByUserId(long userId)
        {
            return _connection.Execute("delete from [UserRole] where UserId = @UserId;"
                , new { userId }
                , transaction: _transaction);
        }

        internal int DeleteByRoleId(long roleId)
        {
            return _connection.Execute("delete from [UserRole] where roleId = @roleId;"
                , new { roleId }
                , transaction: _transaction);
        }
    }
}