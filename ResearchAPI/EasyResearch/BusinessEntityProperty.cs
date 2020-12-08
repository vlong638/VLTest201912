using System.Xml.Linq;

namespace ResearchAPI.EasyResearch
{
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
