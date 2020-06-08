using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VLTest2015.Utils
{
    class EntityAppConfig
    {
        public static string RootElementName = "Views";
        public static string NodeElementName = "View";

        public string ViewName { set; get; }
        public List<EntityAppConfigProperty> Properties { set; get; }

        public EntityAppConfig()
        {

        }
        public EntityAppConfig(XElement element)
        {
            ViewName = element.Attribute(nameof(ViewName)).Value;
            Properties = element.Descendants(EntityAppConfigProperty.ElementName).Select(c => new EntityAppConfigProperty(c)).ToList();
        }

        public XElement ToXElement()
        {
            var table = new XElement(NodeElementName);
            table.SetAttributeValue(nameof(ViewName), ViewName);
            var properties = Properties.Select(p => p.ToXElement());
            table.Add(properties);
            return table;
        }
    }
}
