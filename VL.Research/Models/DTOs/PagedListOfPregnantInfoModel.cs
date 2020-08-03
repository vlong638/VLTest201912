using System;

namespace VL.Research.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PagedListOfPregnantInfoModel
    {
        /// <summary>
        /// 标识符
        /// </summary>
        public long id { set; get; }
        //public long InstitutionCode { set; get; } //机构编码
        //public long CreatorId { set; get; } //创建者
        /// <summary>
        /// 病人Id
        /// </summary>
        public string patientid { set; get; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string idcard { set; get; } //身份证号
        /// <summary>
        /// 孕妇姓名
        /// </summary>
        public string personname { set; get; } //孕妇姓名
        /// <summary>
        /// 性别
        /// </summary>
        public int? sexcode { set; get; } //性别
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string contactphone { set; get; } //联系人电话
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? birthday { set; get; } //出生日期 
        /// <summary>
        /// 孕次
        /// </summary>
        public int? gravidity { set; get; } //孕次
        /// <summary>
        /// 产次
        /// </summary>
        public int? parity { set; get; } //产次
        /// <summary>
        /// 是否建册
        /// </summary>
        public int? iscreatebook { set; get; } //是否建册
        /// <summary>
        /// 建册孕周
        /// </summary>
        public int? GestationalWeeks { set; get; } //建册孕周
        /// <summary>
        /// 末次月经
        /// </summary>
        public DateTime? lastmenstrualperiod { set; get; } //末次月经
        /// <summary>
        /// 预产期
        /// </summary>
        public DateTime? dateofprenatal { set; get; } //预产期
        /// <summary>
        /// 档案状态:(结案标识)
        /// </summary>
        public int? filestatus { set; get; } //档案状态:(结案标识)
    }
}