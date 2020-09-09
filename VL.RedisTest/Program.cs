using System;
using System.Net.Http;
using VL.Consolo_Core.Common.RedisSolution;

namespace VL.RedisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RedisCache("127.0.0.1:6379");
            client.Set<string>("vltest", "vlvlvl", DateTime.Now.AddMinutes(5));
            var test = client.Get<string>("vltest");


            //var client = new RedisCache("127.0.0.1:6379,password=123456");

            Console.WriteLine("Hello World!");
        }
    }
}
