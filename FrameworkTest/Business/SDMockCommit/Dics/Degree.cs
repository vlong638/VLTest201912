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
        public readonly static Dictionary<string, string> Degree_STD_CULTURALDEG = new Dictionary<string, string>()
        {
            {"11" ,"博士"},
            {"12" ,"硕士"},
            {"1" ,"研究生"},
            {"2" ,"本科"},
            {"3" ,"专科"},
            {"4" ,"中专"},
            {"5" ,"技工"},
            {"6" ,"高中"},
            {"7" ,"初中"},
            {"8" ,"小学"},
            {"9" ,"文盲或半文盲"},
        };

        public readonly static Dictionary<string, string> Degree_Hele = new Dictionary<string, string>()
        {
            {"10" ,"研究生"},
            {"11" ,"研究生毕业"},
            {"19" ,"研究生肄业"},
            {"20" ,"大学本科"},
            {"21" ,"大学毕业"},
            {"28" ,"相当大学毕业"},
            {"29" ,"大学肄业"},
            {"30" ,"大学专科和专科学校"},
            {"31" ,"专科毕业"},
            {"38" ,"相当专科毕业"},
            {"39" ,"专科肄业"},
            {"40" ,"中等专业学校或中等技术学校"},
            {"41" ,"中专毕业"},
            {"42" ,"中技毕业"},
            {"48" ,"相当中专或中技毕业"},
            {"49" ,"中专或中技肄业"},
            {"50" ,"技工学校"},
            {"51" ,"技工学校毕业"},
            {"59" ,"技工学校肄业"},
            {"60" ,"高中"},
            {"61" ,"高中毕业"},
            {"62" ,"职业高中毕业"},
            {"63" ,"农业高中毕业"},
            {"68" ,"相当高中毕业"},
            {"69" ,"高中肄业"},
            {"70" ,"初中"},
            {"71" ,"初中毕业"},
            {"72" ,"职业初中毕业"},
            {"73" ,"农业初中毕业"},
            {"78" ,"相当初中毕业"},
            {"79" ,"初中肄业"},
            {"80" ,"小学"},
            {"81" ,"小学毕业"},
            {"88" ,"相当小学毕业"},
            {"89" ,"小学肄业"},
            {"90" ,"文盲或半文盲"},
            {"91" ,"未分类"},
        };

        //注意特殊值有
        //{"11" ,"博士"},
        //{"12" ,"硕士"},
        //注意特殊值有
        //{"91" ,"未分类"},
        /// <summary>
        /// 文化程度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDegree_STD_CULTURALDEGByDegree_Hele(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!Degree_Hele.ContainsKey(key))
                return "";
            if (key == "91")
                return "";
            if (key == "11")
                return "1";
            return Degree_STD_CULTURALDEG.FirstOrDefault(c => key.StartsWith(c.Key)).Key ?? "";
        }
    }
}
