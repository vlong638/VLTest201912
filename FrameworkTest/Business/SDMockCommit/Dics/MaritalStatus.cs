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

        public readonly static Dictionary<string, string> MaritalStatus_STD_MARRIAGE = new Dictionary<string, string>()
        {
            //{"10" ,"未婚"},
            //{"20" ,"已婚"},
            //{"21" ,"初婚"},
            //{"22" ,"再婚"},
            //{"23" ,"复婚"},
            //{"30" ,"丧偶"},
            //{"40" ,"离婚"},
            //{"90" ,"其他"},     
            
            {"2" ,"未婚"},
            {"1" ,"已婚"},
            {"4" ,"丧偶"},
            {"3" ,"离婚"},
            {"5" ,"未说明的婚姻状况"},
        };


        public readonly static Dictionary<string, string> MaritalStatus_HELE = new Dictionary<string, string>()
        {
            {"1" ,"未婚"},
            {"2" ,"已婚"},
            {"5" ,"再婚"},
            {"4" ,"丧偶"},
            {"3" ,"离婚"},
            {"9" ,"未说明的婚姻状况"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMaritalStatus_STD_MARRIAGEByMaritalStatus_HELE(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!MaritalStatus_HELE.ContainsKey(key))
                return "";
            var value = MaritalStatus_HELE[key];
            return MaritalStatus_STD_MARRIAGE.FirstOrDefault(c => c.Value == value).Key ?? "";
        }
    }
}
