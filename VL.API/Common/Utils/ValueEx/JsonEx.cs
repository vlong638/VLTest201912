namespace VL.API.Common.Utils
{
    /// <summary>
    /// Json扩展方法
    /// </summary>
    public static class JsonEx
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            if (obj == null)
                return "";

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
