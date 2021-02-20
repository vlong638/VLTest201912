using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Autobots.Infrastracture.Common.ValuesSolution
{
    /// <summary>
    /// 
    /// </summary>
    public static class VLAutoMappler
    {
        /// <summary>
        /// 注意,只自动匹配 '类型'和'名称'一致的属性
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static List<string> Diffs(this object from, object to)
        {
            List<string> properties = new List<string>();
            PropertyInfo[] fromProperties = from.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            PropertyInfo[] toProperties = to.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var fromProperty in fromProperties)
            {
                var matchedProperty = toProperties.FirstOrDefault(c => c.Name == fromProperty.Name);
                if (matchedProperty != null && matchedProperty.PropertyType == fromProperty.PropertyType)
                {
                    properties.Add(fromProperty.Name);
                }
            }
            return properties;
        }
        /// <summary>
        /// 注意,只自动匹配 '类型'和'名称'一致的属性
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void MapTo(this object from, object to)
        {
            PropertyInfo[] fromProperties = from.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            PropertyInfo[] toProperties = to.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var fromProperty in fromProperties)
            {
                var matchedProperty = toProperties.FirstOrDefault(c => c.Name == fromProperty.Name);
                if (matchedProperty != null && matchedProperty.PropertyType == fromProperty.PropertyType)
                {
                    matchedProperty.SetValue(to, fromProperty.GetValue(from));
                }
            }
        }

        public static string RenderIdToText<T>(this T id, Dictionary<T, string> source) where T : IComparable
        {
            if (id.Equals(default(T)))
                return null;
            List<T> ids = new List<T>() { id };
            var values = RenderIdToText(ids, source);
            return values.First();
        }

        public static List<string> RenderIdToText<T>(this List<T> ids, Dictionary<T, string> source)
        {
            if (ids == null || ids.Count == 0)
                return null;
            var dic = source;
            var values = ids.Select(c => dic.ContainsKey(c) ? dic[c] : c.ToString()).ToList();
            return values;
        }
    }
}
