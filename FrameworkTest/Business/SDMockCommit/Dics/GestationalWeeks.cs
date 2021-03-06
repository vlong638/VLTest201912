﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    public partial class VLConstraints
    {
        /// <summary>
        /// 按预产期计算
        /// </summary>
        /// <param name="prenatalDate"></param>
        /// <returns></returns>
        public static string GetGestationalWeeksByPrenatalDate(System.DateTime? prenatalDate, System.DateTime toCheck, out int weeks, out int days)
        {
            weeks = 0;
            days = 0;
            if (!prenatalDate.HasValue)
                return "-";
            if (toCheck > prenatalDate)
                return "-";
            var totalDays = (int)(toCheck - prenatalDate.Value.AddDays(-280)).TotalDays;
            weeks = totalDays / 7;
            days = totalDays % 7;
            return $"{weeks}+{days}";
        }

        /// <summary>
        /// 按预产期计算
        /// </summary>
        /// <param name="prenatalDate"></param>
        /// <returns></returns>
        public static string GetGestationalWeeksByLastMenstrualPeriodDate(System.DateTime? lastMenstrualPeriodDate, System.DateTime toCheck, out int weeks,out int days)
        {
            weeks = 0;
            days = 0;
            if (!lastMenstrualPeriodDate.HasValue)
                return "-";
            var totalDays = (int)(toCheck - lastMenstrualPeriodDate.Value).TotalDays;
            weeks = totalDays / 7;
            days = totalDays % 7;
            return $"{weeks}+{days}";
        }
    }
}
