using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FS.SyncManager.Models
{
    //替换用 (\w+)\s+(\w+)\s+\d+\s+[-\d]+\s+[-\d]+\s+[-\d]+\s+([\w:\(\)]+).+
    //public $2 $1 {set;get;} //$3
    [Table(TableName)]
    public class ChildDischarge
    {
        public const string TableName = "V_FWPT_GY_ZHUYUANFMYE";

        //public long Id { set; get; }
        //public long InstitutionCode { set; get; } //机构编码
        //public long CreatorId { set; get; } //创建者
        //public string PatientId { set; get; }
        //public string IdCard { set; get; } //身份证号

        //public string PersonName { set; get; } //孕妇姓名
        //public int? SexCode { set; get; } //性别
        //public string ContactPhone { set; get; } //联系人电话
        //public DateTime? Birthday { set; get; } //出生日期 
        //public int? Gravidity { set; get; } //孕次
        //public int? Parity { set; get; } //产次
        //public int? IsCreateBook { set; get; } //是否建册
        //public int? GestationalWeeks { set; get; } //建册孕周
        //public DateTime? LastMenstrualPeriod { set; get; } //末次月经
        //public DateTime? DateOfPrenatal { set; get; } //预产期
        //public int? FileStatus { set; get; } //档案状态:(结案标识)
    }
}
