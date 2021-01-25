using System;

namespace ResearchAPI.CORS.Common
{

    /// <summary>
    /// 
    /// </summary>
    public class ProjectIndicatorDisplayModel : ProjectIndicator
    {
        /// <summary>
        /// 模板的业务对象显示名称
        /// </summary>
        public string TemplateDisplayName { set; get; }
        /// <summary>
        /// 模板的指标显示名称
        /// </summary>
        public string TemplatePropertyDisplayName { set; get; }
        /// <summary>
        /// 模板的指标字段类型
        /// </summary>
        public ColumnType TemplatePropertColumnType { set; get; }
        /// <summary>
        /// 模板的指标枚举类型
        /// </summary>
        public string TemplatePropertEnumType { set; get; }
    }
}
