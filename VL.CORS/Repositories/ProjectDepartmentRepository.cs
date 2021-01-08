using Autobots.Infrastracture.Common.DBSolution;
using Autobots.Infrastracture.Common.RepositorySolution;
using Dapper;
using ResearchAPI.CORS.Common;
using System.Collections.Generic;
using System.Linq;

namespace ResearchAPI.CORS.Repositories
{

    public class ProjectDepartmentRepository : Repository<ProjectDepartment>
    {
        public ProjectDepartmentRepository(DbContext context) : base(context)
        {
        }

        internal int BatchInsert(List<ProjectDepartment> items)
        {
            return _connection.Execute(@$"insert into ProjectDepartment (ProjectId,DepartmentId) values 
{string.Join(",", items.Select(c => $"({c.ProjectId},{ c.DepartmentId})"))}"
                , transaction: _transaction);
        }

        internal bool DeleteByProjectId(long projectId)
        {
            return _connection.Execute("Delete from ProjectDepartment where ProjectId = @ProjectId ;"
                , new { projectId }, transaction: _transaction) > 0;
        }

        internal List<long> GetDepartmentIdsByProjectId(long projectId)
        {
            return _connection.Query<long>(@"
select DepartmentId from ProjectDepartment
where ProjectId = @ProjectId 
"
                , new { projectId }, transaction: _transaction).ToList();
        }

    }
}