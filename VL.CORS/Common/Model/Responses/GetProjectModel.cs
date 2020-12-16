using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetProjectModel
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
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
        /// 创建人
        /// </summary>
        public long? CreatorId { set; get; }
        /// <summary>
        /// 关联科室
        /// </summary>
        public long? DepartmentId { set; get; }
        /// <summary>
        /// 项目查看权限
        /// </summary>
        public ViewAuthorizeType ViewAuthorizeType { set; get; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProjectDescription { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedAt { set; get; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime? LastModifiedAt { set; get; }
        /// <summary>
        /// 最近修改人员
        /// </summary>
        public long? LastModifiedBy { get; set; }
    }
}
