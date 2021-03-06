﻿
using System;
using System.ComponentModel;
using System.Reflection;

namespace VL.API.Common.Utils
{
    /// <summary>
    /// 枚举扩展方法
    /// </summary>
    public static class EnumEx
    {
        /// <summary>
        /// 获取枚举值上的Description特性的说明
        /// </summary>
        /// <returns>特性的说明</returns>
        public static string GetDescription(this Enum obj)
        {
            if (obj == null)
                return "";
            var type = obj.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, obj));
            DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }

            return descAttr.Description;
        }
    }
}
