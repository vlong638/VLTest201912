using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateTaskRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string TaskName { set; get; }
        /// <summary>
        /// 复制队列Id
        /// </summary>
        public long? CopyTaskId { get; set; }
    }
}
