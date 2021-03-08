using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValue<T1, T2, T3, T4>
    {
        public VLKeyValue()
        {
            ParentKey = default(T1);
            ParentValue = default(T2);
            Key = default(T3);
            Value = default(T4);
        }

        public VLKeyValue(T1 parentKey, T2 parentValue, T3 key, T4 value)
        {
            ParentKey = parentKey;
            ParentValue = parentValue;
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public T1 ParentKey { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public T2 ParentValue { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public T3 Key { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public T4 Value { set; get; }
    }

    public class VLKeyValues : List<VLKeyValue>
    {
        public string ToCaseSQL(string fieldName)
        {
            return $@"
case 
{string.Join("\r\n", this.Select(c => $"when {fieldName} = '{c.Value}' then '{c.Key}' "))}
else {fieldName}
end
";
        }
    }
}
