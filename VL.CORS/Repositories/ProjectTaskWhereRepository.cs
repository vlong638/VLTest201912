using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class ProjectTaskWhereRepository : Repository<ProjectTaskWhere>
    {
        public ProjectTaskWhereRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(ProjectTaskWhere ProjectTaskWhere)
        {
            ProjectTaskWhere.Id =  Insert(ProjectTaskWhere);
            return ProjectTaskWhere.Id;
        }

        public int InsertBatch(IEnumerable<ProjectTaskWhere> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectTaskWhere in businessEntityProperties)
            {
                ProjectTaskWhere.Id = Insert(ProjectTaskWhere);
                i++;
            }
            return i;
        }

        public int DeleteByTaskId(long taskId)
        {
            return _connection.Execute("delete from [ProjectTaskWhere] where taskId = @taskId"
                , new { taskId }, transaction: _transaction);
        }

        internal ProjectTaskWhere GetById(long taskId)
        {
            return _connection.Query<ProjectTaskWhere>("select * from [ProjectTaskWhere] where id = @id"
                , new { id = taskId }, transaction: _transaction).FirstOrDefault();
        }

        internal List<ProjectTaskWhere> GetByProjectId(long projectId)
        {
            return _connection.Query<ProjectTaskWhere>("select * from [ProjectTaskWhere] where ProjectId = @ProjectId"
                , new { projectId }, transaction: _transaction).ToList();
        }

        internal List<ProjectTaskWhere> GetByTaskId(long taskId)
        {
            return _connection.Query<ProjectTaskWhere>("select * from [ProjectTaskWhere] where TaskId = @TaskId"
                , new { taskId }, transaction: _transaction).ToList();
        }
    }
}