namespace ResearchAPI.Common
{
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
        GreatOrEqualThan = 7,
        LessOrEqualThan = 8,
    }
}
