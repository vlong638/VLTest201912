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

        internal void Update(ProfessionalExaminationModel_SourceData sourceData)
        {
            //对比数据
            //142328199610271518	李丽	2019-12-18	2020-10-08	0	12	NULL	NULL	2020-07-30	2	
            //[{"index":"0","heartrate":"66","position":"01","presentposition":"1","fetalmove":"1"},{"index":"2","heartrate":"88","position":"02","presentposition":"2","fetalmove":"2"}]	2020-07-02	1393	NULL

            this.D1 = DateTime.Now.ToString("yyyy-MM-dd");
            //D3 矫正末次月经 = 孕妇档案.末次月经
            this.D3 = sourceData.SourceData.lastmenstrualperiod?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";
            //D4 矫正预产期 = 孕妇档案.预产期
            this.D4 = sourceData.SourceData.dateofprenatal?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";
            //D43 子宫压痛 = 子宫
            //有,无
            this.D43 = VLConstraints.Get_Uterus_By_Uterus_HELE(sourceData.SourceData.uterus);
            //D53 宫口开大 = 宫口
            this.D53 = VLConstraints.Get_PalaceMouth_By_PalaceMouth_HELE(sourceData.SourceData.palacemouth);
            //D26,处理 = 处理意见
            this.D26 = sourceData.SourceData.suggestion ?? "";
            ////D29,健康评估 = 其它评估 无需处理
            //this.D29 = sourceData.SourceData.generalcomment ?? "";
            //D39,下次预约时间 = 下次随访
            this.D39 = sourceData.SourceData.followupappointment?.ToDateTime()?.ToString("yyyy-MM-dd") ?? "";
            //-- //D44,预约目的
            //D54 胎膜破裂 = 破水
            //1 = 是  2 = 无  Hele: 1=破 2=无
            this.D54 = sourceData.SourceData.brokenwater ?? "";

            //[{"index":"0","heartrate":"66","position":"01","presentposition":"1","fetalmove":"1"},{"index":"2","heartrate":"88","position":"02","presentposition":"2","fetalmove":"2"}
            //sourceData.SourceData.feltalentities
            //胎数 D56  1 = 单胎 2 = 双胎 3 = 三胎以上
            this.D56 = VLConstraints.Get_FetalAmountByAmount(sourceData.SourceData.feltalentities.Count());
            //胎先露 D14
            //头先露,臀先露,肩先露
            if (sourceData.SourceData.feltalentities.Count() > 0)
                this.D14 = VLConstraints.Get_PresentPosition_By_PresentPosition_Hele(sourceData.SourceData.feltalentities.First().presentposition);
            //胎心
            //1胎 D13
            //2胎 D48
            //3胎 D52
            //胎方位
            //1胎 D12
            //2胎 D46
            //3胎 D51
            for (int i = 0; i < sourceData.SourceData.feltalentities.Count(); i++)
            {
                var feltalentity = sourceData.SourceData.feltalentities[i];
                if (i==0)
                {
                    this.D13 = feltalentity.heartrate;
                    //this.D12 = VLConstraints.Get_FetalPosition_By_FetalPosition_Hele(feltalentity.position);
                }
                else if (i==1)
                {
                    this.D48 = feltalentity.heartrate;
                    //this.D46 = VLConstraints.Get_FetalPosition_By_FetalPosition_Hele(feltalentity.position);
                }
                else if (i == 2)
                {
                    this.D52 = feltalentity.heartrate;
                    //this.D51 = VLConstraints.Get_FetalPosition_By_FetalPosition_Hele(feltalentity.position);
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
