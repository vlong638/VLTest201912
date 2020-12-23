using Autobots.Infrastracture.Common.ValuesSolution;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class COBusinessEntityProperty
    {
        public const string ElementName = "Property";

        public COBusinessEntityProperty()
        {
        }
        public COBusinessEntityProperty(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            From = element.Attribute(nameof(From))?.Value;
            ColumnName = element.Attribute(nameof(ColumnName))?.Value;
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public string From { set; get; }
        public string ColumnName { set; get; }
    }
}
