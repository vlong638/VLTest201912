using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectMember
    {
        public const string TableName = "ProjectMember";

        public ProjectMember()
        {
        }

        public ProjectMember(long projectId, long userId, long roleId)
        {
            ProjectId = projectId;
            UserId = userId;
            RoleId = roleId;
        }

        public long ProjectId { set; get; }
        public long UserId { set; get; }
        public long RoleId { set; get; }
    }
}
