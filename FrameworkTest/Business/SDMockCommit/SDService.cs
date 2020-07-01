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
    public class SDService
    {
        public static string ConntectingStringSD = "Data Source=201.201.201.89;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=sdfy;Password=sdfy123456";
        public static DbContext GetDBContext { get { return DBHelper.GetDbContext(SDBLL.ConntectingStringSD); } }

        public static List<PregnantInfo> TempPregnantInfos = new List<PregnantInfo>();
        //BaseInfo baseInfo = new BaseInfo()
        //{
        //    UserId = "35000528",
        //    UserName = "廖凤贤",
        //    OrgId = "45608491-9",
        //    OrgName = "佛山市妇幼保健院",
        //};
        public static UserInfo UserInfo = new UserInfo()
        {
            UserId = "35021069",
            UserName = "赵卓姝",
            OrgId = "45608491-9",
            OrgName = "佛山市妇幼保健院",
            EncodeUserName = HttpUtility.UrlEncode("赵卓姝"),
        };

        private DbContext DBContext;

        public SDService(DbContext context)
        {
            this.DBContext = context;
        }

        #region SyncOrder
        public SyncOrder GetSyncOrder(SourceType sourceType, string sourceId)
        {
            return SDDAL.GetSyncForFS(DBContext.DbGroup, sourceType, sourceId);
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

        #region PhysicalExamination

        internal List<SourceData_PhysicalExaminationModel> GetPhysicalExaminationsToCreate()
        {
            return SDDAL.GetPhysicalExaminationsToCreate(DBContext.DbGroup);
        }

        #endregion

    }
}
