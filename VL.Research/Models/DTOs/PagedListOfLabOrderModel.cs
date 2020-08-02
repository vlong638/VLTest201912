using System;

namespace VL.Research.Models
{
    public class PagedListOfLabOrderModel
    {
        public long Id { set; get; }

        public string orderid { set; get; } //检查id
        public string examname { set; get; } //检查名称
        public string examcode { set; get; } //检查编码
        public DateTime? ordertime { set; get; } //检查时间
        public string orderdocid { set; get; } //检查医生Id
        public string orderdocname { set; get; } //检查医生名称
    }
}