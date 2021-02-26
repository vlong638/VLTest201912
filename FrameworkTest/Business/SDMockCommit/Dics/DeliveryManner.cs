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
        public readonly static Dictionary<string, string> DeliveryManner = new Dictionary<string, string>()
        {
            {"1" ,"顺产"},
            {"2" ,"剖宫产"},
            {"3" ,"钳产"},
            {"6" ,"臀位助产"},
            {"7" ,"其他"},
            {"8" ,"胎头吸引产"},
            {"9" ,"臀位牵引"},
            {"10" ,"剖宫取胎"},
            {"11" ,"中孕引产"},
        };

        public readonly static Dictionary<string, string> DeliveryManner_FM = new Dictionary<string, string>()
        {
            {"顺产" ,"顺产"},
            {"剖宫产" ,"剖宫产"},
            {"钳产" ,"钳产"},
            {"吸引产" ,"吸引产"},
            {"臂助产" ,"臂助产"},
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
            if (key == "顺产")
                return "1";
            else if (key == "剖宫产")
                return "2";
            else if (key == "钳产")
                return "3";
            else if (key == "臂助产")
                return "6";
            else if (key == "吸引产")
                return "8";
            return "7";
        }
    }
}
