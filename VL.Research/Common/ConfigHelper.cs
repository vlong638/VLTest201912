using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace BBee.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 获取 列表配置
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static ListConfig GetListConfigByDirectoryName(string viewName)
        {
            ListConfig tableConfig;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", viewName, "ListConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ListConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new ListConfig(c));
            tableConfig = tableConfigs.FirstOrDefault();
            return tableConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static ListConfig GetListConfigByTagName(string viewName)
        {
            ListConfig tableConfig;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "ListConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(ListConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new ListConfig(c));
            tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == viewName);
            return tableConfig;
        }

        /// <summary>
        /// 获取 列表sql配置
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static SQLConfig GetSQLConfigByDirectoryName(string viewName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", viewName, "SQLConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(SQLConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new SQLConfig(c));
            return tableConfigs.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static SQLConfig GetSQLConfigByTagName(string viewName)
        {
            SQLConfig tableConfig;
            var path = Path.Combine(AppContext.BaseDirectory, "XMLConfig", "SQLConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(SQLConfig.NodeElementName);
            var tableConfigs = tableElements.Select(c => new SQLConfig(c));
            tableConfig = tableConfigs.FirstOrDefault(c => c.ViewName == viewName);
            return tableConfig;
        }
    }
}
