using Dapper.Contrib.Extensions;
using System;

namespace FS.SyncTask
{
    [Table("[SyncForFS]")]
    public class SyncForFS
    {
        public long Id { set; get; }
        public TargetType TargetType { set; get; }
        public string SourceId { set; get; }
        public DateTime SyncTime { set; get; }
        public string ErrorMessage { set; get; }
        public bool HasError { set; get; }
    }
    public enum TargetType
    {
        PregnantInfo = 1,
    }
}
