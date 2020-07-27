using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 分娩方式
    /// </summary>
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> DeliveryManner = new Dictionary<string, string>()
        {
            {"1" ,"顺产"},
            {"2" ,"剖宫产"},
            {"3" ,"钳产"},
        };

        public readonly static Dictionary<string, string> DeliveryManner_FM = new Dictionary<string, string>()
        {
            {"顺产" ,"顺产"},
            {"剖宫产" ,"剖宫产"},
            {"钳产" ,"钳产"},
            {"吸引产" ,"吸引产"},
            {"臀助产/臀牵引" ,"臀助产/臀牵引"},
        };

        /// <summary>
        /// 分娩方式
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDeliveryMannerByDeliveryManner_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!DeliveryManner_FM.ContainsKey(key))
                return "";
            if (key == "顺产")
                return "1";
            else if (key == "剖宫产")
                return "2";
            else if (key == "钳产")
                return "3";
            return "";
        }
    }
}
