using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
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
                , new { userName })
                .FirstOrDefault();
        }

        public User GetBy(string userName, string password)
        {
            return _connection.Query<User>("select * from [A_User] where Name = @userName and Password = @password;"
                , new { userName, password })
                .FirstOrDefault();
        }
    }
}