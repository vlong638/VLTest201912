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
    public class SDDAL
    {
        public static long InsertSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Insert(syncForFS, transaction: group.Transaction);
        }

        public static bool UpdateSyncForFS(DbGroup group, SyncOrder syncForFS)
        {
            return group.Connection.Update(syncForFS, transaction: group.Transaction);
        }

        internal static SyncOrder GetSyncForFS(DbGroup group, SourceType sourceType, string sourceId)
        {
            return group.Connection.Query<SyncOrder>("select * from SyncForFS where sourceType = @sourceType and sourceId = @sourceId", new { sourceType, sourceId }, transaction: group.Transaction).FirstOrDefault();
        }
    }
}
