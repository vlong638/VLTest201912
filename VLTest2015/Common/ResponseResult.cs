namespace VLTest2015.Common
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ResponseResult<T> // : IResponse
    {
        public ResponseResult()
        {
        }

        public ResponseResult(T data)
        {
            Data = data;
        }

        public T Data { set; get; }
        public bool Status { set; get; }
        public int ErrorCode { set; get; }
        public string ErrorMessage { set; get; }
    }
}