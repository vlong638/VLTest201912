using System.Xml.Linq;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceProperty
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "Property";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceProperty(XElement element)
        {
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

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
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ColumnName), ColumnName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
}
