using Dapper.Contrib.Extensions;
using System;

namespace ResearchAPI.CORS.Common
{
    [Table("SyncManage")]
    public class SyncManage
    {
        public SyncManage()
        {
        }

        public SyncManage(string from, string to, DateTime issueTime, DateTime? latestDataTime, string latestDataField, string message, OperateType operateType, OperateStatus operateStatus)
        {
            From = from;
            To = to;
            IssueTime = issueTime;
            LatestDataTime = latestDataTime;
            LatestDataField = latestDataField;
            Message = message;
            OperateType = operateType;
            OperateStatus = operateStatus;
        }

        [Key]
        public long Id { set; get; }
        public string From { set; get; }
        public string To { set; get; }
        public OperateType OperateType { set; get; }
        public OperateStatus OperateStatus { set; get; }
        public DateTime IssueTime { set; get; }
        public DateTime? LatestDataTime { set; get; }
        public string LatestDataField { set; get; }
        public string Message { set; get; }
    }

    public enum OperateStatus
    {
        None = 0,
        Success = 1,
        Error = 2,
    }

    public enum OperateType
    {
        None = 0,
        /// <summary>
        /// 表结构初始化
        /// </summary>
        InitTable = 1,
        /// <summary>
        /// 数据初始化
        /// </summary>
        InitData = 2,
        /// <summary>
        /// 数据转换
        /// </summary>
        DataTransform = 3,
        /// <summary>
        /// 枚举转换
        /// </summary>
        EnumTransform = 4,
    }
}
