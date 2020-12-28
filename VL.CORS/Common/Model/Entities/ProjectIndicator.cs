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
        public long BusinessEntityPropertyId { set; get; }

        public string EntitySourceName { set; get; }
        public string PropertySourceName { set; get; }
        public string PropertyDisplayName { set; get; }

        ///// <summary>
        ///// 字段别名名称
        ///// </summary>
        //public string ColumnNickName { set; get; }
    }
}