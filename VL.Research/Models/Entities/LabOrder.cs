using Dapper.Contrib.Extensions;
using System;

namespace VL.Research.Models
{
    //替换用 (\w+)\s+(\w+)\s+\d+\s+[-\d]+\s+[-\d]+\s+[-\d]+\s+([\w:\(\)]+).+
    //public $2 $1 {set;get;} //$3
    [Table(TableName)]
    public class LabOrder
    {
        public const string TableName = "O_LabOrder";

        public string orderid { set; get; } //检查id
        public string examname { set; get; } //检查名称
        public string examcode { set; get; } //检查编码
        public DateTime? ordertime { set; get; } //检查时间
        public string orderdocid { set; get; } //检查医生Id
        public string orderdocname { set; get; } //检查医生名称
    }
}
