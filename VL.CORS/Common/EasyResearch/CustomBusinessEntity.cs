using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
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
            Properties.AddRange(element.Descendants(COBusinessEntityProperty.ElementName).Select(c => new COBusinessEntityProperty(c)));
        }

        public string ReportName { set; get; }
        public string Template { set; get; }
        public List<COBusinessEntityProperty> Properties { set; get; } = new List<COBusinessEntityProperty>();
        public List<COBusinessEntityWhere> Wheres { set; get; } = new List<COBusinessEntityWhere>();
    }
}
