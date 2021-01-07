using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class UserAuthority
    {
        public const string TableName = "UserAuthority";

        public long UserId { set; get; }
        public long AuthorityId { set; get; }
    }
}
