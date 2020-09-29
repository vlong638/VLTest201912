using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DALSolution;
using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;

namespace FrameworkTest.Business.Research
{
    [Table(TableName)]
    public class BCheck
    {
        public const string TableName = "R_BCheck";

        public long Id { set; get; }
        public string idcard { set; get; }
        public string chushengyz { set; get; }
        public string bcheckweek { set; get; }
        public string jcdate { set; get; }
        public string age { set; get; }
        public string height { set; get; }
        public string preweight { set; get; }
        public string parity { set; get; }
        public string gravidity { set; get; }
        public string bloodtype { set; get; }
        public string rhbloodtype { set; get; }
        public string education { set; get; }
        public DateTime? dateofprenatal { set; get; }
        public string sbp { set; get; }
        public string dbp { set; get; }
        public string preterm { set; get; }
        public string mociyuejing { set; get; }
        public string pluse { set; get; }
        public string tailin { set; get; }
        public string yunnanga { set; get; }
        public string yunnangb { set; get; }
        public string yunnangc { set; get; }
        public string taiwei { set; get; }
        public string taixin { set; get; }
        public string gr { set; get; }
        public string yangshui { set; get; }
        public string jidongm { set; get; }
        public string nt { set; get; }
        public string dingtunc { set; get; }
        public string shuangdingj { set; get; }
        public string touwei { set; get; }
        public string guguc { set; get; }
        public string taierfw { set; get; }
        public string fmfs { set; get; }
        public string fmfsdm { set; get; }
        public DateTime? fmdate { set; get; }
        public string sex { set; get; }
        public string tz { set; get; }

    }

    public class BCheckRepository : RepositoryBase<BCheck>
    {
        public BCheckRepository(DbContext context) : base(context)
        {
        }

        //public IEnumerable<LabCheck> GetAll()
        //{
        //    return context.DbGroup.Connection.Query<LabCheck>($"select * from [{TableName}] order by Id desc;", transaction: _transaction);
        //}
    }
}