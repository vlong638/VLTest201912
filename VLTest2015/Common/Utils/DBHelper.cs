using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace VLTest2015.Utils
{
    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetDbConnection()
        {
            //return new SqlConnection("Data Source=LAPTOP-NQBU1OIS\\SQLEXPRESS;Initial Catalog=VLTest;Integrated Security=True;MultipleActiveResultSets=True");
            return new SqlConnection("Data Source=heletech.asuscomm.com,8082;Initial Catalog=VLTest;Pooling=true;Max Pool Size=40000;Min Pool Size=0;User ID=HZFYUSER;Password=HZFYPWD");
        }
    }
}