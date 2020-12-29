using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table(TableName)]
    public class ProjectLog
    {
        public const string TableName = "ProjectLog";

        public long Id { set; get; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 操作人
        /// </summary>
        public long OperatorId { set; get; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreatedAt { set; get; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionType ActionType { set; get; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Text { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 修改项目名称
        /// </summary>
        EditProjectName =101,
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
        AddProjectDepartment = 106,
        /// <summary>
        /// 设置关联科室
        /// </summary>
        DeleteProjectDepartment = 107,
        /// <summary>
        /// 设置项目查看权限
        /// </summary>
        SetProjectViewAtuhorityType = 108,
        /// <summary>
        /// 添加科研指标
        /// </summary>
        AddProjectIndicator = 111,
        /// <summary>
        /// 删除科研指标
        /// </summary>
        DeleteProjectIndicator = 112,
        /// <summary>
        /// 添加科研队列
        /// </summary>
        AddTask = 113,
        /// <summary>
        /// 删除科研队列
        /// </summary>
        DeleteTask = 114,
        /// <summary>
        /// 执行科研队列
        /// </summary>
        StartTask = 115,
        /// <summary>
        /// 下载科研队列
        /// </summary>
        DownloadTaskResult = 119,
        /// <summary>
        /// {用户}在科研队列{队列名称}中添加了条件{指标名称}
        /// </summary>
        AddTaskWhere = 121,
        /// <summary>
        /// {用户}在科研队列{队列名称}中删除了条件{指标名称}
        /// </summary>
        DeleteTaskWhere = 122,
        /// <summary>
        /// {用户}在科研队列{队列名称}中修改了条件{指标名称}
        /// </summary>
        EditTaskWhere = 123,
    }
}
