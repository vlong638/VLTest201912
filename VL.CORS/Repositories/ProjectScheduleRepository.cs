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

        internal GetTaskStatusModel GetTaskStatus(long taskId)
        {
            return _connection.Query<GetTaskStatusModel>("select top 1 Status as ScheduleStatus,ResultFile,LastCompletedAt from [ProjectSchedule] where taskId = @taskId order by id desc"
                , new { taskId }, transaction: _transaction).FirstOrDefault();
        }

        internal bool UpdateResultFile(long scheduleId,string resultFile)
        {
            return _connection.Execute("update [ProjectSchedule] set ResultFile = @ResultFile , Status = @Status, LastCompletedAt = GetDate() where Id = @ScheduleId"
                , new { scheduleId, resultFile, Status = ScheduleStatus.Completed }, transaction: _transaction) > 0;
        }

        internal List<ProjectSchedule> GetByProjectId(long projectId)
        {
            return _connection.Query<ProjectSchedule>(@"select ps.* 
from(select ProjectId, TaskId, max(id) maxId from ProjectSchedule where ProjectId = @ProjectId group by ProjectId, TaskId) as t
left join ProjectSchedule ps on t.maxId = ps.id"
                , new { projectId }, transaction: _transaction).ToList();
        }
    }
}