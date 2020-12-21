namespace ResearchAPI.CORS.Common
{
    public class BusinessEntityProperty
    {
        public long Id { set; get; }
        /// <summary>
        /// 业务对象Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 字段中文名称
        /// </summary>
        public string DisplayName { set; get; }
    }
}