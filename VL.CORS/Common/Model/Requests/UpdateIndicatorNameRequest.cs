using System;
using System.Collections.Generic;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateIndicatorNameRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long IndicatorId { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { set; get; }
    }
}
