
using Dapper.Contrib.Extensions;
using System;

namespace FrameworkTest.Business.SDMockCommit
{
    [Table("[SyncForFS]")]
    public class SyncForFS
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
        PregnantInfo = 1,
    }
    public enum SyncStatus
    {
        HasErrorWhileCreating = 1,//创建时 出现异常
        Create = 2,//创建成功
        ExistWhileCreating=3,//首次创建时存在
        HasErrorWhileUpdating = 40,//创建时 出现异常
        Update = 41,//更新成功
    }
}