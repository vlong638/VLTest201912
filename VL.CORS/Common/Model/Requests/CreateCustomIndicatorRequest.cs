using Autobots.Infrastracture.Common.ValuesSolution;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCustomIndicatorRequest
    {
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
        internal List<BusinessEntityPropertyModel> Properties { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BusinessEntityPropertyModel
    {
        /// <summary>
        /// 字段Id
        /// </summary>
        public long Id { set; get; }
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { set; get; }
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
        /// 
        /// </summary>
        Pregnant,
        /// <summary>
        /// 
        /// </summary>
        Woman,
        /// <summary>
        /// 
        /// </summary>
        Child,
    }
}
