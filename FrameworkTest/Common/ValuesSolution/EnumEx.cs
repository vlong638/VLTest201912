using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FrameworkTest.Common.ValuesSolution
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
    }
}