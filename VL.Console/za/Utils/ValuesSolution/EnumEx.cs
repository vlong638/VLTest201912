using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace VL.Consolo_Core.Common.ValuesSolution
{
    public static class EnumEx
    {
        public static string GetDescription(this Enum @this)
        {
            var name = @this.ToString();
            var field = @this.GetType().GetField(name);
            if (field == null) return name;
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }

        public static T ToEnum<T>(this string value) where T : struct
        {
            if (value == null)
            {
                return default(T);
            }

            T t;
            Enum.TryParse<T>(value, out t);
            return t;
        }
    }
}