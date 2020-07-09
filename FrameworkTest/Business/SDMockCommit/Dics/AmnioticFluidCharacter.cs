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
        public readonly static Dictionary<string, string> AmnioticFluidCharacter = new Dictionary<string, string>()
        {
            { "1","清"},
            { "2","浊"},
        };

        public readonly static Dictionary<string, string> AmnioticFluidCharacter_HELE = new Dictionary<string, string>()
        {
            { "1","清"},
            { "2","Ⅰ度"},
            { "3","Ⅱ度"},
            { "4","Ⅲ度"},
            { "5","臭"},
            { "6","Ⅲ度、臭"},
            { "7","未破"},
            { "8","未见"},
            { "9","血性"},
        };

        /// <summary>
        /// 羊水
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_AmnioticFluidCharacter_By_AmnioticFluidCharacter_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            switch (key)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                    return "1";
                case "5":
                case "6":
                    return "2";
                case "7":
                case "8":
                case "9":
                default:
                    return "";
            }

        }
    }
}
   
