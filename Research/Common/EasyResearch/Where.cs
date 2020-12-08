using System;
using System.Collections.Generic;
using System.Linq;
using VL.Consolo_Core.Common.ValuesSolution;

namespace Research.Common
{
    /// <summary>
    /// 条件集
    /// </summary>
    public class WhereSet
    {
        public List<IWhere> Wheres { get; set; } = new List<IWhere>();
        public List<WhereLinker> WhereLinkers { get; set; } = new List<WhereLinker>();
    }
    /// <summary>
    /// 条件连接类型
    /// </summary>
    public enum WhereLinker
    {
        None = 0,
        And = 1,
        Or = 2,
    }
    /// <summary>
    /// 筛选条件接口
    /// </summary>
    public interface IWhere
    {
        string ToSQL(Dictionary<string, string> tableAlias);
        KeyValuePair<string, object>? GetParameter();
    }
    /// <summary>
    /// 表对表 条件项
    /// </summary>
    public class Field2FieldWhere : IWhere
    {
        public Field2FieldWhere(string entityName, string fieldName, WhereOperator @operator, string entityName2Compare, string fieldName2Compare)
        {
            EntityName = entityName;
            FieldName = fieldName;
            Operator = @operator;
            EntityName2Compare = entityName2Compare;
            FieldName2Compare = fieldName2Compare;
        }

        public Field2FieldWhere(string entityName, string fieldName, WhereOperator @operator, string entityName2Compare, string fieldName2Compare, string fieldFormat = null, string fieldName2CompareFormat = null)
        {
            EntityName = entityName;
            FieldName = fieldName;
            FieldFormat = fieldFormat;
            Operator = @operator;
            EntityName2Compare = entityName2Compare;
            FieldName2Compare = fieldName2Compare;
            FieldName2CompareFormat = fieldName2CompareFormat;
        }

        public string EntityName { get; set; }
        public string FieldName { get; set; }
        public string FieldFormat { get; set; }
        public WhereOperator Operator { get; set; }
        public string EntityName2Compare { get; set; }
        public string FieldName2Compare { get; set; }
        public string FieldName2CompareFormat { get; set; }

        public KeyValuePair<string, object>? GetParameter()
        {
            return null;
        }

        public string ToSQL(Dictionary<string, string> tableAlias)
        {
            throw new NotImplementedException();
        }
    }
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
            return new KeyValuePair<string, object>(GetParameterName(""), field.GetValue(Value, ValueFormat));

        }

        private string GetParameterName(string tableAlias)
        {
            return "@" + tableAlias + "_" + FieldName;
        }

        public string GetValue()
        {
            if (ValueFormat.IsNullOrEmpty())
            {
                throw new NotImplementedException("无效的数据格式化类型");
            }
            return string.Format(ValueFormat, Value);
        }

        public string ToSQL(Dictionary<string, string> tableAliass)
        {
            //TODO 这里理论上需要知道数据库的知识
            var field = new DBField();
            var tableAlias = tableAliass[EntityName];
            return $"[{tableAlias}].{FieldName} { Operator.ToSQL() } { GetParameterName(tableAlias) }";
        }
    }

    public class DBField
    {
        public string ColumnType { set; get; } = "string";

    }

    public class ValueEntity
    {
        public string ValueName { set; get; }
        public string ValueFormat { set; get; }
        public string ValueType { set; get; }
    }

    /// <summary>
    /// 条件运算类型
    /// </summary>
    public enum WhereOperator
    {
        None = 0,
        eq = 1,
        Like = 2,
        IsNotNull = 3,
        IsNull = 4,
        gt = 5,
        lt = 6,
        GreatOrEqualThan = 7,
        LessOrEqualThan = 8,
    }

    public static class IWhereEx
    {
        public static string GetValue(this DBField field,string value,string valueFormat)
        {
            var tempValue = value;
            switch (field.ColumnType)
            {
                case "string":
                    tempValue = value;
                    break;
                default:
                    throw new NotImplementedException("未支持的字段类型");
            }
            if (valueFormat.IsNullOrEmpty())
            {
                return tempValue;
            }
            return string.Format(valueFormat, tempValue);
        }

        public static string ToSQL(this RouteType routeType)
        {
            switch (routeType)
            {
                case RouteType.LeftJoin:
                    return "left join";
                case RouteType.InnerJoin:
                    return "inner join";
                default:
                    throw new NotImplementedException("未支持的连接类型");
            }
        }

        public static string ToSQL(this WhereOperator @operator)
        {
            switch (@operator)
            {
                case WhereOperator.eq:
                    return " = ";
                case WhereOperator.Like:
                    return " like ";
                case WhereOperator.IsNotNull:
                    return " is not null ";
                case WhereOperator.IsNull:
                    return " is null ";
                case WhereOperator.gt:
                    return " > ";
                case WhereOperator.lt:
                    return " < ";
                case WhereOperator.GreatOrEqualThan:
                    return " >= ";
                case WhereOperator.LessOrEqualThan:
                    return " <= ";
                default:
                    return "";
            }
        }
    }
}
