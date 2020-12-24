using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectTaskWhere
    {
        public const string TableName = "ProjectTaskWhere";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long TaskId { set; get; }
        public long BusinessEntityId { set; get; }
        public long BusinessEntityPropertyId { set; get; }

        public string EntityName { set; get; }
        public string PropertyName { set; get; }
        public string Operator { set; get; }
        public string Value { set; get; }
    }
}
