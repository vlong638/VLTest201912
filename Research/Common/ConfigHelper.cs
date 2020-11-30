using System;
using System.IO;
using System.Xml.Linq;

namespace Research.Common
{
    public class ConfigHelper
    {
        public static BusinessEntities GetBusinessEntities()
        {
            //TODO 正式版需去掉测试用代码 VLTest项目
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfigs\\VLTest项目", "BusinessEntities.xml");
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(BusinessEntities.ElementName);
            return new BusinessEntities(root);
        }
        public static Routers GetRouters()
        {
            //TODO 正式版需去掉测试用代码 VLTest项目
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfigs\\VLTest项目", "Routers.xml");
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(Routers.ElementName);
            return new Routers(root);
        }
    }
}
