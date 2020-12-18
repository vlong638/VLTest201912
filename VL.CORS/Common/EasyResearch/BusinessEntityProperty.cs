using Autobots.Infrastracture.Common.ValuesSolution;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class BusinessEntityProperty
    {
        public const string ElementName = "Property";

        public BusinessEntityProperty()
        {
        }
        public BusinessEntityProperty(XElement element)
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
