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
        public readonly static Dictionary<string, string> Apnea_FS = new Dictionary<string, string>()
        {
            {"1" ,"无"},
            {"2" ,"轻度"},
            {"3" ,"重度"},
        };

        public readonly static Dictionary<string, string> Apnea_FM = new Dictionary<string, string>()
        {
            {"无" ,"无"},
            {"轻度" ,"轻度"},
            {"重度" ,"重度"},
        };

        public static string Get_Apnea_FS_By_Apnea_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key.StartsWith("无"))
                return "1";
            if (key.StartsWith("轻度"))
                return "2";
            if (key.StartsWith("重度"))
                return "3";
            return key;
        }
    }
}
