using System.Collections.Generic;

namespace Research.Common
{
    /// <summary>
    /// 分组操作类型
    /// </summary>
    public enum GroupSelectOperator
    {
        None = 0,
        Max = 1,
        Min = 2,
    }
    /// <summary>
    /// 分组
    /// </summary>
    public class GroupBySet
    {
        public List<GroupBy> GroupBys { set; get; }
        public List<GroupSelect> GroupSelects { set; get; }
    }
    /// <summary>
    /// 分组根据字段
    /// </summary>
    public class GroupBy
    {
        public GroupBy(string source, string fieldName)
        {
            Source = source;
            FieldName = fieldName;
        }

        public string Source { set; get; }
        public string FieldName { set; get; }
    }
    /// <summary>
    /// 分组选择字段
    /// </summary>
    public class GroupSelect
    {
        public GroupSelect(string source, string fieldName)
        {
            Source = source;
            FieldName = fieldName;
        }

        public GroupSelect(string source, string fieldName, GroupSelectOperator groupSelectOperator) : this(source, fieldName)
        {
            GroupSelectOperator = groupSelectOperator;
        }

        public string Source { set; get; }
        public string FieldName { set; get; }
        public GroupSelectOperator GroupSelectOperator { get; }
    }
}
