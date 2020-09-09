using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace VL.Consolo_Core.Common.RedisSolution
{
    /// <summary>
    /// Redis ����ʵ��
    /// </summary>
    public class RedisCache
    {
        /// <summary>
        /// ǰ׺
        /// </summary>
        private string prefix = string.Empty;
        private IDatabase client;

        /// <summary>
        /// ����һ��RedisCacheʵ��
        /// </summary>
        /// <param name="redisConnectStr">redis�����ַ���</param>
        /// <param name="prefix">����keyǰ׺</param>
        public RedisCache(string redisConnectStr, string prefix = null)
        {
            if (string.IsNullOrEmpty(redisConnectStr))
            {
                throw new Exception("redis�����ַ���Ϊ��");
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
        /// ��Ӷ��󵽻���,����Ѵ��ڻ����ʧ�����׳��쳣
        /// </summary>
        /// <typeparam name="T">����ֵ������</typeparam>
        /// <param name="key">����key</param>
        /// <param name="value">����ֵ</param>
        /// <param name="expiry">����ʱ�䣬֧�ֱ���ʱ�䣬�ڲ�ʵ�ֽ��Զ�תUTCʱ��</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>��<paramref name="value"/> ����Ϊnull</exception>
        /// <exception cref="ArgumentException"><paramref name="key"/> �Ѿ�����ֵ</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="expiry"/> ΪDateTime.MinValue</exception>
        public void Add<T>(string key, T value, DateTime expiry)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            if (value == null) { throw new ArgumentNullException("value"); }

            var cacheKey = GetCacheKey(key);
            var success = client.StringSet(cacheKey, ConvertJson(value), GetTimeSpan(expiry), When.NotExists);
            if (!success)
            {
                throw new ArgumentNullException("key", $"keyֵΪ��{key}���Ļ����Ѵ���");
            }
        }
        /// <summary>
        /// ��ȡ���棬��������ڣ��򷵻�null�����<typeparamref name="T"/>����Ϊ�գ����׳��쳣
        /// </summary>
        /// <typeparam name="T">Ҫ��ȡ��ֵ����</typeparam>
        /// <param name="key">����key</param>
        /// <returns>���ػ�ȡ�Ļ���ֵ</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>Ϊnull</exception>
        /// <exception cref="NullReferenceException">��ȡ����ֵΪ�գ���<typeparamref name="T"/>Ϊֵ���ͣ��޷�ǿ��תת</exception>
        /// <exception cref="InvalidCastException">��ȡ�Ļ���ֵ�޷�ת��Ϊ<typeparamref name="T"/></exception>
        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            var cacheKey = GetCacheKey(key);
            return ConvertObj<T>(client.StringGet(cacheKey));
        }
        /// <summary>
        /// ��ȡ���棬���δ�ҵ�����ʹ��acquire���û��沢������ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ����</typeparam>
        /// <param name="key">����key</param>
        /// <param name="acquire">��ȡ�»���ֵ�ķ���</param>
        /// <param name="expiry">����ʱ��</param>
        /// <param name="refreshForce">�Ƿ�ֱ��ˢ����ֵ�����Ϊtrue����ʹ��acquire��ȡ��ֵˢ�»��沢���أ����Ϊfalse���򻺴�δ�ҵ�ֵʱ����ʹ��acquire��ȡ��ֵˢ�»��沢����</param>
        /// <returns>���ػ���ֵ</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>��<paramref name="acquire"/>�ķ���ֵ Ϊnull</exception>
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
        /// �Ƴ�ָ��key�Ļ���
        /// </summary>
        /// <param name="key">Ҫ�Ƴ��Ļ���key</param>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            client.KeyDelete(GetCacheKey(key));

        }
        /// <summary>
        /// ���ö����ֵ��ָ����key����������������һ����������ڣ��򸲸Ǿ�ֵ
        /// </summary>
        /// <typeparam name="T">����ֵ������</typeparam>
        /// <param name="key">����key</param>
        /// <param name="value">����ֵ</param>
        /// <param name="expiry">����ʱ��</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>��<paramref name="value"/> ����Ϊnull</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="expiry"/> ΪDateTime.MinValue</exception>
        public void Set<T>(string key, T value, DateTime expiry)
        {
            if (string.IsNullOrEmpty(key)) { throw new ArgumentNullException("key"); }
            if (value == null) { throw new ArgumentNullException("value"); }
            var cacheKey = GetCacheKey(key);
            client.StringSet(cacheKey, ConvertJson(value), GetTimeSpan(expiry));
        }
        /// <summary>
        /// ��ȡ���棬��������򷵻�true�������ֵ����������ڣ��򷵻�false, value����ֵΪdefault(T)
        /// </summary>
        /// <typeparam name="T">Ҫ��ȡ��ֵ������</typeparam>
        /// <param name="key">����key</param>
        /// <param name="value">��������򷵻�true�������ֵ����������ڣ��򷵻�false, value����ֵΪdefault(T)</param>
        /// <returns>�������ֵ����������ת���ɹ����򷵻�true�����򷵻�false</returns>
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

        #region ˽�з���
        /// <summary>
        /// ��ȡ�����keyֵ
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetCacheKey(string key)
        {
            return this.prefix + key;
        }
        /// <summary>
        /// ������ת����Json�ַ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        private string ConvertJson<T>(T val)
        {
            return JsonConvert.SerializeObject(val);
        }
        /// <summary>
        /// ���ַ���ת���ɶ���
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
        /// ��ȡʱ����
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