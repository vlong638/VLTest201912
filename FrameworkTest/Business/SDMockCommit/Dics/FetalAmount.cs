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
        public readonly static Dictionary<string, string> FetalAmount = new Dictionary<string, string>()
        {
            {"1" ,"1个"},
            {"2" ,"2个"},
            {"3" ,"3个以上"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_FetalAmountByAmount(int amount)
        {
            if (amount==0)
                return "";
            if (amount < 3)
                return amount.ToString();
            return "3";
        }
    }
}
