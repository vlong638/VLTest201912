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
            return _connection.Execute(@$"insert into UserDepartment (userId,departmentId) values 
{string.Join(",", departmentIds.Select(c => $"({userId},{c})"))}"
                , transaction: _transaction);
        }

        internal int DeleteByUserId(long userId)
        {
            return _connection.Execute("delete from [UserDepartment] where UserId = @UserId;"
                , new { userId }
                , transaction: _transaction);
        }

        /// <summary>
        /// 根据用户Id获取用户角色集合
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public List<UserDepartment> GetByUserIds(List<long> userIds)
        {
            return _connection.Query<UserDepartment>($@"select * from  UserDepartment where UserId in @userIds", new { userIds }, transaction: _transaction)
                .ToList();
        }
    }
}