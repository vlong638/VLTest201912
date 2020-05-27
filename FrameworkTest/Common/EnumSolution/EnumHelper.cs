using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FrameworkTest.Common.EnumSolution
{
    public static class EnumHelper
    {
        public static IEnumerable<T> GetAllEnums<T>()
        {
            var enums = Enum.GetNames(typeof(T)).Select(c => (T)Enum.Parse(typeof(T), c));
            return enums;
        }
    }
}