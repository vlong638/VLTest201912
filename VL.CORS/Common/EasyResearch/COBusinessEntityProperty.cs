using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class COBusinessEntityProperty
    {
        public const string ElementName = "Property";

        public COBusinessEntityProperty()
        {
        }

        public COBusinessEntityProperty(COBusinessEntityProperty c)
        {
            From = c.SourceName;
            Id = c.Id;
            DisplayName = c.DisplayName;
            SourceName = c.SourceName;
            ColumnType = c.ColumnType;
            EnumType = c.EnumType;
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

        internal string GetFieldAlias()
        {
            return $"[{ From}_{ SourceName}]";
        }
    }
}
