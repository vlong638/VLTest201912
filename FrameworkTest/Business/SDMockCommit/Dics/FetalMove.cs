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
        public readonly static Dictionary<string, string> FetalMove = new Dictionary<string, string>()
        {
            { "有","有"},
            { "无","无"},
            { "胎动多","胎动多"},
            { "胎动少","胎动少"},
        };

        public readonly static Dictionary<string, string> FetalMove_HELE = new Dictionary<string, string>()
        {
            { "1","正常"},
            { "2","增多"},
            { "3","减少"},
            { "4","消失"},
        };

        /// <summary>
        /// 羊水
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_FetalMove_By_FetalMove_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            switch (key)
            {
                case "1":
                    return "有";
                case "2":
                    return "胎动多";
                case "3":
                    return "胎动少";
                case "4":
                    return "无";
                default:
                    return "";
            }

        }
    }
}
   
