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
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> EdemaStatus = new Dictionary<string, string>()
        {
            {"1" ,"无"},
            {"2" ,"+"},
            {"3" ,"++"},
            {"4" ,"+++"},
        };

        public readonly static Dictionary<string, string> EdemaStatus_Hele = new Dictionary<string, string>()
        {
            {"1" ,"无"},
            {"2" ,"+"},
            {"3" ,"++"},
            {"4" ,"+++"},
            {"5" ,"++++"},
        };

        public static string GetEdemaStatus_By_EdemaStatus_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!EdemaStatus_Hele.ContainsKey(key))
                return "";
            if (key== "5")
                return "";

            return EdemaStatus.ContainsKey(key) ? EdemaStatus[key] : "";
        }
    }
}
