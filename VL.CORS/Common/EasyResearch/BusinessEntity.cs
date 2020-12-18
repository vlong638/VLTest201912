﻿using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class BusinessEntity
    {
        public const string ElementName = "BusinessEntity";

        public BusinessEntity()
        {
        }
        public BusinessEntity(XElement element)
        {
            Id = element.Attribute(nameof(Id)).Value.ToLong().Value;
            DisplayName = element.Attribute(nameof(DisplayName))?.Value;
            Template = element.Attribute(nameof(Template))?.Value;
            Properties.AddRange(element.Descendants(BusinessEntityProperty.ElementName).Select(c => new BusinessEntityProperty(c)));
        }

        public long Id { set; get; }
        public string DisplayName { set; get; }
        public List<BusinessEntityProperty> Properties { set; get; } = new List<BusinessEntityProperty>();
        public string Template { get; set; }
        public SQLConfigV3 SQLConfig { get; internal set; }
    }
}
