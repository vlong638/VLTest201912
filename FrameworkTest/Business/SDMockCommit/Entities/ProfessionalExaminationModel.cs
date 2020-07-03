using FrameworkTest.Common.ValuesSolution;
using System.Collections.Generic;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ProfessionalExaminationModel
    {
        public string id { set; get; }

        #region PregnantInfo
        public string personname { set; get; }//姓名
        public string idcard { set; get; }
        public string lastmenstrualperiod { set; get; }//末次月经
        public string dateofprenatal { set; get; }//预产期 
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

        public List<feltalentity> feltalentities { get { return multifetal?.FromJson<List<feltalentity>>() ?? new List<feltalentity>(); } }
    }

    public class feltalentity
    {
        public string index { set; get; }
        public string heartrate { set; get; }
        public string position { set; get; }
        public string presentposition { set; get; }
        public string fetalmove { set; get; }
    }
}
