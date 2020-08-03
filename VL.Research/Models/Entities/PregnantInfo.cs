using Dapper.Contrib.Extensions;
using System;

namespace VL.Research.Models
{
    //替换用 (\w+)\s+(\w+)\s+\d+\s+[-\d]+\s+[-\d]+\s+[-\d]+\s+([\w:\(\)]+).+
    //public $2 $1 {set;get;} //$3
    [Table(TableName)]
    public class PregnantInfo
    {
        public const string TableName = "O_PregnantInfo";

        /// <summary>
        /// 标识符
        /// </summary>
        public long Id { set; get; }
        /// <summary>
        /// 机构编码
        /// </summary>
        public long InstitutionCode { set; get; } //机构编码
        /// <summary>
        /// 创建者
        /// </summary>
        public long CreatorId { set; get; } //创建者
        /// <summary>
        /// 病人Id
        /// </summary>
        public string PatientId { set; get; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdCard { set; get; } //身份证号
        /// <summary>
        /// 孕妇姓名
        /// </summary>
        public string PersonName { set; get; } //孕妇姓名
        /// <summary>
        /// 性别
        /// </summary>
        public int? SexCode { set; get; } //性别
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { set; get; } //联系人电话
        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? Birthday { set; get; } //出生日期 
        /// <summary>
        /// 孕次
        /// </summary>
        public int? Gravidity { set; get; } //孕次
        /// <summary>
        /// 产次
        /// </summary>
        public int? Parity { set; get; } //产次
        /// <summary>
        /// 是否建册
        /// </summary>
        public int? IsCreateBook { set; get; } //是否建册
        /// <summary>
        /// 建册孕周
        /// </summary>
        public int? GestationalWeeks { set; get; } //建册孕周
        /// <summary>
        /// 末次月经
        /// </summary>
        public DateTime? LastMenstrualPeriod { set; get; } //末次月经
        /// <summary>
        /// 预产期
        /// </summary>
        public DateTime? DateOfPrenatal { set; get; } //预产期
        /// <summary>
        /// 档案状态:(结案标识)
        /// </summary>
        public int? FileStatus { set; get; } //档案状态:(结案标识)
    }
}
