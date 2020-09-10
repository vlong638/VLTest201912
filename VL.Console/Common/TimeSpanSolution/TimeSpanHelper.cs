using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace VL.Consolo_Core.Common.ValuesSolution
{
    public static class TimeSpanHelper
    {
        public static TimeSpan GetTimeSpan(Action doSomething)
        {
            DateTime dt1 = DateTime.Now;
            doSomething();
            DateTime dt2 = DateTime.Now;
            return dt2 - dt1;
        }
    }
}
