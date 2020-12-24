using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetBusinessEntityModel
    {
        /// <summary>
        /// 指标单元Id
        /// </summary>
        public long BusinessEntityId { set; get; }
        /// <summary>
        /// 指标单元名称
        /// </summary>
        public string BusinessEntityName { set; get; }
        /// <summary>
        /// 是否用户自定义
        /// </summary>
        public bool IsCustom { set; get; }
        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsSelected { set; get; }
    }
}
