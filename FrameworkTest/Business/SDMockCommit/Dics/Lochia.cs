using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 恶露
    /// </summary>
    public partial class VLConstraints
    {
        public readonly static Dictionary<string, string> Lochia_FS = new Dictionary<string, string>()
        {
            { "1" , "无" },
            { "2" , "有" },
        };

        public readonly static Dictionary<string, string> Lochia_FM = new Dictionary<string, string>()
        {
            {"异常" ,"异常"},
            {"正常" ,"正常"},
        };

        public static string Get_Lochia_FS_By_Lochia_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "正常")
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
