using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class COBusinessEntity
    {
        public const string ElementName = "BusinessEntity";

        public COBusinessEntity()
        {
        }
        public COBusinessEntity(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(COBusinessEntityProperty.ElementName).Select(c => new COBusinessEntityProperty(c)));
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public List<COBusinessEntityProperty> Properties { set; get; } = new List<COBusinessEntityProperty>();
        public string Template { get; set; }
        public SQLConfigV3 SQLConfig { get; internal set; }
    }
}
