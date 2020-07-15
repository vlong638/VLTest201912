﻿using System;

namespace FS.SyncManager.Models
{
    public class PagedListOfVisitRecordModel
    {
        public long id { set; get; } //自动编号
        //public long InstitutionCode { set; get; } //机构编码
        //public long CreatorId { set; get; } //创建者
        //public long PregnantInfoId { set; get; } //病人编码		


        //快速替换方案
        //<Property.+ DisplayName="(\w+)" ColumnName="(\w+)" .+ />
        //=>
        //public string $2 {set;get;}//$1
        public long idcard { set; get; } //身份证		
        public string visitdate { set; get; }//检查日期


        public int? DBP { set; get; } //舒张压(mmhg)
        public int? SBP { set; get; } //收缩压(mmhg)
        public decimal? Weight { set; get; } //体重(kg)
        public decimal? HeightFundusuterus { set; get; } //宫高(cm)
        public decimal? AbdomenCircumference { set; get; } //腹围(cm)
        public int? FetalHeartRate { set; get; } //胎心率(次
        public string EdemaStatus { set; get; } //浮肿
    }
}