using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
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
}
