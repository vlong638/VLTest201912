using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetProjectOperateHistoryRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string OperateTimeStart { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string OperateTimeEnd { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long? OperatorId { set; get; }
    }
}
