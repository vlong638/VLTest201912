using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Research.Common
{
    public class BusinessEntities: List<IBusinessEntity>
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

        public IBusinessEntity GetByName(string name )
        {
            return this.FirstOrDefault(c => c.DisplayName == name);
        }
    }

    public interface IBusinessEntity
    {
        string DisplayName { get; set; }
        List<BusinessEntityProperty> Properties { get; set; }
    }

    public class CustomBusinessEntity : IBusinessEntity
    {
        public string DisplayName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<BusinessEntityProperty> Properties { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }

    public class BusinessEntity : IBusinessEntity
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

        public BusinessEntityProperty(string displayName, string from, string columnName)
        {
            DisplayName = displayName;
            From = from;
            ColumnName = columnName;
        }

        public string DisplayName { set; get; }
        public string From { set; get; }
        public string ColumnName { set; get; }
    }
}
