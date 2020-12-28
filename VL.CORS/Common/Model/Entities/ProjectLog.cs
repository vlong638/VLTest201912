using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectLog
    {
        public const string TableName = "ProjectLog";

        public long Id { set; get; }
        public long ProjectId { set; get; }
        public long OperatorId { set; get; }

        public DateTime CreatedAt { set; get; }
        public ActionType ActionType { set; get; }
    }

    public enum ActionType
    {
        None = 0,
        /// <summary>
        /// 修改项目名称
        /// </summary>
        EditProjectName =1,
        /// <summary>
        /// 创建项目
        /// </summary>
        CreateProject=  101,
        /// <summary>
        /// 添加项目管理员
        /// </summary>
        AddProjectManager = 102,
        /// <summary>
        /// 删除项目管理员
        /// </summary>
        DeleteProjectManager = 103,
        /// <summary>
        /// 添加项目成员
        /// </summary>
        AddProjectMember = 104,
        /// <summary>
        /// 删除项目成员
        /// </summary>
        DeleteProjectMember = 105,
        /// <summary>
        /// 设置关联科室
        /// </summary>
        SetProjectDepartment = 106,
        /// <summary>
        /// 设置项目查看权限
        /// </summary>
        SetProjectViewAtuhorityType = 107,





    }
}
