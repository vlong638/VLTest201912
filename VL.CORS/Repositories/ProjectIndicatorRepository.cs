using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectIndicatorRepository : Repository<ProjectIndicator>
    {
        public ProjectIndicatorRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(ProjectIndicator ProjectIndicator)
        {
            return Insert(ProjectIndicator);
        }

        internal int InsertBatch(IEnumerable<ProjectIndicator> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectIndicator in businessEntityProperties)
            {
                Insert(ProjectIndicator);
                i++;
            }
            return i;
        }

        internal ProjectIndicator GetOne(ProjectIndicator ProjectIndicator)
        {
            return _connection.Query<ProjectIndicator>("select * from [ProjectIndicator] where ProjectId = @ProjectId and UserId = @UserId"
                , ProjectIndicator, transaction: _transaction).FirstOrDefault();
        }

        internal int DeleteOne(ProjectIndicator ProjectIndicator)
        {
            return _connection.Execute("delete from [ProjectIndicator] where ProjectId = @ProjectId and UserId = @UserId"
                , ProjectIndicator, transaction: _transaction);
        }

        internal int DeleteByEntityId(long projectId, long businessEntityId)
        {
            return _connection.Execute("delete from [ProjectIndicator] where ProjectId = @projectId and BusinessEntityId = @businessEntityId"
                , new { projectId, businessEntityId }, transaction: _transaction);
        }

        internal List<GetProjectIndicatorModel> GetByProjectId(long projectId)
        {
            return _connection.Query<GetProjectIndicatorModel>("select * from [ProjectIndicator] where ProjectId = @ProjectId"
                , new { projectId }, transaction: _transaction).ToList();
        }
    }
}