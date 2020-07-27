using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 宫底
    /// </summary>
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> FundusUteri_FS = new Dictionary<string, string>()
        {
            {"1" ,"平脐"},
            {"2" ,"脐下一横指"},
            {"3" ,"脐下两横指"},
            {"4" ,"脐下三横指"},
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
        /// 宫底
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_FundusUteri_FS_By_FundusUteri_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "UF=0")
                return "1";
            else if (key == "UF-1")
                return "2";
            else if (key == "UF-2")
                return "3";
            else if (key == "UF-3")
                return "4";
            return key;
        }
    }
}
