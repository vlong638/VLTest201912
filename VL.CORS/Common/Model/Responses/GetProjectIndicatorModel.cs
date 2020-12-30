using System;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetProjectIndicatorModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { set; get; }
        ///// <summary>
        ///// 项目Id
        ///// </summary>
        //public long ProjectId { set; get; }
        ///// <summary>
        ///// 实体Id
        ///// </summary>
        //public long BusinessEntityId { set; get; }
        ///// <summary>
        ///// 字段Id
        ///// </summary>
        //public long BusinessEntityPropertyId { set; get; }
        /// <summary>
        /// 实体名称 如`基本档案`
        /// </summary>
        public string EntityName { set; get; }
        /// <summary>
        /// 显示名称 如`性别`
        /// </summary>
        public string PropertyName { set; get; }
        /// <summary>
        /// 用户设置的别名
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 字段数据类型,前端需要知道比较类型
        /// </summary>
        public ColumnType ColumnType { set; get; }
        /// <summary>
        /// 枚举类型
        /// </summary>
        public string EnumType { get; set; }
    }
    /// <summary>
    /// 字段数据类型
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 字符
        /// </summary>
        String = 1,
        /// <summary>
        /// 日期
        /// </summary>
        DateTime = 2,
        /// <summary>
        /// 数值
        /// </summary>
        Int = 3,
        /// <summary>
        /// 枚举
        /// </summary>
        Enum = 4,
    }
}
