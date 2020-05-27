﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VLTest2015.Utils
{
    class EntityAppConfigTable
    {
        public static string ElementName = "TableName";

        public string TableName { set; get; }
        public List<EntityAppConfigProperty> Properties { set; get; }

        public EntityAppConfigTable()
        { 

        }
        public EntityAppConfigTable(XElement element)
        {
            TableName = element.Attribute(ElementName).Value;
            Properties = element.Descendants(EntityAppConfigProperty.ElementName).Select(c => new EntityAppConfigProperty(c)).ToList();
        }

        public XElement ToXElement()
        {
            var table = new XElement("Table");
            table.SetAttributeValue(nameof(TableName), TableName);
            var properties = Properties.Select(p => p.ToXElement());
            table.Add(properties);
            return table;
        }
    }
}
