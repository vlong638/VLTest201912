using System;

namespace VL.Research.Models
{
    public class PagedListOfPregnantInfoModel
    {
        public long id { set; get; }
        //public long InstitutionCode { set; get; } //机构编码
        //public long CreatorId { set; get; } //创建者
        public string patientid { set; get; }
        public string idcard { set; get; } //身份证号


        public string personname { set; get; } //孕妇姓名
        public int? sexcode { set; get; } //性别
        public string contactphone { set; get; } //联系人电话
        public DateTime? birthday { set; get; } //出生日期 
        public int? gravidity { set; get; } //孕次
        public int? parity { set; get; } //产次
        public int? iscreatebook { set; get; } //是否建册
        public int? GestationalWeeks { set; get; } //建册孕周
        public DateTime? lastmenstrualperiod { set; get; } //末次月经
        public DateTime? dateofprenatal { set; get; } //预产期
        public int? filestatus { set; get; } //档案状态:(结案标识)
    }
}