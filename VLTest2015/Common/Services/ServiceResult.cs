using System;

namespace VLTest2015.Services
{
    /// <summary>
    /// Service层返回结构
    /// </summary>
    public class ServiceResult
    {
        public const int SuccessCode = 200;

        public ServiceResult(params string[] messages)
        {
            this.Code = SuccessCode;
            this.Messages = messages;
        }
        public ServiceResult(int code, params string[] messages)
        {
            this.Code = code;
            this.Messages = messages;
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string[] Messages { set; get; }
        public string Message { get { return string.Join(",", Messages); } }
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
        public bool IsSuccess { get { return Messages == null || Messages.Length == 0; } }
    }
    /// <summary>
    /// 服务端返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T> : ServiceResult
    {
        public ServiceResult(T data, int code, params string[] messages) : base(code, messages)
        {
            Data = data;
        }

        public ServiceResult(T data, params string[] messages) : base(messages)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
    }
}