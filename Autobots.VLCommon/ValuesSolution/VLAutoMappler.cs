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
    }
}
