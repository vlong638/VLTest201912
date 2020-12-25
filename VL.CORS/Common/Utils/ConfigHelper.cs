using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ResearchAPI.CORS.Common
{
    public class ConfigHelper
    {
        public static COBusinessEntities GetBusinessEntities(string path)
        {
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(COBusinessEntities.ElementName);
            return new COBusinessEntities(root);
        }

        public static COBusinessEntities GetBusinessEntities(string directory, string file)
        {
            var path = Path.Combine(AppContext.BaseDirectory, directory, file);
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(COBusinessEntities.ElementName);
            return new COBusinessEntities(root);
        }

        public static Routers GetRouters(string directory, string file)
        {
            var path = Path.Combine(AppContext.BaseDirectory, directory, file);
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(Routers.ElementName);
            return new Routers(root);
        }

        public static BusinessEntityTemplate GetBusinessEntityTemplate(string directory, string file)
        {
            var path = Path.Combine(AppContext.BaseDirectory, directory, file);
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(BusinessEntityTemplate.ElementName);
            return new BusinessEntityTemplate(root);
        }

        public static COCustomBusinessEntity GetCustomBusinessEntity(string directory, string file)
        {
            var path = Path.Combine(AppContext.BaseDirectory, directory, file);
            XDocument doc = XDocument.Load(path);
            var root = doc.Element(COCustomBusinessEntity.ElementName);
            return new COCustomBusinessEntity(root);
        }

        /// <summary>
        /// 获取 列表sql配置
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public static SQLConfigV2 GetSQLConfigByDirectoryName(string viewName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Configs\\XMLConfigs", viewName, "SQLConfig.xml");
            XDocument doc = XDocument.Load(path);
            var tableElements = doc.Descendants(SQLConfigV2.ElementName);
            var tableConfigs = tableElements.Select(c => new SQLConfigV2(c));
            return tableConfigs.FirstOrDefault();
        }

        internal static List<COBusinessEntities> GetCOBusinessEntities()
        {
            var businessEntitiesCollection = new List<COBusinessEntities>();
            var directory = @"Configs/XMLConfigs/BusinessEntities";
            var files = Directory.GetFiles(directory);
            var bsfiles = files.Select(c => Path.GetFileName(c)).Where(c => c.StartsWith("BusinessEntities"));
            foreach (var bsfile in bsfiles)
            {
                var businessEntities = ConfigHelper.GetBusinessEntities(directory, bsfile);
                businessEntitiesCollection.Add(businessEntities);
            }
            return businessEntitiesCollection;
        }

        internal static Dictionary<T, string> GetDictionary<T>(string type)
        {
            var kvs = GetJsonConfig<string,T>(type);
            var result = new Dictionary<T, string>();
            foreach (var kv in kvs)
            {
                result.Add(kv.Value, kv.Key);
            }
            return result;
        }

        internal static List<VLKeyValue<T1, T2>> GetJsonConfig<T1,T2>(string type)
        {
            List<VLKeyValue<T1, T2>> values = new List<VLKeyValue<T1, T2>>();
            var file = (Path.Combine(AppContext.BaseDirectory, "Configs/JsonConfigs", type + ".json"));
            if (!System.IO.File.Exists(file))
            {
                values.Add(new VLKeyValue<T1, T2>(default(T1), default(T2)));
                System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(values));
                return values;
            }
            var data = System.IO.File.ReadAllText(file);
            values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VLKeyValue<T1, T2>>>(data);
            return values;
        }

        internal static List<VLKeyValue<T1, T2,T3,T4>> GetJsonConfig<T1, T2, T3, T4>(string type)
        {
            List<VLKeyValue<T1, T2, T3, T4>> values = new List<VLKeyValue<T1, T2, T3, T4>>();
            var file = (Path.Combine(AppContext.BaseDirectory, "Configs/JsonConfigs", type + ".json"));
            if (!System.IO.File.Exists(file))
            {
                values.Add(new VLKeyValue<T1, T2, T3, T4>(default(T1), default(T2), default(T3), default(T4)));
                System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(values));
                return values;
            }
            var data = System.IO.File.ReadAllText(file);
            values = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VLKeyValue<T1, T2, T3, T4>>>(data);
            return values;
        }
    }
}
