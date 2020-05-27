namespace VLTest2015
{
    public static class JsonEx

    {
        public static string ToJson<T>(this T obj) where T : class
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}