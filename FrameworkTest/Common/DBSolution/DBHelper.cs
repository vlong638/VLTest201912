﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.DBSolution
{
    public static class DBHelper
    {
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static DbConnection GetSqlDbConnection(string connectingString)
        {
            return new SqlConnection(connectingString);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DbContext GetDbContext(string connectingString)
        {
            var connection = GetSqlDbConnection(connectingString);
            return new DbContext(connection);
        }
    }
}
