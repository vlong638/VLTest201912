using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User GetBy(string userName)
        {
            return _connection.Query<User>("select * from [User] where Name = @userName;"
                , new { userName }, transaction: _transaction)
                .FirstOrDefault();
        }

        public List<User> GetAllUsers()
        {
            return _connection.Query<User>("select * from [User] ;"
                , transaction: _transaction)
                .ToList();
        }

        public List<User> GetAllUsersIdAndName()
        {
            return _connection.Query<User>("select Id,Name from [User] ;" 
                , transaction: _transaction)
                .ToList();
        }
    }
}