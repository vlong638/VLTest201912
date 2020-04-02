namespace VL.API.Common.Utils
{
    public static class JsonEx
    {
        public static string ToJson(this object obj)
        {
            if (obj == null)
                return "";

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T FromJson<T>(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
                return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
