using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class CustomerBusinessEntity
    {
        public const string TableName = "CustomerBusinessEntity";

        public long Id { set; get; }
        public string Name { set; get; }
        public long TemplateId { set; get; }
    }
}