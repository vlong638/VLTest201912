using FrameworkTest.Business.SDMockCommit;
using System;

namespace FS.SyncManager.Models
{
    public class PagedListOfSyncOrderModel
    {
        public long Id { set; get; } //0
        public SourceType SourceType { set; get; } //来源类型
        public string SourceId { set; get; } //来源Id
        public DateTime SyncTime { set; get; } //同步时间
        public string ErrorMessage { set; get; } //错误信息
        public SyncStatus SyncStatus { set; get; } //是否出错
        public TargetType TargetType { set; get; } //目标类型

        public string PersonName { set; get; } 
        public string Idcard { set; get; }
    }
}