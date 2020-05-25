using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VL.Consoling.SemiAutoExport
{
    /// <summary>
    /// 配置上下文
    /// </summary>
    public class ConfigContext
    {
        static List<FileConfigSource> ConfigFile { set; get; }
        static Dictionary<int, string> SQLMapper { set; get; }

        public static List<FileConfigSource> GetFileConfig()
        {
            if (ConfigFile==null)
            {
                ConfigFile= new List<FileConfigSource>() {
                    new FileConfigSource(){
                        FunctionCategory=(int)FunctionCategory.门诊病历,
                        SubFunctionCategory =(int)SubFunctionCategory.女方信息,
                        JoinSQL = $"left join XXXXXXXX1 女方信息A on 女方信息A.通用关联 = Main.通用关联{Environment.NewLine}left join XXXXXXXX2 女方信息B on 女方信息B.通用关联 = Main.通用关联",
                        Fields =new List<FileConfigField>()
                        {
                            new FileConfigField((int) SubFunctionField.女方信息_助记符,"女方信息A.助记符"),
                            new FileConfigField((int) SubFunctionField.女方信息_姓名,"女方信息A.姓名"),
                            new FileConfigField((int) SubFunctionField.女方信息_年龄,"女方信息A.年龄"),
                            new FileConfigField((int) SubFunctionField.女方信息_体重,"女方信息A.体重"),
                            new FileConfigField((int) SubFunctionField.女方信息_性别,"女方信息A.性别",ControlType.下拉项,new Dictionary<int, string>(){ { 1, "男" } ,{ 2,"女"} }),
                            new FileConfigField((int) SubFunctionField.女方信息_职业,"女方信息B.职业"),
                            new FileConfigField((int) SubFunctionField.女方信息_单位,"女方信息B.单位"),
                        }
                    },
                    new FileConfigSource(){
                        FunctionCategory=(int)FunctionCategory.门诊病历,
                        SubFunctionCategory =(int)SubFunctionCategory.女方性病史,
                        JoinSQL = $@"left join XXXXXXXX1 女方性病史A on 女方性病史A.通用关联 = Main.通用关联",
                        Fields =new List<FileConfigField>()
                        {
                            new FileConfigField((int) SubFunctionField.女方性病史_名称,"女方性病史.名称"),
                            new FileConfigField((int) SubFunctionField.女方性病史_编码,"女方性病史.编码"),
                        }
                    },
                };
                SQLMapper = new Dictionary<int, string>();
                foreach (var function in ConfigFile)
                {
                    SQLMapper.Add(function.SubFunctionCategory, function.JoinSQL);
                    foreach (var field in function.Fields)
                    {
                        SQLMapper.Add(field.SubFunctionField, field.FieldSQL);
                    }
                }
            }

            return ConfigFile;
        }

        internal static string GetSQL(int subFunctionCategoryOrSubFunctionField)
        {
            if (SQLMapper == null)
            {
                GetFileConfig();
            }
            return SQLMapper[subFunctionCategoryOrSubFunctionField];
        }

        static int Indexer = 0;
        public static int GetIndex()
        {
            if (Indexer == 100)
                Indexer = 1;
            return Indexer++;
        }
    }
}
