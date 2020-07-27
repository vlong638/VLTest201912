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
        public readonly static Dictionary<string, string> FundusUteri_FS = new Dictionary<string, string>()
        {
            {"1" ,"平脐"},
            {"1" ,"脐下一横指"},
            {"1" ,"脐下两横指"},
            {"1" ,"脐下三横指"},
            {"1" ,"降入骨盆"},
        };

        //<Option value="1" text="男" selected="0"/>
        //<Option value="2" text="女" selected="0"/>
        //<Option value="9" text="未说明的性别" selected="0"/>
        //<Option value="0" text="未知的性别" selected="0"/> 
        public readonly static Dictionary<string, string> FundusUteri_FM = new Dictionary<string, string>()
        {
            {"UF=0" ,"UF=0"},
            {"UF-1" ,"UF-1"},
            {"UF-2" ,"UF-2"},
            {"UF-3" ,"UF-3"},
        };

        /// <summary>
        /// 性别
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_FundusUteri_FS_By_FundusUteri_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "UF=0")
                return "3";
            if (key == "0")
                return "4";
            return key;
        }
    }
}
