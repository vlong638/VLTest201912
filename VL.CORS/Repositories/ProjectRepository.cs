using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System;
using System.Collections.Generic;
using System.Data;
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

        public override Project GetById(long id)
        {
            return _connection.Query<Project>("select * from [Project] where Id = @Id and IsDeleted = @IsDeleted;"
                , new { Id = id, IsDeleted = false })
                .FirstOrDefault();
        }

        public override bool DeleteById(long id)
        {
            return _connection.Execute("update [Project] set IsDeleted = @IsDeleted where Id = @Id ;"
                , new { Id = id, IsDeleted = true }) > 0;
        }

        public List<UserFavoriteProjectModel> GetUserFavoriteProjects(long userId)
        {
            return _connection.Query<UserFavoriteProjectModel>(@" select p.Id as ProjectId, p.Name as ProjectName
        from UserFavoriteProject ufp
        left join Project p on p.id = ufp.ProjectId
        where ufp.UserId = @userId and p.IsDeleted = @IsDeleted;"
                , new { userId, IsDeleted = false }).ToList();
        }

        internal List<long> GetUserIdsByProjectAndRoleName(int projectId, string roleName)
        {
            return _connection.Query<long>(@"select * from  ProjectMember where ProjectId = @ProjectId 
and RoleId = (select id from [role] where name = @RoleName)"
                , new { projectId, roleName }).ToList();
        }

        //internal List<GetPagedProjectsModel> GetPagedProjectList(SQLConfig sqlConfig)
        //{
        //    var sql = request.ToListSQL();
        //    var pars = request.GetParams();
        //    return context.DbGroup.Connection.Query<GetPagedProjectsModel>(sql, pars, transaction: _transaction).ToList();
        //}

        //internal int GetPagedProjectCount(GetPagedProjectsRequest request)
        //{
        //    var sql = request.ToCountSQL();
        //    var pars = request.GetParams();
        //    return context.DbGroup.Connection.Query<int>(sql, pars, transaction: _transaction).FirstOrDefault();
        //}

    }

    public class UserFavoriteProjectModel
    {
        public long ProjectId { set; get; }
        public string ProjectName { set; get; }
    }
}