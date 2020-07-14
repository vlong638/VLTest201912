namespace FrameworkTest.Common.ControllerSolution
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
        /// <summary>
        /// 是否成功
        /// 诸如以下的业务异常
        /// false case: 比如更新对象(没有差异项,那么Service层返回的状态是执行成功,但更新条数是0,由Controller层决定是否对外透出异常信息)
        /// 诸如以下的组件异常
        /// false case: 比如服务层返回异常(如服务层校验未通过,出现事务回滚异常)
        /// </summary>
        public bool IsValidated { get { return Code == 200; } }
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
