using Dapper;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Business.SDMockCommit
{
    class HISDAL
    {
        #region BirthDefect

        /// <summary>
        /// 出生缺陷
        /// </summary>
        /// <param name="dbGroup"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        internal static List<BirthDefect> GetBirthDefects(DbGroup dbGroup, string patientId)
        {
            return dbGroup.Connection.Query<BirthDefect>($@"
--select bingrenid from V_FWPT_MZ_YIJI where bingrenid = @patientId
", new { patientId }, transaction: dbGroup.Transaction).ToList();
        }

        #endregion
    }

    public class BirthDefect
    {

    }
}
