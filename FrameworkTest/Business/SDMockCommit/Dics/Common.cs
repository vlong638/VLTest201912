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
        //FS
        //未见异常
        //异常：
        //未做


        //HELE
        //<Option value = "0" text="正常" selected="1"/>
        //<Option value = "1" text="异常"/>


        public static string Get_Common_AbnormalCheck(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "未做";
            if (key == "0")
                return "未见异常";
            if (key == "1")
                return "异常：";
            return key;
        }
    }
}
