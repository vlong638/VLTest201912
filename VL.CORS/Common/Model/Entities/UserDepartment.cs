using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class UserDepartment
    {
        public const string TableName = "UserDepartment";

        public long UserId { set; get; }
        public long DepartmentId { set; get; }
    }
}
