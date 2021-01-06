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
    internal class UserRepository : Repository<User>
    {
        internal UserRepository(DbContext context) : base(context)
        {
        }

        internal long InsertOne(User user)
        {
            return _connection.ExecuteScalar<long>(@"INSERT INTO [User]([Name], [Password], [NickName], [Phone], [Sex], [IsDeleted]) 
VALUES (@name, @password, @nickname, @phone, @sex, @isdeleted);SELECT @@IDENTITY;"
                , user, transaction: _transaction);
        }

        internal User GetBy(string userName)
        {
            return _connection.Query<User>("select * from [User] where Name = @userName;"
                , new { userName }, transaction: _transaction)
                .FirstOrDefault();
        }

        internal List<User> GetAllUsers()
        {
            return _connection.Query<User>("select * from [User] ;"
                , transaction: _transaction)
                .ToList();
        }

        internal List<User> GetAllUsersIdAndName()
        {
            return _connection.Query<User>("select Id,Name from [User] ;" 
                , transaction: _transaction)
                .ToList();
        }

        internal User GetByUserName(string userName)
        {
            return _connection.Query<User>("select * from [User] where Name = @UserName;"
                , new { userName}
                , transaction: _transaction)
                .FirstOrDefault();
        }

        internal User GetByUserNameAndPassword(string userName, string password)
        {
            return _connection.Query<User>("select * from [User] where Name = @UserName and Password = @Password and IsDeleted = 0;"
                , new { userName, Password = password.ToMD5() }
                , transaction: _transaction)
                .FirstOrDefault();
        }

        internal List<User> GetPagedUsers(int page, int limit, string username, string nickname)
        {
            var sql = @$"
select *
from [User]
where 1=1
{ (username.IsNullOrEmpty()?"":" and name like @name")}
{ (nickname.IsNullOrEmpty()?"": " and nickname like @nickname")}
order by id desc
offset {(page - 1) * limit} rows fetch next {limit} rows only
";
            return _connection.Query<User>(sql
                , new { page, limit, name=$"%{username}%" }
                , transaction: _transaction)
                .ToList();
        }

        internal int GetPagedUserCount(string username, string nickname)
        {
            var sql = @$"
select count(*) 
from [User]
where 1=1
{ (username.IsNullOrEmpty() ? "" : " and name like @name")}
{ (nickname.IsNullOrEmpty() ? "" : " and nickname like @nickname")}
";
            return _connection.ExecuteScalar<int>(sql
                , new { name = $"%{username}%" }
                , transaction: _transaction);
        }
    }
}