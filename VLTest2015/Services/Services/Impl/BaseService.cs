using System.Data.Common;
using VLTest2015.Utils;

namespace VLTest2015.Services
{
    public class BaseService
    {
        protected DbConnection _connection;

        public BaseService()
        {
            _connection = DBHelper.GetDbConnection();
        }

        ~BaseService()
        {
            _connection.Dispose();
        }
    }
}
