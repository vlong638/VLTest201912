using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceWhere
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Where";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceWhere(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Formatter = element.Attribute(nameof(Formatter))?.Value;
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? false;
            SQL = element.Value;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string SQL { set; get; }
        /// <summary>
        /// 格式化
        /// </summary>
        public string Formatter { get; set; }

        #region compute
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { set; get; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Formatter), Formatter);
            property.SetValue(SQL);
            return property;
        }
    }
}
