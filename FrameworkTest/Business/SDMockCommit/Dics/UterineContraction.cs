using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    //(\w+)\s+([\w\(\)]+)
    //{"$1" ,"$2"},
    //<Option value = "(\w+)" text="([\w\(\\、\（\）]+)"/>
    //存在中英文标点符号问题,作替换
    //存在名称不对称问题 后续处理需做应对 忽略无法匹配的
    public partial class VLConstraints
    {
        //public readonly static Dictionary<string, string> UterineContraction = new Dictionary<string, string>()
        //{
        //    //无宫缩
        //};

        //public readonly static Dictionary<string, string> UterineContraction_HELE = new Dictionary<string, string>()
        //{
        //    //<Option value="1" text="无" selected="1"/>
        //    //<Option value="4" text="偶有" selected="0"/>
        //    //<Option value="2" text="规则" selected="0"/>
        //    //<Option value="3" text="不规则" selected="0"/>
        //};

        /// <summary>
        /// 无宫缩
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_UterineContraction_By_UterineContraction_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key=="1")
                return "无宫缩";
            return "";
        }
    }
}
   
