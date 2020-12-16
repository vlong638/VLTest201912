
using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class Role
    {
        public const string TableName = "Role";

        public long Id { set; get; }
        public string Name { set; get; }
    }
}
