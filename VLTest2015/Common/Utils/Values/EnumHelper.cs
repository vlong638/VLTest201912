using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace VLTest2015.Services
{
    public static class EnumHelper
    {
        public static IEnumerable<T> GetAllEnums<T>()
        {
            var enums = Enum.GetNames(typeof(T)).Select(c => (T)Enum.Parse(typeof(T), c));
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
    }
}