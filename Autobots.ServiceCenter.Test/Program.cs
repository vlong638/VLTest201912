using Consul;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Autobots.ServiceCenter.Test
{
    class Program
    {
        static List<string> Urls = new List<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("开始输出当前所有服务地址");
            Catalog_Nodes().GetAwaiter().GetResult();
            //Console.WriteLine(HelloConsul().GetAwaiter().GetResult());
            Console.WriteLine("开始随机请求一个地址服务地址");
            int index = new Random().Next(Urls.Count);
            string url = Urls[index];
            Console.WriteLine("请求的随机地址：" + url);
            HttpClient client = new HttpClient();
            var result = client.GetAsync(url).Result;
            //string result = HttpClientHelpClass.PostResponse(url, param, out string statusCode);
            Console.WriteLine("返回状态：" + result.StatusCode);
            Console.WriteLine("返回结果：" + result.Content);
            Console.ReadLine();
        }
        public static async Task Catalog_Nodes()
        {
            var client = new ConsulClient(ConfigurationOverview);
            var nodeList = await client.Agent.Services();
            var url = nodeList.Response.Values;
            foreach (var item in url)
            {
                string Address = item.Address;
                int port = item.Port;
                string name = item.Service;
                Console.WriteLine($"地址：{Address}:{port},name：{name}");
                Urls.Add($"http://{Address}:{port}/weatherforecast");
            }
        }
        private static void ConfigurationOverview(ConsulClientConfiguration obj)
        {
            //consul的地址
            obj.Address = new Uri("http://192.168.1.37:8500");
            //数据中心命名
            obj.Datacenter = "dc1";
        }
    }
}
