using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 表对值 条件项
    /// </summary>
    public class Field2ValueWhere
    {
        public Field2ValueWhere(string entityName, string fieldName, WhereOperator @operator, string value = "", string valueFormat = "")
        {
            EntityName = entityName;
            FieldName = fieldName;
            Operator = @operator;
            Value = value;
            ValueFormat = valueFormat;
        }

        public Field2ValueWhere()
        {
        }

        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public WhereOperator Operator { get; set; } = WhereOperator.Equal;
        public string Value { get; set; }
        public string ValueFormat { get; set; }

        public KeyValuePair<string, object>? GetParameter()
        {
            var field = new DBField();
            return new KeyValuePair<string, object>(GetParameterName(), field.GetValue(Value, ValueFormat));

        }

        public string GetParameterName()
        {
            return $@"@{EntityName}_{Operator.ToString()}_{FieldName}";
        }

        public string GetValue()
        {
            if (ValueFormat.IsNullOrEmpty())
            {
                throw new NotImplementedException("无效的数据格式化类型");
            }
            return string.Format(ValueFormat, Value);
        }

        public string ToSQL()
        {
            //TODO 这里理论上需要知道数据库的知识
            var field = new DBField();
            return $"[{EntityName}].[{FieldName}] { Operator.ToSQL() } { GetParameterName() }";
        }
    }
}
