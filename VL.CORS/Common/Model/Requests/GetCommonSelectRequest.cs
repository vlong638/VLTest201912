using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 统一查询入参结构
    /// </summary>
    public class GetCommonSelectRequest
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { set; get; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int limit { set; get; }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public List<VLKeyValue> search { set; get; }
    }
}
