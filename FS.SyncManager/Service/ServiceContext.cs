using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FS.SyncManager.Service
{
    public class ServiceContext
    {
        public string ConntectingStringSD { get { return System.Web.Configuration.WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(); } }
        public DbContext Hele_DBContext { get { return DBHelper.GetSqlDbContext(ConntectingStringSD); } }

        public SyncService SyncService
        {
            get
            {
                var context = Hele_DBContext;
                return new SyncService(context);
            }
        }
    }
}