using System.Xml.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "Property";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public SQLConfigProperty(XElement element)
        {
            IsOn = element.Attribute(nameof(IsOn))?.Value.ToBool() ?? true;
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 查询用名称
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
}
