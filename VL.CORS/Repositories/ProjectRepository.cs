using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
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
                , new { ProjectName }, transaction: _transaction)
                .FirstOrDefault();
        }

        public override Project GetById(long id)
        {
            return _connection.Query<Project>("select * from [Project] where Id = @Id and IsDeleted = 0;"
                , new { Id = id }, transaction: _transaction)
                .FirstOrDefault();
        }

        public bool DeleteById(long id)
        {
            return _connection.Execute("update [Project] set IsDeleted = 1 where Id = @Id ;"
                , new { Id = id }, transaction: _transaction) > 0;
        }

        public List<GetFavoriteProjectModel> GetFavoriteProjects(long userId)
        {
            return _connection.Query<GetFavoriteProjectModel>(@" select p.Id as ProjectId, p.Name as ProjectName
        from Project p
        left join FavoriteProject fp on p.id = fp.ProjectId
        where fp.UserId = @userId and p.IsDeleted = 0;"
                , new { userId }, transaction: _transaction).ToList();
        }

        internal List<long> GetUserIdsByProjectIdAndRoleId(long projectId, long roleId)
        {
            return _connection.Query<long>(@"
select pm.UserId from ProjectMember pm
where pm.ProjectId = @ProjectId 
and pm.RoleId = @RoleId
"
                , new { projectId, roleId }, transaction: _transaction).ToList();
        }

        internal long GetRoleIdByName(string roleName)
        {
            return _connection.Query<long>(@"select id from Role where name = @RoleName)"
                , new { roleName }, transaction: _transaction).FirstOrDefault();
        }

        internal List<GetLatestProjectModel> GetLatestProjects(int limit)
        {
            return _connection.Query<GetLatestProjectModel>($@" select top {limit} p.Id as ProjectId, p.Name as ProjectName,p.LastModifiedAt from Project p where IsDeleted = 0 order by p.LastModifiedAt desc;"
                , transaction: _transaction).ToList();
        }
    }
}