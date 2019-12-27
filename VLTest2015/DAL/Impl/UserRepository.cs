using Dapper;
using System;
using System.Data;
using System.Linq;
using VLTest2015.Services;
using System.Collections.Generic;

namespace VLTest2015.DAL
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        {
        }

        public User GetBy(string userName)
        {
            return _connection.Query<User>("select * from [User] where Name = @userName;"
                , new { userName })
                .FirstOrDefault();
        }

        public User GetBy(string userName, string password)
        {
            return _connection.Query<User>("select * from [User] where Name = @userName and Password = @password;"
                , new { userName, password })
                .FirstOrDefault();
        }

        public int GetUserPageListCount(GetUserPageListRequest paras)
        {
            var wheres = paras.GetWhereCondition();
            var sql = $@"select count(*) from [User]
{(string.IsNullOrEmpty(wheres) ? "" : "where " + wheres)}
";
            return _connection.ExecuteScalar<int>(sql, paras.GetParameters());
        }

        public List<User> GetUserPageListData(GetUserPageListRequest paras)
        {
            var wheres = paras.GetWhereCondition();
            var sql = $@"select Id,Name from [User]
{(string.IsNullOrEmpty(wheres) ? "" : "where " + wheres)}
{ paras.GetLimitCondition(nameof(User.Id))}
";
            return _connection.Query<User>(sql, paras.GetParameters()).ToList();
        }
    }
}