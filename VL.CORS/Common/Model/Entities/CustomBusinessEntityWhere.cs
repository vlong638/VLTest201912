using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class CustomBusinessEntityWhere
    {
        public const string TableName = "CustomBusinessEntityWhere";

        public long Id { set; get; }
        /// <summary>
        /// 业务对象Id
        /// </summary>
        public long BusinessEntityId { get; set; }
        /// <summary>
        /// 条件名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string Operator { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
    }
}