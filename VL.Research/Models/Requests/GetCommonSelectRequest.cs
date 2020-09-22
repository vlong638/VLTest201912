using System.Collections.Generic;
using VL.Consolo_Core.Common.ValuesSolution;

namespace VL.Research.Models
{
    /// <summary>
    /// 统一查询入参结构
    /// </summary>
    public class GetCommonSelectRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public int page { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int limit { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string field { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string order { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public List<VLKeyValue> search { set; get; }
    }
}