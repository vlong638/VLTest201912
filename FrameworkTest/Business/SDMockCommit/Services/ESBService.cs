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
    public class ESBService
    {
        public static string ConntectingString = "Data Source=201.201.201.89;Initial Catalog=HELEESB;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=5";
        private DbContext DBContext;

        public ESBService(DbContext context)
        {
            this.DBContext = context;
        }

        #region SyncOrder
        public SyncOrder GetSyncOrder(TargetType TargetType, string sourceId)
        {
            return SDDAL.GetSyncForFS(DBContext.DbGroup, TargetType, sourceId);
        }

        public long SaveSyncOrder(SyncOrder syncForFS)
        {
            if (syncForFS.Id > 0)
            {
                SDDAL.UpdateSyncForFS(DBContext.DbGroup, syncForFS);
                return syncForFS.Id;
            }
            else
            {
                return SDDAL.InsertSyncForFS(DBContext.DbGroup, syncForFS);
            }
        }
        #endregion
    }
}
