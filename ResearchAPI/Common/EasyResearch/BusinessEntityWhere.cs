namespace ResearchAPI.Common
{
    public class BusinessEntityWhere
    {
        public string ComponentName { set; get; }
        public WhereOperator Operator { set; get; }
        public string Value { set; get; }
    }
}
