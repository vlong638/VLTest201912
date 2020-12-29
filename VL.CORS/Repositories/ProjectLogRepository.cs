using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class ProjectLogRepository : Repository<ProjectLog>
    {
        public ProjectLogRepository(DbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProjectLog"></param>
        /// <returns></returns>
        public long InsertOne(ProjectLog ProjectLog)
        {
            return _connection.Execute("INSERT INTO [ProjectLog]([ProjectId], [OperatorId], [CreatedAt], [ActionType], [Text]) VALUES (@ProjectId, @OperatorId, GetDate(), @ActionType, @Text);"
                , ProjectLog, transaction: _transaction);
        }

        public int InsertBatch(IEnumerable<ProjectLog> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectLog in businessEntityProperties)
            {
                Insert(ProjectLog);
                i++;
            }
            return i;
        }

        internal ProjectLog GetLastestByTaskId(long taskId)
        {
            return _connection.Query<ProjectLog>("select top 1 * from [ProjectLog] where taskId = @taskId order by id desc"
                , new { taskId }, transaction: _transaction).FirstOrDefault();
        }
    }
}