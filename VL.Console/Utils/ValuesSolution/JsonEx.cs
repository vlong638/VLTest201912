using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkTest.Common.ValuesSolution
{
    public static class JsonEx
    {
        public static string ToJson<T>(this T obj) where T : class
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
