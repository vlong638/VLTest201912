using Dapper.Contrib.Extensions;
using System.ComponentModel;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectTaskWhere
    {
        public const string TableName = "ProjectTaskWhere";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long TaskId { set; get; }
        public long IndicatorId { set; get; }
        /// <summary>
        /// 实体Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 属性Id
        /// </summary>
        public long BusinessEntityPropertyId { set; get; }
        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName { set; get; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { set; get; }
        /// <summary>
        /// 操作符
        /// </summary>
        public ProjectTaskWhereOperator Operator { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
    }
    public enum ProjectTaskWhereOperator
    {
        /// <summary>
        /// 
        /// </summary>
        none= 0,
        /// <summary>
        /// 
        /// </summary>
        [Description("等于")]
        eq =1,
    }
}
