using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;

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
    }
}