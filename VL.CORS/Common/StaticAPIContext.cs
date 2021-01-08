using Autobots.Infrastracture.Common.RedisSolution;

namespace ResearchAPI.CORS.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class StaticAPIContext
    {
        /// <summary>
        /// 
        /// </summary>
        public static RedisCache RedisCache { set; get; }
    }
}
