using System;
using System.Collections.Generic;
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

        public string GetFormatValue()
        {
            if (ValueFormat.IsNullOrEmpty())
            {
                throw new NotImplementedException("无效的数据格式化类型");
            }
            return string.Format(ValueFormat, Value);
        }
    }
    /// <summary>
    /// 条件运算类型
    /// </summary>
    public enum WhereOperator
    {
        None = 0,
        Equal = 1,
        Like = 2,
        IsNotNull = 3,
        IsNull = 4,
        GreatThan = 5,
        LessThan = 6,
    }
}
