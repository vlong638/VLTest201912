using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;

namespace ResearchAPI.Common
{
    /// <summary>
    /// 表对值 条件项
    /// </summary>
    public class Field2ValueWhere : IWhere
    {
        public Field2ValueWhere(string entityName, string fieldName, WhereOperator @operator, string value = "", string valueFormat = "")
        {
            EntityName = entityName;
            FieldName = fieldName;
            Operator = @operator;
            Value = value;
            ValueFormat = valueFormat;
        }

        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public WhereOperator Operator { get; set; }
        public string Value { get; set; }
        public string ValueFormat { get; set; }

        public KeyValuePair<string, object>? GetParameter()
        {
            var field = new DBField();
            return new KeyValuePair<string, object>(GetParameterName(), field.GetValue(Value, ValueFormat));

        }

        private string GetParameterName()
        {
            return "@" + EntityName + "_" + FieldName;
        }

        public string GetValue()
        {
            if (ValueFormat.IsNullOrEmpty())
            {
                throw new NotImplementedException("无效的数据格式化类型");
            }
            return string.Format(ValueFormat, Value);
        }

        public string ToSQL(Dictionary<string, string> tableAlias)
        {
            //TODO 这里理论上需要知道数据库的知识
            var field = new DBField();
            return $"{tableAlias[EntityName]}.{FieldName} { Operator.ToSQL() } { GetParameterName() }";
        }
    }
}
