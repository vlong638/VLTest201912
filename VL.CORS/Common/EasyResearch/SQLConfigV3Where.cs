using Autobots.Infrastracture.Common.ValuesSolution;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class SQLConfigV3Where
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Where";

        public SQLConfigV3Where()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigV3Where(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            Formatter = element.Attribute(nameof(Formatter))?.Value;
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            Required = element.Attribute(nameof(Required))?.Value.ToBool() ?? false;
            SQL = element.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否必须
        /// </summary>
        public bool Required { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string SQL { set; get; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string Formatter { get; set; }
    }
}
