using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace VL.Consolo_Core.Common.RedisSolution
{
    /// <summary>
    /// Redis 缓存实现
    /// </summary>
    public class RedisCache
    {
        /// <summary>
        /// 前缀
        /// </summary>
        private string prefix = string.Empty;
        private IDatabase client;

        /// <summary>
        /// 构造一个RedisCache实例
        /// </summary>
        /// <param name="redisConnectStr">redis链接字符串</param>
        /// <param name="prefix">缓存key前缀</param>
        public RedisCache(string redisConnectStr, string prefix = null)
        {
            if (string.IsNullOrEmpty(redisConnectStr))
            {
                throw new Exception("redis链接字符串为空");
            }
            try
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisConnectStr);
                client = redis.GetDatabase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            this.prefix = prefix;
        }
        /// <summary>
        /// 添加对象到缓存,如果已存在或添加失败则抛出异常
        /// </summary>
        /// <typeparam name="T">缓存值的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiry">过期时间，支持本地时间，内部实现将自动转UTC时间</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>或<paramref name="value"/> 参数为null</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> 已经存在值</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="expiry"/> 为DateTime.MinValue</exception>
        public void Add<T>(string key, T value, DateTime expiry)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            if (value == null) { throw new ArgumentNullException("value"); }

            var cacheKey = GetCacheKey(key);
            var success = client.StringSet(cacheKey, ConvertJson(value), GetTimeSpan(expiry), When.NotExists);
            if (!success)
            {
                throw new ArgumentNullException("key", $"key值为“{key}”的缓存已存在");
            }
        }
        /// <summary>
        /// 获取缓存，如果不存在，则返回null，如果<typeparamref name="T"/>不能为空，则抛出异常
        /// </summary>
        /// <typeparam name="T">要获取的值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns>返回获取的缓存值</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>为null</exception>
        /// <exception cref="NullReferenceException">获取缓存值为空，且<typeparamref name="T"/>为值类型，无法强制转转</exception>
        /// <exception cref="InvalidCastException">获取的缓存值无法转换为<typeparamref name="T"/></exception>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            var cacheKey = GetCacheKey(key);
            return ConvertObj<T>(client.StringGet(cacheKey));
        }
        /// <summary>
        /// 获取缓存，如果未找到，则使用acquire设置缓存并返回新值
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="acquire">获取新缓存值的方法</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="refreshForce">是否直接刷新新值，如果为true，则使用acquire获取新值刷新缓存并返回；如果为false，则缓存未找到值时，则使用acquire获取新值刷新缓存并返回</param>
        /// <returns>返回缓存值</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>或<paramref name="acquire"/>的返回值 为null</exception>
        public T Get<T>(string key, Func<T> acquire, DateTime expiry, bool refreshForce = false)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            if (acquire == null) { throw new ArgumentNullException("acquire"); }
            if (refreshForce)
            {
                T result = acquire();
                Set(key, result, expiry);
                return result;
            }
            else
            {
                T value;
                if (TryGet(key, out value))
                {
                    return value;
                }
                else
                {
                    value = acquire();
                    Set(key, value, expiry);
                    return value;
                }
            }
        }
        /// <summary>
        /// 移除指定key的缓存
        /// </summary>
        /// <param name="key">要移除的缓存key</param>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            client.KeyDelete(GetCacheKey(key));

        }
        /// <summary>
        /// 设置对象的值到指定的key，如果不存在则添加一个；如果存在，则覆盖旧值
        /// </summary>
        /// <typeparam name="T">缓存值的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="expiry">过期时间</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>或<paramref name="value"/> 参数为null</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="expiry"/> 为DateTime.MinValue</exception>
        public void Set<T>(string key, T value, DateTime expiry)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            if (value == null) { throw new ArgumentNullException("value"); }
            var cacheKey = GetCacheKey(key);
            client.StringSet(cacheKey, ConvertJson(value), GetTimeSpan(expiry));
        }
        /// <summary>
        /// 获取缓存，如果存在则返回true，并输出值；如果不存在，则返回false, value将赋值为default(T)
        /// </summary>
        /// <typeparam name="T">要获取的值的类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="value">如果存在则返回true，并输出值；如果不存在，则返回false, value将赋值为default(T)</param>
        /// <returns>如果缓存值存在且类型转换成功，则返回true，否则返回false</returns>
        public bool TryGet<T>(string key, out T value)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            var cacheKey = GetCacheKey(key);
            if (client.KeyExists(cacheKey))
            {
                try
                {
                    value = ConvertObj<T>(client.StringGet(cacheKey)); ;
                    return true;
                }
                catch
                {

                }
            }
            value = default(T);
            return false;
        }

        #region 私有方法
        /// <summary>
        /// 获取缓存的key值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetCacheKey(string key)
        {
            return this.prefix + key;
        }
        /// <summary>
        /// 将对象转换成Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        private string ConvertJson<T>(T val)
        {
            return JsonConvert.SerializeObject(val);
        }
        /// <summary>
        /// 将字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public T ConvertObj<T>(string val)
        {
            if (string.IsNullOrEmpty(val))
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(val);
        }
        /// <summary>
        /// 获取时间间隔
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private TimeSpan GetTimeSpan(DateTime time)
        {
            return time - DateTime.Now;
        }
        #endregion
    }
}