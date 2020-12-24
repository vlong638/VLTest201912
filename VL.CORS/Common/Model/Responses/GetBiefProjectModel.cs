using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetBiefProjectModel
    {
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedAt { set; get; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime LastModifiedAt { set; get; }
        /// <summary>
        /// 创建者
        /// </summary>
        public long CreatorName { set; get; }
        /// <summary>
        /// 关联科室
        /// </summary>
        public long DepartmentId { set; get; }
        /// <summary>
        /// 项目管理人员
        /// </summary>
        public List<long> AdminIds { set; get; }
        /// <summary>
        /// 项目成员
        /// </summary>
        public List<long> MemberIds { set; get; }
        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProjectDescription { set; get; }
    }
}
