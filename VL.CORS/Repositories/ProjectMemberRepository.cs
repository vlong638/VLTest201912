using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{

    public class ProjectMemberRepository : Repository<ProjectMember>
    {
        public ProjectMemberRepository(DbContext context) : base(context)
        {
        }

        internal void AddProjectMembers(List<ProjectMember> members)
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

        internal List<User> GetUsersByProjectId(long projectId)
        {
            return _connection.Query<User>(@"
select u.Name,pm.UserId as Id from ProjectMember pm
left join [User] u on pm.userId = u.id
where pm.ProjectId = @ProjectId 
group by u.Name,pm.UserId;"
                , new { projectId }, transaction: _transaction).ToList();
        }
    }
}