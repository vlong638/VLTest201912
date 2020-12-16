using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Repositories
{

    public class ProjectMemberRepository : Repository<ProjectMember>
    {
        public ProjectMemberRepository(DbContext context) : base(context)
        {
        }

        internal void CreateProjectMembers(List<ProjectMember> members)
        {
            foreach (var member in members)
            {
                Insert(member);
            }
        }

        internal bool DeleteByProjectId(long projectId)
        {
            return _connection.Execute("Delete from ProjectMember where ProjectId = @ProjectId ;"
                , new { projectId }, transaction: _transaction) > 0;
        }
    }
}