using System;

namespace VL.Research.Models
{
    public class PagedListOfLabOrderModel
    {
        /// <summary>
        /// 标识符
        /// </summary>
        public long Id { set; get; }

        /// <summary>
        /// 检查id
        /// </summary>
        public string orderid { set; get; } //检查id
        /// <summary>
        /// 检查名称
        /// </summary>
        public string examname { set; get; } //检查名称
        /// <summary>
        /// 检查编码
        /// </summary>
        public string examcode { set; get; } //检查编码
        /// <summary>
        /// 检查时间
        /// </summary>
        public DateTime? ordertime { set; get; } //检查时间
        /// <summary>
        /// 检查医生Id
        /// </summary>
        public string orderdocid { set; get; } //检查医生Id
        /// <summary>
        /// 检查医生名称
        /// </summary>
        public string orderdocname { set; get; } //检查医生名称
    }
}