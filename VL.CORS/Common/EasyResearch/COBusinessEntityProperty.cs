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
        public COBusinessEntityProperty(COBusinessEntity cOBusinessEntity, XElement element)
        {
            From = cOBusinessEntity.SourceName;
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            ColumnType = element.Attribute(nameof(ColumnType))?.Value.ToEnum<ColumnType>() ?? ColumnType.None;
            EnumType = element.Attribute(nameof(EnumType))?.Value;
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public string From { set; get; }
        public string SourceName { set; get; }
        public ColumnType ColumnType { set; get; }
        public string EnumType { set; get; }
    }
}
