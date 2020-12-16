using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class User
    {
        public const string TableName = "User";

        public long UserId { set; get; }
        public string Name { set; get; }
    }
}
