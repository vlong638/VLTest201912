using Dapper;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.ValuesSolution;
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
        public string 姓名 { set; get; }
        public string 身份证号 { set; get; }
        public string 住院号 { set; get; }
        public string 住院次 { set; get; }
        public string 出生缺陷诊断 { set; get; }
        public string 第17尿道下裂类型 { set; get; }
        public string 第23先天性心脏病类型 { set; get; }
        public string 第24出生缺陷诊断其它描述 { set; get; }
        public string 第25胎儿水肿综合征类型 { set; get; }
        public string 第26地中海贫血类型 { set; get; }

        public string GetRemark()
        {
            return 第17尿道下裂类型.IsNotNullOrEmpty() ? 第17尿道下裂类型 :
                第23先天性心脏病类型.IsNotNullOrEmpty() ? 第23先天性心脏病类型 :
                第24出生缺陷诊断其它描述.IsNotNullOrEmpty() ? 第24出生缺陷诊断其它描述 :
                第25胎儿水肿综合征类型.IsNotNullOrEmpty() ? 第25胎儿水肿综合征类型 :
                第26地中海贫血类型.IsNotNullOrEmpty() ? 第26地中海贫血类型 :
                "";
        }
    }
}