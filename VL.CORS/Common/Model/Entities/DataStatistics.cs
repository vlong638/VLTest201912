using Dapper.Contrib.Extensions;
using System;
using System.ComponentModel;

namespace ResearchAPI.CORS.Common
{

    [Table("DataStatistics")]
    public class DataStatistics
    {
        [Key]
        public long Id { set; get; }
        public string Name { set; get; }
        public DataStatisticsCategory Category { set; get; }
        public string Value { set; get; }
        public DateTime IssueTime { set; get; }
        public string Parent { set; get; }
        public string Message { set; get; }
    }

    public enum DataStatisticsCategory
    {
        /// <summary>
        /// 产科数据生成时间
        /// </summary>
        PT_GeneratedTime =101001,

        #region PregnantInfo, 101001

        /// <summary>
        /// 产妇总数
        /// </summary>
        [Description("产妇总数")]
        PT_PatientCount = 101001001,
        /// <summary> 
        ///  档案总数
        /// </summary>
        [Description("档案总数")]
        PT_PregnantRecordCount = 101001002,

        /// <summary> 
        ///  建册年龄分布 lt20  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 lt20  @createtime,birthday")]
        PT_CreateBookAge_lt20_Count = 101001003,
        /// <summary> 
        ///  建册年龄分布 20-29  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 20-29  @createtime,birthday")]
        PT_CreateBookAge_20_29_Count = 101001004,
        /// <summary> 
        ///  建册年龄分布 30-34  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 30-34  @createtime,birthday")]
        PT_CreateBookAge_30_34_Count = 101001005,
        /// <summary> 
        ///  建册年龄分布 35-39  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 35-39  @createtime,birthday")]
        PT_CreateBookAge_35_39_Count = 101001006,
        /// <summary> 
        ///  建册年龄分布 40-44  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 40-44  @createtime,birthday")]
        PT_CreateBookAge_40_44_Count = 101001007,
        /// <summary> 
        ///  建册年龄分布 gt44  @createtime,birthday 
        /// </summary>
        [Description("建册年龄分布 gt44  @createtime,birthday")]
        PT_CreateBookAge_gt_44_Count = 101001008,
        #endregion

        #region MHC_VisitRecord,101003

        /// <summary> 
        ///  病历总数 
        /// </summary>
        [Description("病历总数")]
        PT_MHC_VisitRecordCount = 101003001,

        [Description("诊断_频次统计预处理")]
        PT_MHC_VisitRecord_Diagnosis = 101003991,

        #endregion

        #region MHC_VisitRecord_Monthly,101103

        /// <summary> 
        ///  病历总数 月周期 
        /// </summary>
        [Description("病历总数 月周期")]
        PT_MHC_VisitRecordCount_Monthly = 101103001,

        #endregion

        #region MHC_HighRiskReason,101007

        /// <summary> 
        ///  五色高危分布_绿色 
        /// </summary>
        [Description("五色高危分布_绿色")]
        PT_RiskLevel_Green_Count = 101007001,
        /// <summary> 
        ///  五色高危分布_黄色 
        /// </summary>
        [Description("五色高危分布_黄色")]
        PT_RiskLevel_Yellow_Count = 101007002,
        /// <summary> 
        ///  五色高危分布_橙色 
        /// </summary>
        [Description("五色高危分布_橙色")]
        PT_RiskLevel_Orange_Count = 101007003,
        /// <summary> 
        ///  五色高危分布_红色 
        /// </summary>
        [Description("五色高危分布_红色")]
        PT_RiskLevel_Red_Count = 101007004,
        /// <summary> 
        ///  五色高危分布_紫色 
        /// </summary>
        [Description("五色高危分布_紫色")]
        PT_RiskLevel_Violet_Count = 101007005,

        #endregion

        #region CDH_NeonateRecord 101008

        /// <summary> 
        ///  新生儿总数 
        /// </summary>
        [Description("新生儿总数")]
        PT_ChildCount = 101008001,
        /// <summary> 
        ///  新生儿档案总数 
        /// </summary>
        [Description("新生儿档案总数")]
        PT_ChildRecordCount = 101008002,
        /// <summary> 
        ///  新生儿男孩总数 
        /// </summary>
        [Description("新生儿男孩总数")]
        PT_BoyCount = 101008004,
        /// <summary> 
        ///  新生儿女孩总数 
        /// </summary>
        [Description("新生儿女孩总数")]
        PT_GirlCount = 101008005,

        #endregion

        #region CDH_NeonateRecord 101108

        /// <summary> 
        ///  新生儿档案总数月周期 
        /// </summary>
        [Description("新生儿档案总数月周期")]
        PT_ChildRecordCount_Monthly = 101108002,

        #endregion

        #region CDH_DeliveryRecord 101009

        /// <summary> 
        ///  已分娩产妇总数 
        /// </summary>
        [Description("已分娩产妇总数")]
        PT_MotherCount = 101009001,
        /// <summary> 
        ///  待分娩产妇总数 
        /// </summary>
        [Description("待分娩产妇总数")]
        PT_PregnantCount = 101009002,
        /// <summary> 
        ///  流产产妇总数 
        /// </summary>
        [Description("流产产妇总数")]
        PT_AbortionCount = 101009003,

        /// <summary> 
        ///  分娩结局`存活`母亲总数 @fenmianjjmc 
        ///  CDH_DeliveryRecord: 553773
        ///  PregnantInfo: 1174824
        ///  342148 = 13s
        /// </summary>
        [Description("分娩结局`存活`母亲总数 @fenmianjjmc")]
        PT_PregnancyOutcomeAliveCount = 101009004,
        /// <summary> 
        ///  分娩结局`死胎`母亲总数 @fenmianjjmc 
        /// </summary>
        [Description("分娩结局`死胎`母亲总数 @fenmianjjmc")]
        PT_PregnancyOutcomeStillBirthCount = 101009005,
        /// <summary> 
        ///  分娩结局`死产`母亲总数 @fenmianjjmc 
        /// </summary>
        [Description("分娩结局`死产`母亲总数 @fenmianjjmc")]
        PT_PregnancyOutcomeDeadBirthCount = 101009006,

        /// <summary> 
        ///  母亲结局`产时死亡`母亲总数 @chanfujjmc 
        /// </summary>
        [Description("母亲结局`产时死亡`母亲总数 @chanfujjmc")]
        PT_MotherOutcomeDeadBirthCount = 101009007,
        /// <summary> 
        ///  母亲结局`存活`母亲总数 @chanfujjmc 
        /// </summary>
        [Description("母亲结局`存活`母亲总数 @chanfujjmc")]
        PT_MotherOutcomeAliveCount = 101009008,

        //<=27周+6天
        //>=28周  and <=36周+6天
        //>=37周  and <41周+6天
        //>=42

        /// <summary> 
        ///  分娩孕周 小于28周 
        /// </summary>
        [Description("分娩孕周 小于28周")]
        PT_Boy_DeliveryWeekCount_lt28 = 101009011,
        /// <summary> 
        ///  分娩孕周 28周-37周 
        /// </summary>
        [Description("分娩孕周 28周-37周")]
        PT_Boy_DeliveryWeekCount_28_37 = 101009012,
        /// <summary> 
        ///  分娩孕周 37周-41周 
        /// </summary>
        [Description("分娩孕周 37周-41周")]
        PT_Boy_DeliveryWeekCount_37_41 = 101009013,
        /// <summary> 
        ///  分娩孕周 大于41周 
        /// </summary>
        [Description("分娩孕周 大于41周")]
        PT_Boy_DeliveryWeekCount_gt41 = 101009014,

        /// <summary> 
        ///  分娩孕周 小于28周 
        /// </summary>
        [Description("分娩孕周 小于28周")]
        PT_Girl_DeliveryWeekCount_lt28 = 101009015,
        /// <summary> 
        ///  分娩孕周 28周-37周 
        /// </summary>
        [Description("分娩孕周 28周-37周")]
        PT_Girl_DeliveryWeekCount_28_37 = 101009016,
        /// <summary> 
        ///  分娩孕周 37周-41周 
        /// </summary>
        [Description("分娩孕周 37周-41周")]
        PT_Girl_DeliveryWeekCount_37_41 = 101009017,
        /// <summary> 
        ///  分娩孕周 大于41周 
        /// </summary>
        [Description("分娩孕周 大于41周")]
        PT_Girl_DeliveryWeekCount_gt41 = 101009018,


        /// <summary> 
        ///  分娩方式`产钳助产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`产钳助产` @fenmianfsmc")]
        PT_DeliveryMode_ForcepsCount = 101009020,
        /// <summary> 
        ///  分娩方式`毁胎术` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`毁胎术` @fenmianfsmc")]
        PT_DeliveryMode_DestroyCount = 101009021,
        /// <summary> 
        ///  分娩方式`剖宫产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`剖宫产` @fenmianfsmc")]
        PT_DeliveryMode_CaesareanCount = 101009022,
        /// <summary> 
        ///  分娩方式`其他` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`其他` @fenmianfsmc")]
        PT_DeliveryMode_OtherCount = 101009023,
        /// <summary> 
        ///  分娩方式`其它` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`其它` @fenmianfsmc")]
        PT_DeliveryMode_Other2Count = 101009024,
        /// <summary> 
        ///  分娩方式`胎头吸引` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`胎头吸引` @fenmianfsmc")]
        PT_DeliveryMode_ExtractionCount = 101009025,
        /// <summary> 
        ///  分娩方式`臀位助产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`臀位助产` @fenmianfsmc")]
        PT_DeliveryMode_BreechAssistCount = 101009026,
        /// <summary> 
        ///  分娩方式`阴道自然分娩` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`阴道自然分娩` @fenmianfsmc")]
        PT_DeliveryMode_VaginalCount = 101009027,
        /// <summary> 
        ///  分娩方式`治疗性引产` @fenmianfsmc 
        /// </summary>
        [Description("分娩方式`治疗性引产` @fenmianfsmc")]
        PT_DeliveryMode_InducedCount = 101009028,

        #endregion

        #region LabOrder 199001

        /// <summary> 
        ///  检验记录数 
        /// </summary>
        [Description("检验记录数")]
        Common_LabOrderCount = 199001001,

        #endregion

        #region LabResult 199002

        /// <summary> 
        ///  检验记录项目数 
        /// </summary>
        [Description("检验记录项目数")]
        Common_LabResultCount = 199002001,

        #endregion
    }

}