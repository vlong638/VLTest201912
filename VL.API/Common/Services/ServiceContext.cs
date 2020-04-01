using VL.API.Common.Repositories;
using VL.API.PT.Repositories;

namespace VL.API.Common.Services
{
    public class ServiceContext
    {
        #region DbGroups

        string ConnectingString_Pregnant { get { return "Data Source=crm1.heletech.cn,8082;Initial Catalog=HL_Pregnant;Pooling=true;Max Pool Size=200;Min Pool Size=0;User ID=HZFYUSER;Password=HZFYPWD"; } }
        DbGroup dbGroup_Pregnant;
        public DbGroup DbGroup_Pregnant
        {
            get
            {
                if (dbGroup_Pregnant == null)
                {
                    dbGroup_Pregnant = new DbGroup(DbHelper.GetSQLServerDbConnection(ConnectingString_Pregnant));
                }
                return dbGroup_Pregnant;
            }
        }

        string ConnectingString_Sample01 { get { return "Data Source=crm1.heletech.cn,8082;Initial Catalog=HL_Sample01;Pooling=true;Max Pool Size=200;Min Pool Size=0;User ID=HZFYUSER;Password=HZFYPWD"; } }
        DbGroup dbGroup_Sample01;
        public DbGroup DbGroup_Sample01
        {
            get
            {
                if (dbGroup_Sample01 == null)
                {
                    dbGroup_Sample01 = new DbGroup(DbHelper.GetSQLServerDbConnection(ConnectingString_Sample01));
                }
                return dbGroup_Sample01;
            }
        }

        #endregion

        #region Repositories

        PregnantInfoRepository repository_PregnantInfo;
        public PregnantInfoRepository Repository_PregnantInfo
        {
            get
            {
                if (repository_PregnantInfo == null)
                {
                    repository_PregnantInfo = new PregnantInfoRepository(DbGroup_Pregnant);
                }
                return repository_PregnantInfo;
            }
        }

        #endregion
    }
}
