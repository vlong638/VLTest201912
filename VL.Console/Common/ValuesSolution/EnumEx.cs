using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

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

        public static T ToEnum<T>(this long value) where T : struct
        {
            T t;
            Enum.TryParse<T>(value.ToString(), out t);
            return t;
        }

        public static string ToEnumDescription(this object value, Type t)
        {
            if (value == null || t == null)
                return null;
            int i;
            string name;
            FieldInfo fieldInfo = null;
            if (int.TryParse(value.ToString(), out i))
            {
                var fields = t.GetFields();
                foreach (var field in fields)
                {
                    if (field.FieldType.BaseType != typeof(Enum))
                        continue;

                    if ((int)field.GetValue(null) == i)
                    {
                        fieldInfo = field;
                        break;
                    }
                }
            }
            else
            {
                name = value.ToString();
                if (name == null)
                    return null;
                fieldInfo = t.GetField(name);
            }
            if (fieldInfo == null)
                return null;
            var att = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false);
            return att == null ? fieldInfo.Name : ((DescriptionAttribute)att).Description;
        }
    }
}