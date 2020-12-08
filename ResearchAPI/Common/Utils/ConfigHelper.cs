using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ResearchAPI.Common
{
    public class ConfigHelper
    {
        public static BusinessEntities GetBusinessEntities(string directory, string file)
        {
            XDocument doc = GetDoc(directory, file);
            var root = doc.Element(BusinessEntities.ElementName);
            return new BusinessEntities(root);
        }

        public static Routers GetRouters(string directory, string file)
        {
            XDocument doc = GetDoc(directory, file);
            var root = doc.Element(Routers.ElementName);
            return new Routers(root);
        }

        public static BusinessEntityTemplate GetBusinessEntityTemplate(string directory, string file)
        {
            XDocument doc = GetDoc(directory, file);
            var root = doc.Element(BusinessEntityTemplate.ElementName);
            return new BusinessEntityTemplate(root);
        }

        public static CustomBusinessEntity GetCustomBusinessEntity(string directory, string file)
        {
            XDocument doc = GetDoc(directory, file);
            var root = doc.Element(CustomBusinessEntity.ElementName);
            return new CustomBusinessEntity(root);
        }

        private static XDocument GetDoc(string directory, string file)
        {
            var path = Path.Combine(AppContext.BaseDirectory, directory, file);
            XDocument doc = XDocument.Load(path);
            return doc;
        }
    }
}
