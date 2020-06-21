using System.Collections.Generic;
using System.Linq;

namespace FrameworkTest.Business.SDMockCommit
{
    //(\w+)\s+([\w\(\\、)]+)
    //{"$1" ,"$2"},
    //<Option value = "(\w+)" text="([\w\(\\、]+)"/>
    //存在中英文标点符号问题,作替换
    //存在名称不对称问题 后续处理需做应对 忽略无法匹配的
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> Occupation_STD_OCCUPATION = new Dictionary<string, string>()
        {
            {"1" ,"国家机关、党群组织、企业、事业单位负责人"},
            {"2" ,"专业技术人员"},
            {"3" ,"办事人员和有关人员"},
            {"4" ,"商业、服务业人员"},
            {"5" ,"农、林、牧、渔、水利业生产人员"},
            {"6" ,"生产、运输设备操作人员及有关人员"},
            {"7" ,"军人"},
            {"8" ,"不便分类的其他从业人员"},
        };

        public readonly static Dictionary<string, string> Occupation_Hele = new Dictionary<string, string>()
        {
            {"0" ,"国家机关、党群组织、企业、事业单位负责人"},
            {"1" ,"专业技术人员"},
            {"3" ,"办事人员和有关人员"},
            {"4" ,"商业、服务业人员"},
            {"5" ,"农、林、牧、渔、水利业生产人员"},
            {"7" ,"生产、运输设备操作人员及有关人员"},
            {"X" ,"军人"},
            {"Y" ,"不便分类的其他从业人员"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetOccupation_STD_OCCUPATIONByOccupation_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!Occupation_Hele.ContainsKey(key))
                return "";
            return Occupation_STD_OCCUPATION.FirstOrDefault(c => c.Value == Occupation_Hele[key]).Key ?? "";
        }
    }
}
