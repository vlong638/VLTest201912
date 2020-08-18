using System.Collections.Generic;

namespace VL.Research.Models
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class KeyValue
    {
        private string v1;
        private string v2;

        public KeyValue(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public string Value { set; get; }
    }
    /// <summary>
    /// 统一查询入参结构
    /// </summary>
    public class GetCommonSelectRequest
    {
        /// <summary>
        /// 目标视图
        /// </summary>
        public string target { set; get; }
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
        public List<KeyValue> search { set; get; }
    }
}