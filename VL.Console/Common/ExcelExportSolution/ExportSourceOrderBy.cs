using System.Xml.Linq;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportSourceOrderBy
    {
        /// <summary>
        /// 
        /// </summary>
        public const string NodeElementName = "OrderBy";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }
        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsAsc { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string ComponentName { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Alias { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public ExportSourceOrderBy(XElement element)
        {
            ComponentName = element.Attribute(nameof(ComponentName))?.Value;
            Alias = element.Attribute(nameof(Alias))?.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XElement ToXElement()
        {
            var property = new XElement(NodeElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
}
