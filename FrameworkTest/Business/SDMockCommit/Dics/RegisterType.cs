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
        public readonly static Dictionary<string, string> RegisterType_STD_REGISTERT2PE= new Dictionary<string, string>()
        {
            {"1" ,"城镇或非农籍"},
            {"2" ,"乡村或农籍"},
        };

        public readonly static Dictionary<string, string> RegisterType_HELE = new Dictionary<string, string>()
        {
            {"1" ,"是"},//农籍
            {"2" ,"否"},//非农籍
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRegisterType_STD_REGISTERT2PE_By_RegisterType_HELE(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "1")
                return "2";
            if (key == "2")
                return "1";
            return "";
        }

    }
}
