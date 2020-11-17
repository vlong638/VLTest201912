using System;
using System.Collections.Generic;
using System.Text;

namespace Autobots.Infrastracture.Common.ValuesSolution
{
    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValue
    {
        public VLKeyValue()
        {
            Key = "";
            Value = "";
        }
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
