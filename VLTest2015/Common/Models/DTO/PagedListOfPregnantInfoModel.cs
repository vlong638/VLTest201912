using System;

namespace VLTest2015.Services
{
    public class PagedListOfPregnantInfoModel
    {
        public long Id { set; get; }
        /// <summary>
        /// 病人编码
        /// </summary>
        public string PatientCode { set; get; }

        public string Name { set; get; } //孕妇姓名
        public int? Sex { set; get; } //性别
        public string IdCard { set; get; } //身份证号
        public string ContactPhone { set; get; } //联系人电话
        public DateTime? Birthday { set; get; } //出生日期
        public int? Gravidity { set; get; } //孕次
        public int? Parity { set; get; } //产次
        public int? IsCreateBook { set; get; } //是否建册
        public int? GestationalWeeks { set; get; } //建册孕周
        public DateTime? LastMenstrualPeriod { set; get; } //末次月经
        public DateTime? DateOfPrenatal { set; get; } //预产期
        public int? FileStatus { set; get; } //档案状态:(结案标识)
    }
}