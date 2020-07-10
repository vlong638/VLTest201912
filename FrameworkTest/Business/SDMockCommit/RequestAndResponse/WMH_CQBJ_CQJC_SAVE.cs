using FrameworkTest.Common.ValuesSolution;
using FrameworkTest.ConfigurableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public class WMH_CQBJ_CQJC_SAVE
    {
        public WMH_CQBJ_CQJC_SAVE()
        {
            this.D1 = "";
            this.D2 = "";
            this.D3 = "";
            this.D4 = "";
            this.D5 = "";
            this.D6 = "";
            this.D7 = "";
            this.D8 = "";
            this.D9 = "";
            this.D10 = "";
            this.D11 = "";
            this.D12 = "";
            this.D13 = "";
            this.D14 = "";
            this.D15 = "";
            this.D16 = "";
            this.D17 = "";
            this.D18 = "";
            this.D19 = "";
            this.D20 = "";
            this.D21 = "";
            this.D22 = "";
            this.D23 = "";
            this.D24 = "";
            this.D25 = "";
            this.D26 = "";
            this.D27 = "";
            this.D28 = "";
            this.D29 = "";
            this.D30 = "";
            this.D31 = "";
            this.D32 = "";
            this.D33 = "";
            this.D34 = "";
            this.D35 = "";
            this.D36 = "";
            this.D37 = "";
            this.D38 = "";
            this.D39 = "";
            this.D40 = "";
            this.D41 = "";
            this.D42 = "";
            this.D43 = "";
            this.D44 = "";
            this.D45 = "";
            this.D46 = "";
            this.D47 = "";
            this.D48 = "";
            this.D49 = "";
            this.D50 = "";
            this.D51 = "";
            this.D52 = "";
            this.D53 = "";
            this.D54 = "";
            this.D55 = "";
            this.D56 = "";
            this.D57 = "";
        }
        public WMH_CQBJ_CQJC_SAVE(WMH_CQBJ_CQJC_READ_Data data)
        {
            this.D1 = data.D1;
            this.D2 = data.D2;
            this.D3 = data.D3;
            this.D4 = data.D4;
            this.D5 = data.D5;
            this.D6 = data.D6;
            this.D7 = data.D7;
            this.D8 = data.D8;
            this.D9 = data.D9;
            this.D10 = data.D10;
            this.D11 = data.D11;
            this.D12 = data.D12;
            this.D13 = data.D13;
            this.D14 = data.D14;
            this.D15 = data.D15;
            this.D16 = data.D16;
            this.D17 = data.D17;
            this.D18 = data.D18;
            this.D19 = data.D19;
            this.D20 = data.D20;
            this.D21 = data.D21;
            this.D22 = data.D22;
            this.D23 = data.D23;
            this.D24 = data.D24;
            this.D25 = data.D25;
            this.D26 = data.D26;
            this.D27 = data.D27;
            this.D28 = data.D28;
            this.D29 = data.D29;
            this.D30 = data.D30;
            this.D31 = data.D31;
            this.D32 = data.D32;
            this.D33 = data.D33;
            this.D34 = data.D34;
            this.D35 = data.D35;
            this.D36 = data.D36;
            this.D37 = data.D37;
            this.D38 = data.D38?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";
            this.D39 = data.D39;
            //this.D40 = data.D40;
            this.D41 = data.D41;
            this.D42 = data.D42;
            this.D43 = data.D43;
            this.D44 = data.D44;
            this.D45 = "";
            this.D46 = data.D46;
            this.D47 = data.D47;
            this.D48 = data.D48;
            this.D49 = data.D49;
            this.D50 = data.D50;
            this.D51 = data.D51;
            this.D52 = data.D52;
            this.D53 = data.D53;
            this.D54 = data.D54;
            this.D55 = data.D55;
            this.D56 = data.D56;
            this.D57 = data.D57;
        }

        internal void Update(UserInfo userInfo, ProfessionalExaminationModel_SourceData sourceDataModel, WMH_WCQBJ_GWYCF_SCORE_SAVERequest highRisksToSave)
        {
            //对比数据
            //142328199610271518	李丽	2019-12-18	2020-10-08	0	12	NULL	NULL	2020-07-30	2	
            //[{"index":"0","heartrate":"66","position":"01","presentposition":"1","fetalmove":"1"},{"index":"2","heartrate":"88","position":"02","presentposition":"2","fetalmove":"2"}]	2020-07-02	1393	NULL
            //收缩压 D8
            this.D8 = sourceDataModel.SourceData.sbp ?? "";
            //舒张压 D9
            this.D9 = sourceDataModel.SourceData.dbp ?? "";
            //体重 D7
            this.D7 = sourceDataModel.SourceData.weight ?? "";

            //D42.宫缩,uterinecontraction
            this.D42 = VLConstraints.Get_UterineContraction_By_UterineContraction_Hele(sourceDataModel.SourceData.uterinecontraction);
            //D55.羊水,amnioticfluidcharacter
            this.D55 = VLConstraints.Get_AmnioticFluidCharacter_By_AmnioticFluidCharacter_Hele(sourceDataModel.SourceData.amnioticfluidcharacter);
            //D17.胎动,fetalmoves
            if (sourceDataModel.SourceData.feltalentities.Count() > 0)
                this.D17 = VLConstraints.Get_FetalMove_By_FetalMove_Hele(sourceDataModel.SourceData.feltalentities.First().fetalmove);

            //D5.主诉        chiefcomplaint
            this.D5 = sourceDataModel.SourceData.chiefcomplaint;
            //D6.现病史      presenthistory
            this.D6 = sourceDataModel.SourceData.presenthistory;
            //D10.宫高       heightfundusuterus
            //FS: 耻骨联合上 横指
            //HL: 文本框,直接传
            this.D10 = string.IsNullOrEmpty(sourceDataModel.SourceData.heightfundusuterus)? "未查" : sourceDataModel.SourceData.heightfundusuterus;
            //D11.腹围        abdomencircumference
            this.D11 = string.IsNullOrEmpty(sourceDataModel.SourceData.abdomencircumference) ? "未查" : sourceDataModel.SourceData.abdomencircumference;
            //D15.衔接        xianjie
            //FS: 文本=>已衔接,未衔接
            //HL: 未衔接,衔接,半衔接
            this.D15 = VLConstraints.GetLinkByLink_HELE(sourceDataModel.SourceData.xianjie);
            //D16.浮肿        edemastatus
            //FS: 1.无,2.+,3.++,4.+++
            //HL: -,+,++,+++,++++
            this.D16 = VLConstraints.GetEdemaStatus_By_EdemaStatus_Hele(sourceDataModel.SourceData.edemastatus);

            //高危因素
            //高危等级  highrisklevel
            //FS: 
            //HL: A,
            //高危因素  highriskreason
            this.D21 = highRisksToSave.GetCurrentHighRiskNames();
            this.D20 = this.D21 == "" ? "2" : "1";

            //D25.诊断    
            //          diagnosisinfo,主诊断说明
            //          maindiagnosis,主诊断
            //          secondarydiagnosis,次诊断
            //HL: 高危妊娠监督	妊娠合并中度贫血,健康查体,妊娠期阴道炎,缺铁性贫血,妊娠期糖尿病
            this.D25 = $"{sourceDataModel.SourceData.diagnosisinfo},{sourceDataModel.SourceData.maindiagnosis ?? ""},{sourceDataModel.SourceData.secondarydiagnosis ?? ""}";

            //D36.检查医生
            this.D36 = userInfo.UserName;
            //D37.机构Id
            this.D37 = userInfo.OrgId;

            this.D1 = DateTime.Now.ToString("yyyy-MM-dd");//检查日期
            this.D2 = VLConstraints.GetGestationalWeeksByPrenatalDate(sourceDataModel.SourceData.dateofprenatal?.ToDateTime(), DateTime.Now);//D2.孕周   
            this.D3 = sourceDataModel.SourceData.lastmenstrualperiod?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";//D3 矫正末次月经 = 孕妇档案.末次月经
            this.D4 = sourceDataModel.SourceData.dateofprenatal?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";//D4 矫正预产期 = 孕妇档案.预产期
            //D26,处理 = 处理意见
            this.D26 = sourceDataModel.SourceData.suggestion ?? "";
            ////D29,健康评估 = 其它评估 无需处理
            //this.D29 = sourceData.SourceData.generalcomment ?? "";
            //D39,下次预约时间 = 下次随访
            this.D39 = sourceDataModel.SourceData.followupappointment?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";
            //D43 子宫压痛 = 子宫
            //有,无
            this.D43 = VLConstraints.Get_Uterus_By_Uterus_HELE(sourceDataModel.SourceData.uterus);
            //D53 宫口开大 = 宫口
            this.D53 = VLConstraints.Get_PalaceMouth_By_PalaceMouth_HELE(sourceDataModel.SourceData.palacemouth);
            //-- //D44,预约目的
            //D54 胎膜破裂 = 破水
            //1 = 是  2 = 无  Hele: 1=破 2=无
            this.D54 = sourceDataModel.SourceData.brokenwater ?? "";
            //[{"index":"0","heartrate":"66","position":"01","presentposition":"1","fetalmove":"1"},{"index":"2","heartrate":"88","position":"02","presentposition":"2","fetalmove":"2"}
            //sourceData.SourceData.feltalentities
            //胎数 D56  1 = 单胎 2 = 双胎 3 = 三胎以上
            this.D56 = VLConstraints.Get_FetalAmountByAmount(sourceDataModel.SourceData.feltalentities.Count());
            //胎先露 D14
            //头先露,臀先露,肩先露
            if (sourceDataModel.SourceData.feltalentities.Count() > 0)
                this.D14 = VLConstraints.Get_PresentPosition_By_PresentPosition_Hele(sourceDataModel.SourceData.feltalentities.First().presentposition);
            //胎心
            //1胎 D13
            //2胎 D48
            //3胎 D52
            //胎方位
            //1胎 D12
            //2胎 D46
            //3胎 D51
            for (int i = 0; i < sourceDataModel.SourceData.feltalentities.Count(); i++)
            {
                var feltalentity = sourceDataModel.SourceData.feltalentities[i];
                if (i == 0)
                {
                    this.D13 = string.IsNullOrEmpty(feltalentity.heartrate) ? "未查" : feltalentity.heartrate;
                    this.D12 = VLConstraints.Get_FetalPosition_By_FetalPosition_Hele(feltalentity.position);
                }
                else if (i == 1)
                {
                    this.D48 = string.IsNullOrEmpty(feltalentity.heartrate) ? "未查" : feltalentity.heartrate;
                    this.D46 = VLConstraints.Get_FetalPosition_By_FetalPosition_Hele(feltalentity.position);
                }
                else if (i == 2)
                {
                    this.D52 = string.IsNullOrEmpty(feltalentity.heartrate) ? "未查" : feltalentity.heartrate;
                    this.D51 = VLConstraints.Get_FetalPosition_By_FetalPosition_Hele(feltalentity.position);
                }
            }
        }

        public string D1 { get; set; }//"2020-07-01"
        public string D2 { get; set; }//"12+1"
        public string D3 { get; set; }//"2020-04-07"
        public string D4 { get; set; }//"2021-01-12"
        public string D5 { get; set; }//"停经12+1周,"
        public string D6 { get; set; }//"患者平素月经规则，末次月经日期：2020-04-07"
        public string D8 { get; set; }//"88"
        public string D9 { get; set; }//"111"
        public string D49 { get; set; }//""
        public string D50 { get; set; }//""
        public string D7 { get; set; }//"55"
        public string D10 { get; set; }//"耻骨联合上 横指"
        public string D11 { get; set; }//"122"
        public string D56 { get; set; }//"1"
        public string D12 { get; set; }//""
        public string D46 { get; set; }//""
        public string D51 { get; set; }//""
        public string D14 { get; set; }//"臀先露"
        public string D15 { get; set; }//""
        public string D41 { get; set; }//"2"
        public string D42 { get; set; }//""
        public string D43 { get; set; }//"无"
        public string D53 { get; set; }//""
        public string D54 { get; set; }//"无"
        public string D55 { get; set; }//"1"
        public string D16 { get; set; }//"1"
        public string D17 { get; set; }//"无"
        public string D13 { get; set; }//"11"
        public string D48 { get; set; }//""
        public string D52 { get; set; }//""
        public string D20 { get; set; }//"2"
        public string D21 { get; set; }//""
        public string D40 { get; set; }//"2019-12-18"
        public string D22 { get; set; }//""
        public string D23 { get; set; }//""
        public string D24 { get; set; }//""
        public string D18 { get; set; }//""
        public string D19 { get; set; }//""
        public string D25 { get; set; }//"孕0产0,孕12+1周,"
        public string D26 { get; set; }//""
        public string D47 { get; set; }//""
        public string D27 { get; set; }//"个人卫生,心理咨询,营养咨询,避免致畸因素和疾病对胚胎的不良影响"
        public string D57 { get; set; }//""
        public string D28 { get; set; }//"1"
        public string D29 { get; set; }//""
        public string D30 { get; set; }//"1"
        public string D31 { get; set; }//"1"
        public string D32 { get; set; }//""
        public string D33 { get; set; }//""
        public string D34 { get; set; }//""
        public string D35 { get; set; }//""
        public string D36 { get; set; }//"赵卓姝"
        public string D37 { get; set; }//"45608491-9"
        public string D38 { get; set; }//"2020-07-01"
        public string D39 { get; set; }//"2020-07-29"
        public string D44 { get; set; }//""
        public string D45 { get; set; }//"1"
    }
}
