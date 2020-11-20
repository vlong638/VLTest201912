namespace Autobots.Infrastracture.Common.ControllerSolution
{
    /// <summary>
    /// Controller层返回结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIRequest<T> : APIResult
    {
        public APIRequest(T data, string token)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { set; get; }
        /// <summary>
        /// 用以确立用户身份
        /// </summary>
        public string Token { set; get; }
    }
}
