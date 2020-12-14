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
    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValue<T1, T2>
    {
        public VLKeyValue()
        {
            Key = default(T1);
            Value = default(T2);
        }
        public VLKeyValue(T1 key, T2 value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public T1 Key { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public T2 Value { set; get; }
    }
}
