using System;

namespace BBee.Models
{
    public class PagedListOfVisitRecordModel
    {
        /// <summary>
        /// 标识符
        /// </summary>
        public long Id { set; get; } //自动编号
        //public long InstitutionCode { set; get; } //机构编码
        //public long CreatorId { set; get; } //创建者
        /// <summary>
        /// 病人编码
        /// </summary>
        public long PregnantInfoId { set; get; } //病人编码		
        /// <summary>
        /// 检查时间
        /// </summary>
        public string VisitDate { set; get; } //检查时间
        /// <summary>
        /// 舒张压(mmhg)
        /// </summary>
        public int? DBP { set; get; } //舒张压(mmhg)
        /// <summary>
        /// 收缩压(mmhg)
        /// </summary>
        public int? SBP { set; get; } //收缩压(mmhg)
        /// <summary>
        /// 体重(kg)
        /// </summary>
        public decimal? Weight { set; get; } //体重(kg)
        /// <summary>
        /// 宫高(cm)
        /// </summary>
        public decimal? HeightFundusuterus { set; get; } //宫高(cm)
        /// <summary>
        /// 腹围(cm)
        /// </summary>
        public decimal? AbdomenCircumference { set; get; } //腹围(cm)
        /// <summary>
        /// 胎心率(次)
        /// </summary>
        public int? FetalHeartRate { set; get; } //胎心率(次)
        /// <summary>
        /// 浮肿
        /// </summary>
        public string EdemaStatus { set; get; } //浮肿
    }
}