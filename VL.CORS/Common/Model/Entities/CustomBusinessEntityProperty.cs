﻿using Dapper.Contrib.Extensions;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class CustomBusinessEntityProperty
    {
        public const string TableName = "CustomBusinessEntityProperty";

        public long Id { set; get; }
        /// <summary>
        /// 业务对象Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        public long TemplateId { set; get; }
        public long TemplatePropertyId { set; get; }

        /// <summary>
        /// 来源对象名称: 表或临时表或视图
        /// </summary>
        public string EntityName { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 字段中文名称
        /// </summary>
        public string DisplayName { set; get; }
        /// <summary>
        /// 模板的指标字段类型
        /// </summary>
        public ColumnType ColumnType { set; get; }
        /// <summary>
        /// 模板的指标枚举类型
        /// </summary>
        public string EnumType { set; get; }
    }
}