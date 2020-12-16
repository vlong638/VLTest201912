using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;

namespace ResearchAPI.CORS.Repositories
{
    public class FavoriteProjectRepository : Repository<FavoriteProject>
    {
        public FavoriteProjectRepository(DbContext context) : base(context)
        {
        }

        public int InsertOne(FavoriteProject favoriteProject)
        {
            return _connection.Execute("insert into [FavoriteProject] (UserId,ProjectId) values (@UserId,@ProjectId)"
                , favoriteProject, transaction: _transaction);
        }
    }
}