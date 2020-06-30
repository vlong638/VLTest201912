
using Dapper.Contrib.Extensions;
using System;

namespace FrameworkTest.Business.SDMockCommit
{
    [Table("[SyncForFS]")]
    public class SyncOrder
    {
        public long Id { set; get; }
        public SourceType SourceType { set; get; }
        public string SourceId { set; get; }
        public DateTime SyncTime { set; get; }
        public SyncStatus SyncStatus { set; get; }
        public string ErrorMessage { set; get; }
    }
    public enum SourceType
    {
        /// <summary>
        /// 孕妇档案
        /// </summary>
        PregnantInfo = 1,
        /// <summary>
        /// 问询病史
        /// </summary>
        HistoryEnquiry = 2,
        /// <summary>
        /// 体格检查
        /// </summary>
        PhysicalExamination = 3,
        /// <summary>
        /// 专科检查
        /// </summary>
        ProfessionalExamination =4,
    }
    public enum SyncStatus
    {
        Error = 1,//创建时 出现异常
        Existed = 11,//已存在
        NotExisted = 12,//未存在
        Success = 2,//处理成功
        Test = 99,//更新成功 仅作测试使用
    }
}