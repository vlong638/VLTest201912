using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class CustomBusinessEntity
    {
        public const string TableName = "CustomBusinessEntity";

        public long Id { set; get; }
        public string Name { set; get; }
        public long TemplateId { set; get; }
    }
}