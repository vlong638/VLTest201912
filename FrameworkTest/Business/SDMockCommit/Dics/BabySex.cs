﻿using System;
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
        public readonly static Dictionary<string, string> BabySex = new Dictionary<string, string>()
        {
            {"1" ,"男"},
            {"2" ,"女"},
            {"3" ,"不详"},
            {"4" ,"两性畸形"},
        };

        //<Option value="1" text="男" selected="0"/>
        //<Option value="2" text="女" selected="0"/>
        //<Option value="9" text="未说明的性别" selected="0"/>
        //<Option value="0" text="未知的性别" selected="0"/> 

        public readonly static Dictionary<string, string> BabySex_HELE = new Dictionary<string, string>()
        {
            {"1" ,"男"},
            {"2" ,"女"},
            {"9" ,"未说明的性别"},
            {"0" ,"未知的性别"},
        };

        /// <summary>
        /// 性别
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_BabySex_By_BabySex_HELE(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (key == "9")
                return "3";
            if (key == "0")
                return "4";
            return key;
        }

        /// <summary>
        /// 新生儿性别
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Get_BabySex_FS_By_BabySex_FM(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            key = key.Trim(',');
            if (key.StartsWith("男"))
                return "1";
            if (key.StartsWith("女"))
                return "2";
            return key;
        }
    }
}
