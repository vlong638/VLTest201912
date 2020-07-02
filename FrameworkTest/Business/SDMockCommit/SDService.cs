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


        internal static string GetPhysicalExaminationId(UserInfo userInfo, WCQBJ_CZDH_DOCTOR_READResponse base8, DateTime issueDate, ref StringBuilder logger)
        {
            var container = new CookieContainer();
            var dateStr = issueDate.ToString("yyyy-MM-dd");
            //获取体格检查Id
            var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/WMH_TODAY_CQJC_ID_READ&sUserID={userInfo.UserId}&sParams={base8.MainId}${dateStr}";
            var postData = "";
            var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
            logger.AppendLine($"查询-获取体格检查Id");
            logger.AppendLine(url);
            logger.AppendLine(result);
            var re2 = result.FromJson<WMH_TODAY_CQJC_ID_READ>();
            if (string.IsNullOrEmpty(re2.d1))
                return null;
            return re2.d1;
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

        internal List<PhysicalExaminationModel> GetPhysicalExaminationsToCreate()
        {
            return SDDAL.GetPhysicalExaminationsToCreate(DBContext.DbGroup);
        }

        #endregion

        #region ProfessionalExamination

        internal List<ProfessionalExaminationModel> GetProfessionalExaminationsToCreate()
        {
            return SDDAL.GetProfessionalExaminationsToCreate(DBContext.DbGroup);
        }

        internal IEnumerable<ProfessionalExaminationModel> GetProfessionalExaminationsToUpdate()
        {
            return SDDAL.GetProfessionalExaminationsToUpdate(DBContext.DbGroup);
        }

        #endregion



    }
}
