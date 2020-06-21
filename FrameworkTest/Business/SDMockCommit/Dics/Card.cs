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
        public readonly static Dictionary<string, string> CardType_CV02_01_101 = new Dictionary<string, string>()
        {
            {"01" ,"居民身份证"},
            {"02" ,"居民户口簿"},
            {"03" ,"护照"},
            {"04" ,"军官证"},
            {"05" ,"驾驶证"},
            {"06" ,"港澳居民来往内地通行证"},
            {"07" ,"台湾居民来往内地通行证"},
            {"99" ,"其他法定有效证件"},
        };


        public readonly static Dictionary<string, string> CardType_Hele = new Dictionary<string, string>()
        {
            {"1" ,"居民身份证"},
            {"2" ,"军人身份证"},
            {"3" ,"护照"},
            {"4" ,"港澳居民来往内地通行证"},
            {"5" ,"台湾居民来往内地通行证"},
            {"6" ,"中华人民共和国旅行证"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCardType_CV02_01_101ByCardType_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!CardType_Hele.ContainsKey(key))
                return "";
            var value = CardType_Hele[key];
            return CardType_CV02_01_101.FirstOrDefault(c => c.Value == value).Key ?? "";
        }
    }
}
