using System;

namespace FrameworkTest.Common.TimeSpanSolution
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
