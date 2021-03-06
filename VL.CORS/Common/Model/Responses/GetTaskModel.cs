﻿using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetTaskModel
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 队列Id
        /// </summary>
        public long TaskId { set; get; }
        /// <summary>
        /// 队列名称
        /// </summary>
        public string TaskName { set; get; }
        /// <summary>
        /// 队列条件
        /// </summary>
        public List<GetTaskWhereModel> Wheres { set; get; }

        /// <summary>
        /// 执行状态
        /// </summary>
        public ScheduleStatus ScheduleStatus { set; get; }
        /// <summary>
        /// 执行状态_文本
        /// </summary>
        public string ScheduleStatusName { set; get; }
        /// <summary>
        /// 可导出文件
        /// </summary>
        public string ResultFile { set; get; }
        /// <summary>
        /// 上一次执行完成时间
        /// </summary>
        public DateTime? LastCompletedAt { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class GetTaskWhereModel : ProjectTaskWhere
    {
        /// <summary>
        /// 操作类型的显示名称
        /// </summary>
        public string OperatorName { set; get; }
        /// <summary>
        /// 属性的显示名称
        /// </summary>
        public string DisplayName { set; get; }
    }
}
