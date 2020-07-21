using FrameworkTest.Common.ValuesSolution;
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
        ////运行时常量 readonly static (引用型),编译时常量 const (值类型)
        //public readonly static Dictionary<string, string> BabySex = new Dictionary<string, string>()
        //{
        //    {"1" ,"男"},
        //    {"2" ,"女"},
        //    {"3" ,"不详"},
        //    {"4" ,"两性畸形"},
        //};

        ////<Option value="1" text="男" selected="0"/>
        ////<Option value="2" text="女" selected="0"/>
        ////<Option value="9" text="未说明的性别" selected="0"/>
        ////<Option value="0" text="未知的性别" selected="0"/> 

        //public readonly static Dictionary<string, string> BabySex_HELE = new Dictionary<string, string>()
        //{
        //    {"1" ,"男"},
        //    {"2" ,"女"},
        //    {"9" ,"未说明的性别"},
        //    {"0" ,"未知的性别"},
        //};

        ///// <summary>
        ///// return "" when not contains key
        ///// return matchedKey when matched
        ///// return "" when not matched
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static string Get_BabySex_By_BabySex_HELE(string key)
        //{
        //    if (string.IsNullOrEmpty(key))
        //        return "";
        //    if (key == "9")
        //        return "3";
        //    if (key == "0")
        //        return "4";
        //    return key;
        //}

        public static class DateTime {
            public static string DateFormatter = "yyyy-MM-dd";
            public static string DateTimeFormatter = "yyyy-MM-dd HH:mm:ss";

            /// <summary>
            /// 获得生产年龄
            /// </summary>
            /// <param name="birthday">生日</param>
            /// <param name="gestationalDate">预产期</param>
            /// <returns></returns>
            public static int? GetYearsBy(System.DateTime? birthday, System.DateTime? gestationalDate) {
                if (!birthday.IsValidDateTime()|| !gestationalDate.IsValidDateTime())
                    return null;

                int years = gestationalDate.Value.Year - birthday.Value.Year;
                if (gestationalDate.Value.Month > birthday.Value.Month || (gestationalDate.Value.Month == birthday.Value.Month && gestationalDate.Value.Day > birthday.Value.Day))
                {
                    years -= 1;
                }
                return years;
            }
        }
    }
}
