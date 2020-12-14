using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class CustomBusinessEntity
    {
        public const string ElementName = "CustomBusinessEntity";

        public CustomBusinessEntity()
        {
        }
        public CustomBusinessEntity(XElement element)
        {
            ReportName = element.Attribute(nameof(ReportName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(BusinessEntityProperty.ElementName).Select(c => new BusinessEntityProperty(c)));
        }

        public string ReportName { set; get; }
        public string Template { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; } = new List<BusinessEntityProperty>();
        public List<BusinessEntityWhere> Wheres { set; get; } = new List<BusinessEntityWhere>();
    }
}
