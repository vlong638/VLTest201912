using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    /// <summary>
    /// 乳房
    /// </summary>
    public partial class VLConstraints
    {
        //运行时常量 readonly static (引用型),编译时常量 const (值类型)
        public readonly static Dictionary<string, string> BreastStatus_FS = new Dictionary<string, string>()
        {
            {"1" ,"未见异常"},
            {"?" ,"红肿"},
            {"?" ,"乳头皲裂"},
        };

        //<Option value="1" text="男" selected="0"/>
        //<Option value="2" text="女" selected="0"/>
        //<Option value="9" text="未说明的性别" selected="0"/>
        //<Option value="0" text="未知的性别" selected="0"/> 

        public readonly static Dictionary<string, string> BreastStatus_FY = new Dictionary<string, string>()
        {
            {"正常" ,"正常"},
        };

        /// <summary>
        /// 乳房
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_BreastStatus_FS_By_BreastStatus_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "正常")
                return "1";
            return "";
        }
    }
}
