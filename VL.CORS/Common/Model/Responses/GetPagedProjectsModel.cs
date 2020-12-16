using System;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetPagedProjectsModel
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
        /// 关联科室
        /// </summary>
        public long DepartmentId { set; get; }
        /// <summary>
        /// 创建者
        /// </summary>
        public long CreatorId { set; get; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedAt { set; get; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime LastModifiedAt { set; get; }
    }
}
