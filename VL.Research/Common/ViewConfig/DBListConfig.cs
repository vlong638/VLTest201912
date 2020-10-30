namespace BBee.Common
{
    public class DBListConfig
    {
        public long Id { set; get; }
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { set; get; }
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { set; get; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { set; get; }
        /// <summary>
        /// 占用字节数
        /// </summary>
        public int MaxLength { set; get; }
        /// <summary>
        /// 数字长度
        /// </summary>
        public int Precision { set; get; }
        /// <summary>
        /// 小数位数
        /// </summary>
        public int Scale { set; get; }
        /// <summary>
        /// 是否允许空
        /// </summary>
        public bool IsNullable { set; get; }
        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { set; get; }
        /// <summary>
        /// 是否是主键
        /// </summary>
        public bool IsPrimaryKey { set; get; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Description { set; get; }
    }
}
