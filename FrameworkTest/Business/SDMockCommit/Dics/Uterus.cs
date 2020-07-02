using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    //(\w+)\s+([\w\(\)]+)
    //{"$1" ,"$2"},
    //存在中英文标点符号问题,作替换
    //存在名称不对称问题 后续处理需做应对 忽略无法匹配的
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> Uterus = new Dictionary<string, string>()
        {
            {"有" ,"有"},
            {"无" ,"无"},
            {"3" ,"不详"},
            {"4" ,"两性畸形"},
        };

        //<Option value="1" text="男" selected="0"/>
        //<Option value="2" text="女" selected="0"/>
        //<Option value="9" text="未说明的性别" selected="0"/>
        //<Option value="0" text="未知的性别" selected="0"/> 

        public readonly static Dictionary<string, string> Uterus_HELE = new Dictionary<string, string>()
        {
            {"1" ,"异常"},
            {"0" ,"正常"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_Uterus_By_Uterus_HELE(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "1")
                return "有";
            if (key == "0")
                return "无";
            return "";
        }
    }
}
