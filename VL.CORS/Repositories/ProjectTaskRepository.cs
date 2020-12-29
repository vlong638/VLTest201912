using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Autobots.Infrastracture.Common.ValuesSolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class ProjectTaskRepository : Repository<ProjectTask>
    {
        public ProjectTaskRepository(DbContext context) : base(context)
        {
        }

        public long InsertOne(ProjectTask ProjectTask)
        {
            return Insert(ProjectTask);
        }

        public int InsertBatch(IEnumerable<ProjectTask> businessEntityProperties)
        {
            int i = 0;
            foreach (var ProjectTask in businessEntityProperties)
            {
                Insert(ProjectTask);
                i++;
            }
            return i;
        }

        public int UpdateName(long id, string projectName)
        {
            return _connection.Execute("update [ProjectTask] Name=@projectName where id = @id"
                , new { id, projectName }, transaction: _transaction);
        }

        internal ProjectTask GetById(long taskId)
        {
            return _connection.Query<ProjectTask>("select * from [ProjectTask] where id = @id and IsDeleted = 0"
                , new { id = taskId }, transaction: _transaction).FirstOrDefault();
        }

        public override bool DeleteById(long taskId)
        {
            return _connection.Execute("update [ProjectTask] set IsDeleted = 1 where Id = @Id ;"
                , new { id = taskId }, transaction: _transaction) > 0;
        }

        internal List<ProjectTask> GetByProjectId(long projectId)
        {
            return _connection.Query<ProjectTask>("select * from [ProjectTask] where projectId = @projectId and IsDeleted = 0"
                , new { projectId }, transaction: _transaction).ToList();
        }

        internal List<VLKeyValue<string, string>> GetTaskNameAndIds(long projectId)
        {
            return _connection.Query<VLKeyValue<string, string>>("select id as [value],name as [key] from [ProjectTask] where projectId = @projectId and IsDeleted = 0"
                , new { projectId }, transaction: _transaction).ToList();
        }
    }
}