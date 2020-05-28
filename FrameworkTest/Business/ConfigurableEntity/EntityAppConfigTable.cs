using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FrameworkTest.ConfigurableEntity
{
    class EntityAppConfigTable
    {
        public static string ElementName = "Table";

        public string TableName { set; get; }
        public List<EntityAppConfigProperty> Properties { set; get; }

        public EntityAppConfigTable()
        {

        }
        public EntityAppConfigTable(XElement element)
        {
            TableName = element.Attribute(nameof(TableName)).Value;
            Properties = element.Descendants(EntityAppConfigProperty.ElementName).Select(c => new EntityAppConfigProperty(c)).ToList();
        }

        public XElement ToXElement()
        {
            var table = new XElement(ElementName);
            table.SetAttributeValue(nameof(TableName), TableName);
            var properties = Properties.Select(p => p.ToXElement());
            table.Add(properties);
            return table;
        }
    }
}
