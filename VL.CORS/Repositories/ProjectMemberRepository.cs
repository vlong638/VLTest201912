using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using ResearchAPI.CORS.Common;

namespace ResearchAPI.CORS.Repositories
{

    public class ProjectMemberRepository : Repository<ProjectMember>
    {
        public ProjectMemberRepository(DbContext context) : base(context)
        {
        }

    }
}