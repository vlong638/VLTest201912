using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    //(\w+)\s+([\w\(\)]+)
    //{"$1" ,"$2"},
    //<Option value = "(\w+)" text="([\w\(\\、]+)"/>
    //存在中英文标点符号问题,作替换
    //存在名称不对称问题 后续处理需做应对 忽略无法匹配的
    //<Option value="(\w+)" text="(.+)" rule=.+\r\n
    //{"$1",@"$2"},\r\n
    public partial class VLConstraints
    {
        public readonly static Dictionary<string, string> Dysmenorrhea = new Dictionary<string, string>()
        {
            {"有" ,"有"},
            {"无" ,"无"},
        };

        public readonly static Dictionary<string, string> Dysmenorrhea_Hele = new Dictionary<string, string>()
        {
            {"1" ,"无"},
            {"2" ,"轻"},
            {"3" ,"重"},
            {"4" ,"中"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDysmenorrheaByDysmenorrhea_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!CardType_Hele.ContainsKey(key))
                return "";
            return key == "1" ? "无" : "有";

            //var value = CardType_Hele[key];
            //return CardType_CV02_01_101.FirstOrDefault(c => c.Value == value).Key ?? "";
        }
    }
}
