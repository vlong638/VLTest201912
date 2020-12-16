using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class UserFavoriteProject
    {
        public const string TableName = "UserFavoriteProject";

        public long UserId { set; get; }
        public long ProjectId { set; get; }
    }
}
