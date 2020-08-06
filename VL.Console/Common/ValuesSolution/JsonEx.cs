﻿namespace VL.Consolo_Core.Common.ValuesSolution
{
    public static class JsonEx
    {
        public static string ToJson<T>(this T obj) where T : class
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T FromJson<T>(this string jsonStr) where T : class
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
        }
    }
}