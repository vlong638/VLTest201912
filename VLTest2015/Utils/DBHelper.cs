using System.Data.Common;
using System.Data.SqlClient;

namespace VLTest2015.Utils
{
    public class DBHelper
    {
        public static DbConnection GetDbConnection()
        {
            //VLTODO 可以优化为连接池
            return new SqlConnection("Data Source=.;Initial Catalog=VLTest;Integrated Security=True;MultipleActiveResultSets=True");
        }
    }
}