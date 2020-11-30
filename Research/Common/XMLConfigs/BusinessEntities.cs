using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Research.Common
{
    public class BusinessEntities: List<BusinessEntity>
    {
        #region 预设配置

        /// <summary>
        /// 
        /// </summary>
        public static string ElementName = "BusinessEntities";

        /// <summary>
        /// 
        /// </summary>
        public BusinessEntities()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public BusinessEntities(XElement element)
        {
            var businessEntities = element.Elements(BusinessEntity.ElementName);
            this.AddRange(businessEntities.Select(c => new BusinessEntity(c)));
        }

        #endregion
    }
    public class BusinessEntity
    {
        public const string ElementName = "BusinessEntity";

        public BusinessEntity()
        {
        }
        public BusinessEntity(XElement element)
        {
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            Properties.AddRange(element.Descendants(BusinessEntityProperty.ElementName).Select(c => new BusinessEntityProperty(c)));
        }

        public string DisplayName { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; } = new List<BusinessEntityProperty>();
    }
    public class BusinessEntityProperty
    {
        public const string ElementName = "Property";

        public BusinessEntityProperty()
        {
        }
        public BusinessEntityProperty(XElement element)
        {
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            From = element.Attribute(nameof(From))?.Value;
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
        }

        public string DisplayName { set; get; }
        public string From { set; get; }
        public string ColumnName { set; get; }
    }
}
