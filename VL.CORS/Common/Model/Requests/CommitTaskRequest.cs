using System;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CommitTaskRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public long TaskId { set; get; }
        /// <summary>
        /// 是否立即执行
        /// </summary>
        public bool IsStartNow { set; get; }
        /// <summary>
        /// 指定的执行时间
        /// </summary>
        public DateTime? StartAt { set; get; }
    }
}
