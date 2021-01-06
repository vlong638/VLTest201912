using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetDropdownsWithParentRequest
    {
        /// <summary>
        /// 获取键值的类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 父节点Id
        /// </summary>
        public string ParentId { set; get; }
    }
}
