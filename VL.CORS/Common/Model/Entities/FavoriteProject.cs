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

        public FavoriteProject(long userId, long projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }

        public long UserId { set; get; }
        public long ProjectId { set; get; }
    }
}
