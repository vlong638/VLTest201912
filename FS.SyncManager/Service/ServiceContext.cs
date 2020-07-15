using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FS.SyncManager.Service
{
    public class ServiceContext
    {
        public string ConntectingStringSD = "Data Source=192.168.50.102,1433;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=ESBUSER;Password=ESBPWD";
        public DbContext Hele_DBContext { get { return DBHelper.GetDbContext(ConntectingStringSD); } }

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