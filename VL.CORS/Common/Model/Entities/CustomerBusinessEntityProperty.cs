﻿using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class CustomerBusinessEntityProperty
    {
        public const string TableName = "CustomerBusinessEntityProperty";

        public long Id { set; get; }
        /// <summary>
        /// 业务对象Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 来源对象名称: 表或临时表或视图
        /// </summary>
        public string SourceName { set; get; }
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