using Autobots.Infrastracture.Common.ValuesSolution;
using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetBriefProjectModel : GetProjectModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public GetBriefProjectModel(GetProjectModel model)
        {
            model.MapTo(this);

            //this.ProjectId = model.ProjectId;
            //this.ProjectName = model.ProjectName;
            //this.AdminIds = model.AdminIds;
            //this.MemberIds = model.MemberIds;
            //this.CreatorId = model.CreatorId;
            //this.DepartmentId = model.DepartmentId;
            //this.ViewAuthorizeType = model.ViewAuthorizeType;
            //this.ProjectDescription = model.ProjectDescription;
            //this.CreatedAt = model.CreatedAt;
            //this.LastModifiedAt = model.LastModifiedAt;
            //this.LastModifiedBy = model.LastModifiedBy;
            //this.IsFavorite = model.IsFavorite;
        }

        /// <summary>
        /// 项目查看权限 名称
        /// </summary>
        public string ViewAuthorizeTypeName { set; get; }
        /// <summary>
        /// 创建者 名称
        /// </summary>
        public string CreateName { set; get; }
        /// <summary>
        /// 关联科室 名称
        /// </summary>
        public string DepartmentName { set; get; }
        /// <summary>
        /// 项目管理人员 名称
        /// </summary>
        public List<string> AdminNames { set; get; }
        /// <summary>
        /// 项目成员 名称
        /// </summary>
        public List<string> MemberNames { set; get; }
    }
}
