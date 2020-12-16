using Autobots.Infrastracture.Common.PagerSolution;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class ProjectDTO
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { set; get; }
        /// <summary>
        /// 项目管理人员
        /// </summary>
        public List<long> AdminIds { set; get; }
        /// <summary>
        /// 项目成员
        /// </summary>
        public List<long> MemberIds { set; get; }
        /// <summary>
        /// 关联科室
        /// </summary>
        public long DepartmentId { set; get; }
        /// <summary>
        /// 项目查看权限
        /// </summary>
        public int ViewAuthorizeType { set; get; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProjectDescription { set; get; }
    }
}
