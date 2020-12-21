using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectIndicator
    {
        public const string TableName = "ProjectIndicator";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long BusinessEntityId { set; get; }
        public string SourceName { set; get; }
        public string ColumnName { set; get; }
        public string DisplayName { set; get; }
        /// <summary>
        /// 字段别名名称
        /// </summary>
        public string ColumnNickName { set; get; }
    }
}