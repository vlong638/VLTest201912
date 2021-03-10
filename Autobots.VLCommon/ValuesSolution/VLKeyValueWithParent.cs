using Autobots.Infrastracture.Common.ServiceSolution;
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
    public class VLKeyValue<T1, T2, T3>
    {
        public VLKeyValue()
        {
            Key = default(T1);
            Value1 = default(T2);
            Value2 = default(T3);
        }
        public VLKeyValue(T1 key, T2 value1, T3 value2)
        {
            Key = key;
            Value1 = value1;
            Value2 = value2;
        }

        /// <summary>
        /// 
        /// </summary>
        public T1 Key { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public T2 Value1 { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public T3 Value2 { set; get; }

        public static List<VLKeyValue<string, string, string>> Merge(List<VLKeyValue> data1, List<VLKeyValue> data2)
        {
            var result = new List<VLKeyValue<string, string, string>>();
            var items = data1.Select(c => c.Key).Concat(data2.Select(c => c.Key)).Distinct();
            foreach (var item in items)
            {
                var d1 = data1.FirstOrDefault(c => c.Key == item);
                var d2 = data2.FirstOrDefault(c => c.Key == item);
                result.Add(new VLKeyValue<string, string, string>(item, d1?.Value ?? "0", d2?.Value ?? "0"));
            }
            return result;
        }
    }

    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValueWithParent<T1, T2, T3, T4>
    {
        public VLKeyValueWithParent()
        {
            ParentKey = default(T1);
            ParentValue = default(T2);
            Key = default(T3);
            Value = default(T4);
        }

        public VLKeyValueWithParent(T1 parentKey, T2 parentValue, T3 key, T4 value)
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
