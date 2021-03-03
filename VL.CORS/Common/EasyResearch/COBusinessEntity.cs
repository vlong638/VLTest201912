using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class COBusinessEntity
    {
        public const string ElementName = "BusinessEntity";

        public COBusinessEntity(COBusinessEntity businessEntity)
        {
            this.Id = businessEntity.Id;
            this.DisplayName = businessEntity.DisplayName;
            this.SourceName = businessEntity.SourceName;
            this.TargetName = businessEntity.TargetName;
            this.Template = businessEntity.Template;
            this.Properties = businessEntity.Properties.Select(c => new COBusinessEntityProperty(c)).ToList();
            this.SQLConfig = new SQLConfigV3(businessEntity.SQLConfig);

        }
        public COBusinessEntity(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            SourceName = element.Attribute(nameof(SourceName))?.Value;
            TargetName = element.Attribute(nameof(TargetName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(COBusinessEntityProperty.ElementName).Select(c => new COBusinessEntityProperty(this,c)));
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public string SourceName { set; get; }
        public string TargetName { set; get; }
        public List<COBusinessEntityProperty> Properties { set; get; } = new List<COBusinessEntityProperty>();
        public string Template { get; set; }
        public SQLConfigV3 SQLConfig { get; internal set; }
    }
}
