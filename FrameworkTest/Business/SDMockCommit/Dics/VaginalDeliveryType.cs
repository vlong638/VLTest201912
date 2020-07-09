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
        public readonly static Dictionary<string, string> VaginalDeliveryType = new Dictionary<string, string>()
        {
            { "顺产", "顺产" },
            { "钳产", "钳产" },
            {"臀位牵引" ,"臀位牵引"},
            {"负压吸引产","负压吸引产"},
        };

        //<Option value = "22" text="无"/>
        //<Option value = "1" text="顺产-足月-健"/>
        //<Option value = "2" text="足月产-亡"/>
        //<Option value = "23" text="巨大胎"/>
        //<Option value = "3" text="顺产-早产-健"/>
        //<Option value = "4" text="早产-亡"/>
        //<Option value = "5" text="畸形-健"/>
        //<Option value = "6" text="畸形-亡"/>
        //<Option value = "7" text="双胎"/>
        //<Option value = "8" text="死胎"/>
        //<Option value = "9" text="死产"/>
        //<Option value = "10" text="胎位异常"/>
        //<Option value = "11" text="妊高症"/>
        //<Option value = "12" text="前置胎盘"/>
        //<Option value = "13" text="胎盘早剥"/>
        //<Option value = "14" text="阴道手术-吸引产"/>
        //<Option value = "141" text="阴道手术-产钳"/>
        //<Option value = "15" text="剖宫产-足月"/>
        //<Option value = "151" text="剖宫产-早产"/>
        //<Option value = "16" text="人流"/>
        //<Option value = "17" text="自然流产"/>
        //<Option value = "18" text="引产"/>
        //<Option value = "19" text="药流"/>
        //<Option value = "20" text="胎停"/>
        //<Option value = "21" text="宫外孕"/>
        //<Option value = "25" text="稽留流产"/>
        //<Option value = "26" text="生化"/>

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetVaginalDeliveryType(List<string> pregstatuss)
        {
            if (pregstatuss.FirstOrDefault(c => c.Contains("顺产")) != null)
            {
                return "顺产";
            }
            else if (pregstatuss.FirstOrDefault(c => c.Contains("产钳")) != null)
            {
                return "钳产";
            }
            else if (pregstatuss.FirstOrDefault(c => c.Contains("吸引产")) != null)
            {
                return "负压吸引产";
            }
            else 
            {
                return "";
            }
        }
    }
}
