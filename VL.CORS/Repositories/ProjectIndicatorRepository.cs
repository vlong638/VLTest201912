using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
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

        internal int DeleteByIds(IEnumerable<long> indicatorIds)
        {
            return _connection.Execute("delete from [ProjectIndicator] where id in @IndicatorIds"
                , new { indicatorIds }, transaction: _transaction);
        }

        internal int DeleteByEntityId(long projectId, long businessEntityId)
        {
            return _connection.Execute("delete from [ProjectIndicator] where ProjectId = @projectId and BusinessEntityId = @businessEntityId"
                , new { projectId, businessEntityId }, transaction: _transaction);
        }

        internal List<ProjectIndicator> GetByProjectId(long projectId)
        {
            return _connection.Query<ProjectIndicator>("select * from [ProjectIndicator] where ProjectId = @ProjectId"
                , new { projectId }, transaction: _transaction).ToList();
        }
    }
}