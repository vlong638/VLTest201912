using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{
    public class ProjectRepository : Repository<Project>
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }

        public Project GetBy(string ProjectName)
        {
            return _connection.Query<Project>("select * from [Project] where Name = @ProjectName;"
                , new { ProjectName })
                .FirstOrDefault();
        }

        public Project GetBy(string ProjectName, string password)
        {
            return _connection.Query<Project>("select * from [A_Project] where Name = @ProjectName and Password = @password;"
                , new { ProjectName, password })
                .FirstOrDefault();
        }

        public List<UserFavoriteProjectModel> GetUserFavoriteProjects(long userId)
        {
            return _connection.Query<UserFavoriteProjectModel>(@" select p.Id as ProjectId, p.Name as ProjectName
        from UserFavoriteProject ufp
        left join Project p on p.id = ufp.ProjectId
        where ufp.UserId = @userId"
                , new { userId }).ToList();
        }
    }

    public class UserFavoriteProjectModel
    {
        public long ProjectId { set; get; }
        public string ProjectName { set; get; }
    }
}