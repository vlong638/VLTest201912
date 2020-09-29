namespace FrameworkTest.Common.ValuesSolution
{
    public class PregnantCalculator
    {
        /// <summary>
        /// 按预产期计算
        /// </summary>
        /// <param name="prenatalDate"></param>
        /// <returns></returns>
        public static string GetGestationalWeeksByPrenatalDate(System.DateTime? prenatalDate, System.DateTime dateToCheck, out int weeks, out int days)
        {
            weeks = 0;
            days = 0;
            if (!prenatalDate.HasValue)
                return "-";
            if (dateToCheck > prenatalDate)
                return "-";
            var totalDays = (int)(dateToCheck - prenatalDate.Value.AddDays(-280)).TotalDays;
            weeks = totalDays / 7;
            days = totalDays % 7;
            return $"{weeks}+{days}";
        }

        /// <summary>
        /// 按末次月经计算
        /// </summary>
        /// <param name="prenatalDate"></param>
        /// <returns></returns>
        public static string GetGestationalWeeksByLastMenstrualPeriodDate(System.DateTime? lastMenstrualPeriodDate, System.DateTime dateToCheck, out int weeks, out int days)
        {
            weeks = 0;
            days = 0;
            if (!lastMenstrualPeriodDate.HasValue)
                return "-";
            var totalDays = (int)(dateToCheck - lastMenstrualPeriodDate.Value).TotalDays;
            weeks = totalDays / 7;
            days = totalDays % 7;
            return $"{weeks}+{days}";
        }
    }
}
