using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCustomIndicatorRequest
    {
        ///// <summary>
        ///// 指标名称: 如 孕12-16周血红蛋白
        ///// </summary>
        //public string Name { set; get; }
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { set; get; }
        /// <summary>
        /// 目标人群
        /// </summary>
        public TargetArea TargetArea { set; get; }
        /// <summary>
        /// 搜索项
        /// 约定: 
        /// 孕产妇孕周检验模板: (@孕周小值-@孕周大值, @多次时分组:{0:最早,1:最晚,2:平均值,3:最小值,4:最大值})
        /// </summary>
        public List<VLKeyValue> Search { set; get; }
        /// <summary>
        /// 添加的字段
        /// 默认为:检验值
        /// 可选有:检验值,检验日期,检验单号
        /// </summary>
        public List<BusinessEntityPropertyModel> Properties { set; get; }
        /// <summary>
        /// 时间周期模板
        /// </summary>
        public List<BusinessEntityPeriodTemplate> PeriodTemplates { set; get; }
    }

    /// <summary>
    /// 时间周期模板
    /// </summary>
    public class BusinessEntityPeriodTemplate
    {
        /// <summary>
        /// 起始参数名
        /// </summary>
        public string StartAtComponentName { set; get; }
        /// <summary>
        /// 起始
        /// </summary>
        public string StartAt { set; get; }
        /// <summary>
        /// 截止参数名
        /// </summary>
        public string EndAtComponentName { set; get; }
        /// <summary>
        /// 截止
        /// </summary>
        public string EndAt { set; get; }
        /// <summary>
        /// 字段显示名称
        /// </summary>
        public string PropertyDisplayName{set;get;}
    }

    /// <summary>
    /// 选择的属性
    /// </summary>
    public class BusinessEntityPropertyModel
    {
        /// <summary>
        /// 字段Id
        /// </summary>
        public long TemplatePropertyId { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TargetArea
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 孕期检验模板
        /// </summary>
        PregnantLabResult =1,
        /// <summary>
        /// 
        /// </summary>
        PregnantVisitRecord = 2,
        /// <summary>
        /// 
        /// </summary>
        Child,
    }
}