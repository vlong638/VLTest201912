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
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 实体Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 实体名称 如`基本档案`
        /// </summary>
        public string BusinessEntityName { set; get; }
        /// <summary>
        /// 来源名称 如`PregnantInfo`
        /// </summary>
        public string SourceName { set; get; }
        /// <summary>
        /// 字段名称 如`Sex`
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 显示名称 如`性别`
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 用户设置的别名
        /// </summary>
        public string ColumnNickName { set; get; }
    }
}
