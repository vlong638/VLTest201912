using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{

    /// <summary>
    /// 键值对
    /// </summary>
    public class VLKeyValue
    {
        public VLKeyValue()
        {
            Key = "";
            Value = "";
        }
        public VLKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Key { set; get; }
        /// <summary>
        /// /
        /// </summary>
        public string Value { set; get; }
    }

    class Program
    {
        public static string GetHashValue(string input)
        {
            using (MD5 mi = MD5.Create())
            {
                //开始加密
                byte[] buffer = Encoding.Default.GetBytes(input);
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        class A
        {
            public string Type { set; get; }
        }
        class CA: A
        {
            public string Name { set; get; }
        }

        static void Main(string[] args)
        {


            var pwd = GetHashValue("123456");
            Console.WriteLine(pwd);


            if (true)
            {
                var sources = File.ReadAllLines(@"C:\Users\Administrator\Desktop\杭妇院\1223.原Id名单.txt");
                var sourcePersons = new List<VLKeyValue>();
                var targetPersons = new List<VLKeyValue>();
                foreach (var source in sources)
                {
                    var key = source.Split(',');
                    sourcePersons.Add(new VLKeyValue(key[1], key[0]));
                }
                var ties = File.ReadAllLines(@"C:\Users\Administrator\Desktop\杭妇院\1223.新Idcard名单.txt");
                var newid = 3000000;
                var countold = 0;
                var countnew= 0;
                foreach (var tie in ties)
                {
                    var idcard = tie;
                    var id = 0;
                    var fetched = sourcePersons.FirstOrDefault(c => c.Key == idcard);
                    if (fetched != null)
                    {
                        id = fetched.Value.ToInt().Value;
                        countold++;
                    }
                    else
                    {
                        id = newid;
                        newid++;
                        countnew++;
                    }
                    targetPersons.Add(new VLKeyValue(idcard, id.ToString()));
                }
                var s = countold + "," + countnew;
                StringBuilder sb = new StringBuilder();
                foreach (var item in targetPersons)
                {
                    sb.AppendLine(item.Key + "\t" + item.Value);
                }
                File.WriteAllText(@"C:\Users\Administrator\Desktop\杭妇院\1223.Output.txt", sb.ToString());
            }
            if (false)
            {
                var sources = File.ReadAllLines(@"C:\Users\Administrator\Desktop\杭妇院\1211.铁剂补充源列表.txt");
                var sourcePersons = new List<VLKeyValue>();
                foreach (var source in sources)
                {
                    var key = source.Replace(" ", "");
                    sourcePersons.Add(new VLKeyValue(key, "否"));
                }
                var ties = File.ReadAllLines(@"C:\Users\Administrator\Desktop\杭妇院\1211.铁剂名单.txt");
                foreach (var tie in ties)
                {
                    var key = tie.Replace(" ", "").Replace("/", "-");
                    var fetched = sourcePersons.FirstOrDefault(c => c.Key == key);
                    if (fetched != null)
                    {
                        fetched.Value = "是";
                    }
                }
                StringBuilder sb = new StringBuilder();
                foreach (var sourcePerson in sourcePersons)
                {
                    sb.AppendLine(sourcePerson.Key + "\t\t" + sourcePerson.Value);
                }
                File.WriteAllText(@"C:\Users\Administrator\Desktop\杭妇院\1211.Output.txt", sb.ToString());
            }
            if (false)
            {
                var grouper = new List<Grouper>();
                Data.AddData(grouper);
                var groupedData = grouper.GroupBy(c => c.id);
                StringBuilder sb = new StringBuilder();
                foreach (var items in groupedData)
                {
                    //12 16 24 32 37
                    List<KeyValuePair<string, string>> validValues = new List<KeyValuePair<string, string>>();
                    AddBorder(items, validValues, 0, 12, 12);
                    AddBorder(items, validValues, 13, 21, 16);
                    AddBorder(items, validValues, 22, 29, 24);
                    AddBorder(items, validValues, 30, 34, 32);
                    AddBorder(items, validValues, 35, 37, 36);
                    AddBorder(items, validValues, 38, 50, 40);
                    sb.AppendLine($"{items.Key + "\t"} {string.Join("\t", validValues.Select(c => (c.Key ?? "") + "\t" + c.Value).ToList().Distinct())}");
                }
                var s = sb.ToString();
            }
            if (false)
            {
                Console.WriteLine("發起請求2");
                var container = new CookieContainer();
                var url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/CQJL_LIST&sUserID=35021069&sParams=null$45608491-9$2$%E7%8E%8B%E9%A6%99%E7%8E%89$P$P$4406";
                var postData = "pageIndex=0&pageSize=1000&sortField=&sortOrder=";
                var result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                Console.WriteLine(result);

                Console.WriteLine("發起請求2");
                container = new CookieContainer();
                url = $"http://19.130.211.1:8090/FSFY/disPatchJson?clazz=READDATA&UITYPE=WCQBJ/CQJL_LIST&sUserID=35021069&sParams=null$45608491-9$1$0000289796$P$P$4406";
                postData = "pageIndex=0&pageSize=1000&sortField=&sortOrder=";
                result = HttpHelper.Post(url, postData, ref container, contentType: "application/x-www-form-urlencoded; charset=UTF-8");
                Console.WriteLine(result);

                //var postData = new { pageIndex = 0, pageSize = 20, sortField = "", sortOrder = "" }.ToJson();
                //var result = HttpHelper.Post(url, postData, ref container, contentType: "application/json; charset=UTF-8");
            }
            Console.ReadLine();
        }

        private static void AddBorder(IGrouping<string, Grouper> items, List<KeyValuePair<string, string>> validValues, int minweek, int maxweek, int border)
        {
            var closeItems = items.Where(c => c.week >= minweek && c.week <= maxweek && c.weight.IsNotNullOrEmpty() && c.weight != "NULL");
            if (closeItems.Count() == 0)
            {
                validValues.Add(new KeyValuePair<string, string>("", ""));
            }
            else
            {
                var closest = closeItems.Min(c => c.week * 10 + (c.day.ToInt() ?? 0));
                var closestItem = closeItems.First(c => c.week == closest / 10 && (c.day.ToInt() ?? 0) == closest % 10);
                validValues.Add(new KeyValuePair<string, string>(closestItem.week + (closestItem.day == "" ? "" : ("/" + closestItem.day)), closestItem.weight));
            }
        }
    }
    public class HttpHelper
    {
        public static string Post(string url, string postData, ref CookieContainer container, string contentType = "application/x-www-form-urlencoded; charset=UTF-8", Action<HttpWebRequest> configRequest = null)
        {
            var result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytepostData = encoding.GetBytes(postData); ;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "Post";
                request.ContentType = contentType;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.CookieContainer = container;//获取验证码时候获取到的cookie会附加在这个容器里面
                request.KeepAlive = true;//建立持久性连接
                if (bytepostData != null)
                {
                    request.ContentLength = bytepostData.Length;
                    using (Stream requestStm = request.GetRequestStream())
                    {
                        requestStm.Write(bytepostData, 0, bytepostData.Length);
                    }
                }
                configRequest?.Invoke(request);
                response = (HttpWebResponse)request.GetResponse();//响应
                container.Add(response.Cookies);
                using (Stream responseStm = response.GetResponseStream())
                {
                    StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                    result = redStm.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static string Get(string url, string postData, ref CookieContainer container)
        {
            var result = "";
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] bytepostData = encoding.GetBytes(postData); ;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:5.0.1) Gecko/20100101 Firefox/5.0.1";
                request.Accept = "image/webp,*/*;q=0.8";

                #region cookie处理
                request.CookieContainer = container;

                //request.CookieContainer = new CookieContainer();//!Very Important.!!!
                //container = request.CookieContainer;
                //var c = request.CookieContainer.GetCookies(request.RequestUri);
                //response = (HttpWebResponse)request.GetResponse();
                //response.Cookies = container.GetCookies(request.RequestUri); 
                #endregion

                response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStm = response.GetResponseStream())
                {
                    StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                    result = redStm.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
    }
}

public class Grouper
{
    public string id;
    public int week;
    public string day;
    public string weight;

    public Grouper(string id, int week, string day, string weight)
    {
        this.id = id;
        this.week = week;
        this.day = day;
        this.weight = weight;
    }
}






