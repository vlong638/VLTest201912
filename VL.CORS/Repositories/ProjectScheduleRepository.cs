using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class ProjectScheduleRepository : Repository<ProjectSchedule>
    {
        public ProjectScheduleRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(ProjectSchedule ProjectSchedule)
        {
            return Insert(ProjectSchedule);
        }

        public int InsertBatch(IEnumerable<ProjectSchedule> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectSchedule in businessEntityProperties)
            {
                Insert(ProjectSchedule);
                i++;
            }
            return i;
        }

        internal ProjectSchedule GetLastestByTaskId(long taskId)
        {
            return _connection.Query<ProjectSchedule>("select top 1 * from [ProjectSchedule] where taskId = @taskId order by id desc"
                , new { taskId }, transaction: _transaction).FirstOrDefault();
        }
    }
}