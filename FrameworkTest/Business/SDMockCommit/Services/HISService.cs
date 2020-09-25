using FrameworkTest.Common.DBSolution;
using System;
using System.Collections.Generic;

namespace FrameworkTest.Business.SDMockCommit
{
    public class HISService
    {
        //public static string ConntectingString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=201.201.201.81)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=MyOracleSID)));User Id=jhmz;Password=jhmz;";
        public static string ConntectingString = "Data Source= (DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST =201.201.201.81)(PORT =1521)) (CONNECT_DATA = (SERVER = DEDICATED) (SERVICE_NAME =jhemr) ) );Persist Security Info=True;User ID=jhmz;Password=jhmz";
        private DbContext DBContext;

        public HISService(DbContext context)
        {
            this.DBContext = context;
        }

        #region BirthDefect

        internal List<BirthDefect> GetBirthDefects(string idcard)
        {
            return HISDAL.GetBirthDefects(DBContext.DbGroup, idcard);
        }

        #endregion
    }

}
