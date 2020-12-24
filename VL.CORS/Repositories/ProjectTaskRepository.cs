using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
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
            return _connection.Query<ProjectTask>("select * from [ProjectTask] where id = @id"
                , new { id = taskId }, transaction: _transaction).FirstOrDefault();
        }
    }
}