using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class FavoriteProjectRepository : Repository<FavoriteProject>
    {
        public FavoriteProjectRepository(DbContext context) : base(context)
        {
        }

        public int InsertOne(FavoriteProject favoriteProject)
        {
            return _connection.Execute("insert into [FavoriteProject] (ProjectId,UserId) values (@ProjectId,@UserId)"
                , favoriteProject, transaction: _transaction);
        }

        internal FavoriteProject GetOne(FavoriteProject favoriteProject)
        {
            return _connection.Query<FavoriteProject>("select * from [FavoriteProject] where ProjectId = @ProjectId and UserId = @UserId"
                , favoriteProject, transaction: _transaction).FirstOrDefault();
        }
    }
}