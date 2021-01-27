using Dapper.Contrib.Extensions;
using ResearchAPI.CORS.Repositories;
using System;
using System.ComponentModel;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectTaskWhere
    {
        public const string TableName = "ProjectTaskWhere";

        public ProjectTaskWhere()
        {
        }
        public ProjectTaskWhere(EditTaskV2GroupedCondition c)
        {
            //this.c = c;
        }
        public ProjectTaskWhere(EditTaskWhereCondition where)
        {
            //this.where = where;
        }

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
        /// <summary>
        /// 节点类型
        /// </summary>
        public ProjectTaskWhereCategory WhereCategory { set; get; }
        /// <summary>
        /// 父节点
        /// </summary>
        public long? ParentId { set; get; }
    }

    public enum ProjectTaskWhereCategory
    {
        None = 0,
        Indicator = 1,
        GroupAnd = 2,
        GroupOr = 3,
    }

    /// <summary>
    /// 
    /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        like =2,
        /// <summary>
        /// 
        /// </summary>
        isNotNull=3,
        /// <summary>
        /// 
        /// </summary>
        isNull=4,
        /// <summary>
        /// GreatThan
        /// </summary>
        gt=5,
        /// <summary>
        /// LessThan
        /// </summary>
        lt=6,
        /// <summary>
        /// GreatOrEqualThan
        /// </summary>
        get=7,
        /// <summary>
        /// LessOrEqualThan
        /// </summary>
        let=8,
    }
}
