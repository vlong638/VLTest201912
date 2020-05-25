using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VL.Consoling.SemiAutoExport
{
    /// <summary>
    /// 配置文件输出项集合
    /// </summary>
    public class ConfigEntityConditionCollection : List<ConfigEntityCondition>
    {
        public static char SplitterItem = ',';
        public static char SplitterField = '\'';
        public static char SplitterValue = '|';
        //TODO 注意! 分隔符为保留字符不允许输入

        /// <summary>
        /// 条件名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 条件说明
        /// </summary>
        public string Remark { set; get; }

        public ConfigEntityConditionCollection() { }
        public ConfigEntityConditionCollection(string str)
        {
            var itemStrs = str.Split(SplitterItem);
            foreach (var itemStr in itemStrs)
            {
                this.Add(new ConfigEntityCondition(itemStr));
            }
        }

        public override string ToString()
        {
            return string.Join(SplitterItem.ToString(), this.Select(c => c.ToString()));
        }

        internal List<string> GetJoinSQLs()
        {
            return this.Select(c => c.GetJoinSQL()).ToList();
        }

        internal string GetCondtions()
        {
            var condition = string.Join(" and ", this.Select(c => c.GetConditionSQL()));
            return string.IsNullOrEmpty(condition) ? "" : "where " + condition;
        }
    }

    /// <summary>
    /// 配置文件
    /// </summary>
    public class ConfigEntityCondition
    {
        public int FunctionCategory { set; get; }
        public int SubFunctionCategory { set; get; }
        public int SubFunctionField { set; get; }
        public OperatorType OperatorType { set; get; }
        public string Value { set; get; }
        
        public ConfigEntityCondition() { }
        public ConfigEntityCondition(string str)
        {
            var values = str.Split(ConfigEntityConditionCollection.SplitterField);
            if (values.Count() == 3)
            {
                if (int.TryParse(values[0], out int functionCategory))
                {
                    FunctionCategory = functionCategory;
                }
                else
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
                if (int.TryParse(values[1], out int subFunctionCategory))
                {
                    SubFunctionCategory = subFunctionCategory;
                }
                else
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
                if (int.TryParse(values[2], out int subFunctionField))
                {
                    this.SubFunctionField = subFunctionField;
                }
                else
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
                if (int.TryParse(values[3], out int operatorType))
                {
                    OperatorType = (OperatorType)Enum.Parse(typeof(OperatorType), values[3]);
                }
                else
                {
                    throw new NotImplementedException("数据格式不服务规范");
                }
            }
        }

        string GetCompareSQL(OperatorType operatorType, string parameterName)
        {
            //TODO 需要额外考虑 数据库类型为字符的情况
            switch (operatorType)
            {
                case OperatorType.大于:
                    return ">" + parameterName;
                case OperatorType.等于:
                    return "=" + parameterName;
                case OperatorType.小于:
                    return "<" + parameterName;
                default:
                    throw new NotImplementedException("尚未支持该比较操作");
            }
        }

        public override string ToString()
        {
            return $@"{FunctionCategory
                + ConfigEntityConditionCollection.SplitterField.ToString()
                + SubFunctionCategory
                + ConfigEntityConditionCollection.SplitterField.ToString()
                + SubFunctionField
                + ConfigEntityConditionCollection.SplitterField.ToString()
                + OperatorType
                + ConfigEntityConditionCollection.SplitterField.ToString()
                + Value
                }";
        }

        string JoinSQL { set; get; }
        string ConditionSQL { set; get; }
        public string Parameter { set; get; }//TODO 参数化

        internal string GetJoinSQL()
        {
            if (string.IsNullOrEmpty(JoinSQL))
            {
                JoinSQL = ConfigContext.GetSQL(SubFunctionCategory);
            }
            return JoinSQL;
        }

        internal string GetConditionSQL()
        {
            if (string.IsNullOrEmpty(ConditionSQL))
            {
                Parameter = "@" + ConfigContext.GetSQL(SubFunctionField) + ConfigContext.GetIndex();
                ConditionSQL = $@"{ConfigContext.GetSQL(SubFunctionField)}{ GetCompareSQL(OperatorType, Parameter) }";
            }
            return ConditionSQL;
        }
    }
}
