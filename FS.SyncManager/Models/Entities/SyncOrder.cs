using FrameworkTest.Business.SDMockCommit;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.SyncManager.Models
{
    //替换用 
    //(\w+)\s+(\w+)\s+\d+\s+[-\d]+\s+[-\d]+\s+[-\d]+\s+([\w:\(\)]+).+
    //public $2 $1 {set;get;} //$3
    [Table(TableName)]
    public class SyncOrder
    {
        public const string TableName = "SyncForFS";

        public long Id { set; get; } //0
        public SourceType SourceType { set; get; } //来源类型
        public string SourceId { set; get; } //来源Id
        public DateTime SyncTime { set; get; } //同步时间
        public string ErrorMessage { set; get; } //错误信息
        public SyncStatus SyncStatus { set; get; } //是否出错
        public TargetType TargetType { set; get; } //目标类型
    }
}