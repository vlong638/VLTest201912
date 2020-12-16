using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class FavoriteProject
    {
        public const string TableName = "FavoriteProject";

        public FavoriteProject()
        {
        }

        public FavoriteProject(long projectId, long userId)
        {
            ProjectId = projectId;
            UserId = userId;
        }

        public long UserId { set; get; }
        public long ProjectId { set; get; }
    }
}
