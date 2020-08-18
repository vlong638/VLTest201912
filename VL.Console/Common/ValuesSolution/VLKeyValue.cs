using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consolo_Core.Common.ValuesSolution
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValue
    {
        public VLKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
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
}
