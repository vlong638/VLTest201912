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
        public readonly static Dictionary<string, string> Link = new Dictionary<string, string>()
        {
            {"已衔接" ,"已衔接"},
            {"未衔接" ,"未衔接"},
        };


        public readonly static Dictionary<string, string> Link_HELE = new Dictionary<string, string>()
        {
            {"2" ,"未衔接"},
            {"1" ,"衔接"},
            {"3" ,"半衔接"},
        };

        /// <summary>
        /// return "" when not contains key
        /// return matchedKey when matched
        /// return "" when not matched
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLinkByLink_HELE(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "";
            if (!Link_HELE.ContainsKey(key))
                return "";
            switch (key)
            {
                case "1":
                case "3":
                    return "已衔接";
                case "2":
                    return "未衔接";
                default:
                    return "";
            }
        }
    }
}
