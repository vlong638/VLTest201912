using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ProfessionalExaminationModel
    {
        /// <summary>
        /// 舒张压
        /// </summary>
        public string dbp { set; get; }
        /// <summary>
        /// 收缩压
        /// </summary>
        public string sbp { set; get; }
        /// <summary>
        /// 主诉
        /// </summary>
        public string chiefcomplaint;
        /// <summary>
        /// 现病史
        /// </summary>
        public string presenthistory;
        /// <summary>
        /// 宫高
        /// </summary>
        public string heightfundusuterus;
        /// <summary>
        /// 腹围
        /// </summary>
        public string abdomencircumference;
        /// <summary>
        /// 衔接
        /// </summary>
        public string xianjie;
        /// <summary>
        /// 浮肿
        /// </summary>
        public string edemastatus;
        /// <summary>
        /// 主诊断说明
        /// </summary>
        public string diagnosisinfo;
        /// <summary>
        /// 主诊断
        /// </summary>
        public string maindiagnosis;
        /// <summary>
        /// 次诊断
        /// </summary>
        public string secondarydiagnosis;

        /// <summary>
        /// 高危
        /// 样例: [{"l":"B","r":"b170101","t":"三胎及以上妊娠"},{"l":"A","r":"a170101","t":"双胎妊娠"},{"l":"A","r":"a160102","t":"瘢痕子宫"}]
        /// </summary>
        public string highriskdic;

        /// <summary>
        /// 宫缩
        /// </summary>
        public string uterinecontraction;
        /// <summary>
        /// 羊水
        /// </summary>
        public string amnioticfluidcharacter;
        /// <summary>
        /// 胎动
        /// </summary>
        public string fetalmoves;

        public string id { set; get; }

        #region PregnantInfo
        public string personname { set; get; }//姓名
        public string idcard { set; get; }
        public string lastmenstrualperiod { set; get; }//末次月经
        public string dateofprenatal { set; get; }//预产期 
        public string BMI { set; get; }//BMI指数
        /// <summary>
        /// 体重
        /// </summary>
        public string weight { set; get; }
        #endregion

        //,vr.uterus--子宫 1=异常 0=正常
        public string uterus { set; get; }
        //,vr.palacemouth --宫口 详见枚举
        public string palacemouth { set; get; }
        //, vr.suggestion --处理意见
        public string suggestion { set; get; }
        //,vr.generalcomment --其他评估
        public string generalcomment { set; get; }
        //,vr.followupappointment --下次随访
        public string followupappointment { set; get; }
        //--预约目的
        //,vr.brokenwater --破水
        public string brokenwater { set; get; }
        //,vr.multifetal --多胎
        //--胎数
        //--胎方位
        //--胎先露
        public string multifetal { set; get; }

        public List<FeltalEntity> feltalentities { get { return multifetal?.FromJson<List<FeltalEntity>>() ?? new List<FeltalEntity>(); } }
    }

    public class FeltalEntity
    {
        public string index { set; get; }
        public string heartrate { set; get; }
        public string position { set; get; }
        public string presentposition { set; get; }
        public string fetalmove { set; get; }
    }

    //[{ "l":"B","r":"b170101","t":"三胎及以上妊娠"},{ "l":"A","r":"a170101","t":"双胎妊娠"},{ "l":"A","r":"a160102","t":"瘢痕子宫"}]
    public class HighRiskEntity
    {
        /// <summary>
        /// 高危等级
        /// </summary>
        public string L { set; get; }
        /// <summary>
        /// 高危编号
        /// </summary>
        public string R { set; get; }
        /// <summary>
        /// 高危文本
        /// </summary>
        public string T { set; get; }
    }
}
