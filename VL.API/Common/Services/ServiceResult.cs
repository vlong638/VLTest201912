namespace VL.API.Common.Services
{
    /// <summary>
    /// 服务端返回结构
    /// </summary>
    public class ServiceResult
    {
        public ServiceResult(string message, int code)
        {
            this.Message = message;
            this.Code = code;
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
        /// false case: 校验未通过(通用或非通用校验)
        /// false case: 需事务回滚的异常逻辑
        /// 诸如以下的组件异常
        /// false case: 数据库连接异常
        /// false case: 事务执行异常
        /// false case: 数据库更新异常(如主外键限制,字符串长度截断等)
        /// false case: 消息发送失败
        /// </summary>
        public bool IsSuccess { get { return !string.IsNullOrEmpty(Message); } }
    }
    /// <summary>
    /// 服务端返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        public const int SuccessCode = 200;

        public ServiceResult(T data, string message = null, int code = SuccessCode) : base(message, code)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
    }
}
