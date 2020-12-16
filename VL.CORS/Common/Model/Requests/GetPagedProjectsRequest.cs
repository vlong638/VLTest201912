using Autobots.Infrastracture.Common.PagerSolution;
using System;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetPagedProjectsRequest : VLPagerRequest
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { set; get; }
        /// <summary>
        /// 科室Id
        /// </summary>
        public long DepartmentId { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { set; get; }
    }
}
