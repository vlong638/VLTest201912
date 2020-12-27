using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class GetIndicatorsResponse
    {
        /// <summary>
        /// 指标名称
        /// </summary>
        public string IndicatorName { set; get; }
        /// <summary>
        /// 指标类别
        /// </summary>
        public int IndicatorType { set; get; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { set; get; }
        /// <summary>
        /// 规则
        /// </summary>
        public string Rule { set; get; }
    }
}
