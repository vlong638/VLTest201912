using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    internal class UserDepartmentRepository : Repository<UserDepartment>
    {
        internal UserDepartmentRepository(DbContext context) : base(context)
        {
        }

        public int BatchInsert(long userId, List<long> departmentIds)
        {
            return _connection.Execute(@$"insert into UserDepartment (userId,roleId) values 
{string.Join(",", departmentIds.Select(c => $"({userId},{c})"))}"
                , transaction: _transaction);
        }

        internal int DeleteByUserId(long userId)
        {
            return _connection.Execute("delete from [UserDepartment] where UserId = @UserId;"
                , new { userId }
                , transaction: _transaction);
        }
    }
}