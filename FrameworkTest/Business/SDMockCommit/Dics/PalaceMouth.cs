using FrameworkTest.Common.ValuesSolution;
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

        //<Option value="1" text="男" selected="0"/>
        //<Option value="2" text="女" selected="0"/>
        //<Option value="9" text="未说明的性别" selected="0"/>
        //<Option value="0" text="未知的性别" selected="0"/> 

        public readonly static Dictionary<string, string> PalaceMouth_HELE = new Dictionary<string, string>()
        {
            {"1" ,"0cm"},
            {"2" ,"1cm"},
            {"3" ,"2cm"},
            {"4" ,"3cm"},
            {"5" ,"4cm"},
            {"6" ,"5cm"},
            {"7" ,"6cm"},
            {"8" ,"7cm"},
            {"9" ,"8cm"},
            {"10" ,"9cm"},
            {"11" ,"10cm"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_PalaceMouth_By_PalaceMouth_HELE(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!PalaceMouth_HELE.ContainsKey(key))
                return "";
            return PalaceMouth_HELE[key].TrimEnd("cm");
        }
    }
}
