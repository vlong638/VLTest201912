namespace VL.Consolo_Core.Common.ServiceSolution
{
    public class BaseService
    {
        public ServiceResult<T> Success<T>(T data)
        {
            return new ServiceResult<T>(data);
        }

        public ServiceResult<T> Error<T>(string errorMessage = "", int errorCode = -1)
        {
            return new ServiceResult<T>(default(T), errorCode, errorMessage);
        }
    }
}
