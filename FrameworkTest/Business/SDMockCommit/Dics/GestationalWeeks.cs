using System;
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
        public static string GetGestationalWeeksByPrenatalDate(DateTime? prenatalDate, DateTime toCheck)
        {
            if (!prenatalDate.HasValue)
                return "-";
            if (toCheck > prenatalDate)
                return "-";
            var days = (int)(toCheck - prenatalDate.Value.AddDays(-280)).TotalDays;
            var subWeeks = days / 7;
            var subDay = days % 7;
            return $"{subWeeks}+{subDay}";
        }
    }
}
