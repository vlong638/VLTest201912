using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class UserRole
    {
        public const string TableName = "UserRole";

        public long UserId { set; get; }
        public long RoleId { set; get; }
    }
}
