using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace FrameworkTest.Common.ConfigSolution
{
    public class ConfigHelper
    {
        #region AppConfig
        public static void SetAppConfig(string appKey, string appValue)
        {
            XmlDocument doc = new XmlDocument();
            //获得配置文件的全路径
            string strFileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + ".exe.config";
            doc.Load(strFileName);
            //找出名称为“add”的所有元素
            XmlNodeList nodes = doc.GetElementsByTagName("add");
            for (int i = 0; i < nodes.Count; i++)
            {
                //获得将当前元素的key属性
                XmlAttribute att = nodes[i].Attributes["key"];
                //根据元素的第一个属性来判断当前的元素是不是目标元素
                if (att.Value == appKey)
                {
                    //对目标元素中的第二个属性赋值
                    att = nodes[i].Attributes["value"];
                    att.Value = appValue;
                    break;
                }
            }
            //保存上面的修改
            doc.Save(strFileName);
        }

        public static string GetAppConfig(string appKey)
        {
            return ConfigurationManager.AppSettings[appKey];
        }
        #endregion

        #region VLConfig
        public static Dictionary<string,string> GetVLConfig(string text)
        {
            var config = new VLConfig(text);
            return config.GetKeyValues();
        }
        #endregion
    }

}
