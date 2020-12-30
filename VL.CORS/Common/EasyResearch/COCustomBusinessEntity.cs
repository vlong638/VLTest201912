using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class COCustomBusinessEntity
    {
        public const string ElementName = "CustomBusinessEntity";

        public COCustomBusinessEntity()
        {
        }
        public COCustomBusinessEntity(XElement element)
        {
            ReportName = element.Attribute(nameof(ReportName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(COBusinessEntityProperty.ElementName).Select(c => new COBusinessEntityProperty(null, c)));
        }

        public string ReportName { set; get; }
        public string Template { set; get; }
        public List<COBusinessEntityProperty> Properties { set; get; } = new List<COBusinessEntityProperty>();
        public List<COBusinessEntityWhere> Wheres { set; get; } = new List<COBusinessEntityWhere>();
    }
}
