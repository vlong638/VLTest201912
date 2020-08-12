using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace VL.Consolo_Core.Common.ValuesSolution
{
    public static class EnumEx
    {
        public static List<T> GetAllEnums<T>(this Type type)
        {
            if (type.BaseType != typeof(Enum))
                return null;

            var enums = Enum.GetNames(type).Select(c => (T)Enum.Parse(type, c)).ToList();
            return enums;
        }

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

        public static string ToEnumDescription(this object value, Type t)
        {
            if (value == null || t == null)
                return null;
            int i;
            string name;
            if (int.TryParse(value.ToString(), out i))
            {
                name = t.GetEnumName(i);
            }
            else
            {
                name = value.ToString();
            }
            if (name == null)
            {
                return null;
            }
            var field = t.GetField(name);
            if (field == null)
            {
                return null;
            }
            var att = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
            return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        }
    }
}