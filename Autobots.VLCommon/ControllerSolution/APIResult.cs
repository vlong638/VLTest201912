namespace Autobots.Infrastracture.Common.ControllerSolution
{
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    public class APIResult
    {
        public const int SuccessCode = 200;

        public APIResult(params string[] messages)
        {
            Code = SuccessCode;
            if (messages != null & messages.Length != 0)
                Message = string.Join(",", messages);
        }
        public APIResult(int code, params string[] messages)
        {
            Code = code;
            if (messages != null & messages.Length != 0)
                Message = string.Join(",", messages);
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { set; get; }
    }
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T> : APIResult
    {
        public APIResult(T data, int code, params string[] messages) : base(code, messages)
        {
            Data = data;
        }

        public APIResult(T data, params string[] messages) : base(messages)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
    }
}
