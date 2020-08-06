using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consolo_Core.Common.ControllerSolution
{
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    public class APIResult
    {
        public const int SuccessCode = 200;

        public APIResult(params string[] messages)
        {
            code = SuccessCode;
            if (messages != null & messages.Length != 0)
                msg = string.Join(",", messages);
        }
        public APIResult(int code, params string[] messages)
        {
            this.code = code;
            if (messages != null & messages.Length != 0)
                msg = string.Join(",", messages);
        }

        /// <summary>
        /// 信息
        /// </summary>
        public string msg { set; get; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int code { set; get; }
        /// <summary>
        /// 是否成功
        /// 诸如以下的业务异常
        /// false case: 比如更新对象(没有差异项,那么Service层返回的状态是执行成功,但更新条数是0,由Controller层决定是否对外透出异常信息)
        /// 诸如以下的组件异常
        /// false case: 比如服务层返回异常(如服务层校验未通过,出现事务回滚异常)
        /// </summary>
        public bool IsValidated { get { return code == 0; } }
    }
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T> : APIResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="messages"></param>
        public APIResult(T data, int code, params string[] messages) : base(code, messages)
        {
            this.data = data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messages"></param>
        public APIResult(T data, params string[] messages) : base(messages)
        {
            this.data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T data { set; get; }
    }
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T1,T2> : APIResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="messages"></param>
        public APIResult(T1 data1, T2 data2, int code, params string[] messages) : base(code, messages)
        {
            this.data1 = data1;
            this.data2 = data2;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="messages"></param>
        public APIResult(T1 data1, T2 data2, params string[] messages) : this(data1, data2, APIResult.SuccessCode, messages)
        {
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T1 data1 { set; get; }
        public T2 data2 { set; get; }
    }
}
