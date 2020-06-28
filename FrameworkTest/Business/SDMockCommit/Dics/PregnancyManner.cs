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
        public readonly static Dictionary<string, string> PregnancyManner = new Dictionary<string, string>()
        {
            {"自然受孕" ,"自然受孕"},
            {"宫腔内人工受精" ,"宫腔内人工受精"},
            {"胚胎移植" ,"胚胎移植"},
        };

        public readonly static Dictionary<string, string> PregnancyManner_Hele = new Dictionary<string, string>()
        {
            {"1" ,"自然受孕"},
            {"2" ,"人工受精"},
            {"3" ,"试管婴儿"},
            {"4" ,"促排卵怀孕"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetPregnancyMannerByPregnancyManner_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!CardType_Hele.ContainsKey(key))
                return "";
            var value = CardType_Hele[key];
            return PregnancyManner.FirstOrDefault(c => c.Value == value).Key ?? "";
        }
    }
}
