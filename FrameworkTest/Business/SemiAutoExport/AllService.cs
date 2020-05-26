using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace VL.Consoling.SemiAutoExport
{
    public class AllService : IAllService
    {
        public ConfigEntity GetConfigEntity(long configEntityId)
        {
            //测试用示例,具体实现需要根据Id获取

            ConfigEntity myConfig = new ConfigEntity()
            {
                Name = "测试配置",
                ConfigEntityOutputs = new ConfigEntityOutputCollection()
                    {
                        new ConfigEntityOutput()
                        {
                            FunctionCategory = (int)FunctionCategory.门诊病历,
                            SubFunctionCategory = (int)SubFunctionCategory.女方信息,
                            SubFunctionFields = new List<int>() {
                                (int)SubFunctionField.女方信息_姓名,
                                (int)SubFunctionField.女方信息_年龄,
                                (int)SubFunctionField.女方信息_助记符,
                            },
                        }
                        , new ConfigEntityOutput()
                        {
                            FunctionCategory = (int)FunctionCategory.门诊病历,
                            SubFunctionCategory = (int)SubFunctionCategory.女方性病史,
                            SubFunctionFields = new List<int>() {
                                (int)SubFunctionField.女方性病史_名称,
                                (int)SubFunctionField.女方性病史_编码,
                            },
                        }
                    },
                ConfigEntityConditions = new ConfigEntityConditionCollection()
                    {
                        new ConfigEntityCondition()
                        {
                            FunctionCategory = (int)FunctionCategory.门诊病历,
                            SubFunctionCategory = (int)SubFunctionCategory.女方信息,
                            SubFunctionField  = (int)SubFunctionField.女方信息_年龄,
                            OperatorType  = OperatorType.小于,
                            Value = "23",
                        }
                        ,new ConfigEntityCondition()
                        {
                            FunctionCategory = (int)FunctionCategory.门诊病历,
                            SubFunctionCategory = (int)SubFunctionCategory.女方信息,
                            SubFunctionField  = (int)SubFunctionField.女方信息_体重,
                            OperatorType  = OperatorType.大于,
                            Value = "60",
                        }
                    },
            };
            return myConfig;
        }

        public bool Delete(long configEntityId)
        {
            throw new NotImplementedException();
        }

        public ConfigEntity GetConditionDetail(long configEntityId)
        {
            throw new NotImplementedException();
        }

        public ConfigEntity GetConditionMain(long configEntityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 输出为表格数据,页面用
        /// </summary>
        /// <param name="configEntityId"></param>
        /// <returns></returns>
        public DataTable GetData(long configEntityId)
        {
            var config = GetConfigEntity(1);
            var sql = config.GetSQL();
            return new DataTable();
        }

        public byte[] GetExcel(long configEntityId)
        {
            throw new NotImplementedException();
        }

        public ConfigEntity GetMain(long configEntityId)
        {
            throw new NotImplementedException();
        }

        public ConfigEntity GetOutputs(long configEntityId)
        {
            throw new NotImplementedException();
        }

        public bool SaveConditionDetail(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool SaveConditionMain(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public long SaveMain(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool SaveOutputs(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConditionDetail(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateConditionMain(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMain(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOutputs(ConfigEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
