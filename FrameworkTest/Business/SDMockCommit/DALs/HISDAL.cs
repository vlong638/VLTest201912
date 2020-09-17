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
        internal static List<BirthDefect> GetBirthDefects(DbGroup dbGroup, string idcard)
        {
            return dbGroup.Connection.Query<BirthDefect>($@"
select * from XSECSQX2  where 身份证号 = :idcard
", new { idcard }, transaction: dbGroup.Transaction).ToList();
        }

        #endregion
    }

    public class BirthDefect
    {

    }
}
