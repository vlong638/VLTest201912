using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Research.Common
{
    public class BusinessEntityTemplate
    {
        public const string ElementName = "BusinessEntityTemplate";

        public BusinessEntityTemplate()
        {
        }
        public BusinessEntityTemplate(XElement element)
        {
            ConnectionString = element.Attribute(nameof(ConnectionString))?.Value;
            TemplateName = element.Attribute(nameof(TemplateName))?.Value;
            BusinessEntity = new BusinessEntity(element.Element(BusinessEntity.ElementName));
            SQLConfig = new SQLConfig(element.Descendants(SQLConfig.ElementName).First());
            Router = new Router(element.Element(Router.ElementName));
        }

        public string ConnectionString { set; get; }
        public string TemplateName { set; get; }
        public BusinessEntity BusinessEntity { set; get; }
        public SQLConfig SQLConfig { set; get; }
        public Router Router { set; get; }
    }
}
