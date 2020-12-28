using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectDepartment
    {
        public const string TableName = "ProjectDepartment";

        public ProjectDepartment()
        {
        }

        public ProjectDepartment(long projectId, long departmentId)
        {
            ProjectId = projectId;
            DepartmentId = departmentId;
        }

        public long ProjectId { set; get; }
        public long DepartmentId { set; get; }
    }
}
