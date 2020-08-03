
using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel;

namespace FrameworkTest.Business.SDMockCommit
{
    [Table("[SyncForFS]")]
    public class SyncOrder
    {
        public long Id { set; get; }
        public SourceType SourceType
        {
            get
            {
                switch (TargetType)
                {
                    case TargetType.PregnantInfo:
                    case TargetType.HistoryEnquiry:
                        return SourceType.PregnantInfo;
                    case TargetType.PhysicalExamination:
                    case TargetType.ProfessionalExamination:
                        return SourceType.MHC_VisitRecord;
                    case TargetType.ChildDischarge:
                    case TargetType.PregnantDischarge:
                        return SourceType.V_FWPT_GY_ZHUYUANFM;
                    default:
                        return SourceType.None;
                }
            }
        }
        public string SourceId { set; get; }
        public TargetType TargetType { set; get; }
        public DateTime SyncTime { set; get; }
        public SyncStatus SyncStatus { set; get; }
        public string ErrorMessage { set; get; }
    }
    public enum SourceType
    {
        [Description("")]
        None = 0,
        /// <summary>
        /// 孕妇档案
        /// </summary>
        [Description("孕妇档案")]
        PregnantInfo = 1,
        /// <summary>
        /// 检查
        /// </summary>
        [Description("孕妇产前检查")]
        MHC_VisitRecord = 2,
        /// <summary>
        /// 出院管理
        /// </summary>
        [Description("出院管理-分娩记录")]
        V_FWPT_GY_ZHUYUANFM = 3,
    }
    public enum TargetType
    {
        [Description("")]
        None = 0,
        /// <summary>
        /// 孕妇档案
        /// </summary>
        [Description("基本数据")]
        PregnantInfo = 1,
        /// <summary>
        /// 问询病史
        /// </summary>
        [Description("问询病史")]
        HistoryEnquiry = 2,
        /// <summary>
        /// 体格检查
        /// </summary>
        [Description("体格检查")]
        PhysicalExamination = 3,
        /// <summary>
        /// 专科检查
        /// </summary>
        [Description("专科检查")]
        ProfessionalExamination = 4,
        /// <summary>
        /// 孕妇出院登记
        /// </summary>
        [Description("孕妇出院登记")]
        PregnantDischarge = 5,
        /// <summary>
        /// 婴儿出院登记
        /// </summary>
        [Description("婴儿出院登记")]
        ChildDischarge = 6,
    }
    public enum SyncStatus
    {
        [Description("")]
        None = 0,
        [Description("执行出错")]
        Error = 1,//创建时 出现异常
        [Description("数据重复")]
        Existed = 11,//已存在
        [Description("待更新主体不存在")]
        NotExisted = 12,//未存在
        [Description("未获得有效Id")]
        Repeated = 13,//查重出错
        [Description("预产期冲突")]
        Conflict = 14,//冲突,对方存在不同预产期的数据
        [Description("诊断`死亡`")]
        DeadDiagnosis = 15,
        [Description("成功")]
        Success = 2,//处理成功
        AllError = 99,//更新成功 仅作测试使用
    }
}