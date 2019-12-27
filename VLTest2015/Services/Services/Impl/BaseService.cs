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

        public ResponseResult<T> Success<T>(T data)
        {
            return new ResponseResult<T>()
            {
                Data = data,
                Status = true,
            };
        }
        public ResponseResult<T> Error<T>(string errorMessage = "", int errorCode = -1)
        {
            return new ResponseResult<T>()
            {
                Status = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
            };
        }
    }
}
