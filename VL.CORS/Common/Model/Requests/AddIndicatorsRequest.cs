﻿using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    public class AddIndicatorsRequest
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 业务对象Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 业务对象字段Id
        /// </summary>
        public List<long> BusinessEntityPropertyIds { set; get; }

        ///// <summary>
        ///// 业务对象字段
        ///// </summary>
        //public List<AddIndicatorPropertyModel> Properties { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AddIndicatorPropertyModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long BusinessEntityPropertyId { set; get; }
        /// <summary>
        /// 表名
        /// </summary>
        public string EntityName { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string PropertyName { set; get; }
        /// <summary>
        /// 显示用名称
        /// </summary>
        public string DisplayName { set; get; }
    }
}
