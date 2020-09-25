using Dapper;
using Dapper.Contrib.Extensions;
using FrameworkTest.Common.DBSolution;
using FrameworkTest.Common.FileSolution;
using FrameworkTest.Common.HttpSolution;
using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FrameworkTest.Business.SDMockCommit
{
    public class ServiceContext
    {
        public string ConnectingString_Pregnant = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";

        public DbContext DBContext_Pregnant { get { return DBHelper.GetSqlDbContext(PregnantService.ConntectingString); } }
        public DbContext DBContext_ESB { get { return DBHelper.GetSqlDbContext(ESBService.ConntectingString); } }
        public DbContext DBContext_HIS { get { return DBHelper.GetOracleDbContext(HISService.ConntectingString); } }

        public PregnantService PregnantService
        {
            get
            {
                var context = DBContext_Pregnant;
                return new PregnantService(context);
            }
        }

        public HISService HISService
        {
            get
            {
                var context = DBContext_HIS;
                return new HISService(context);
            }
        }


        public ESBService ESBService
        {
            get
            {
                var context = DBContext_ESB;
                return new ESBService(context);
            }
        }

        public FSService FSService
        {
            get
            {
                return new FSService();
            }
        }
    }
}
