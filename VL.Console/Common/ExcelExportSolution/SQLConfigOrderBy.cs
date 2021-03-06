﻿using System.Xml.Linq;

namespace VL.Consolo_Core.Common.ExcelExportSolution
{
    /// <summary>
    /// 
    /// </summary>
    public class SQLConfigOrderBy
    {
        /// 
        /// </summary>
        public const string RootElementName = "OrderBys";
        /// <summary>
        /// 
        /// </summary>
        public const string ElementName = "OrderBy";

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
        public SQLConfigOrderBy(XElement element)
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
            var property = new XElement(ElementName);
            property.SetAttributeValue(nameof(ComponentName), ComponentName);
            property.SetAttributeValue(nameof(Alias), Alias);
            return property;
        }
    }
}
