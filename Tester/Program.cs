using FrameworkTest.Common.ValuesSolution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
            if (true)
            {
                var array = new int[] { 2,1,2,1,0,1,2 };

                //var count = 100;
                //var array = new int[count];
                //Random random = new Random();
                //for (int i = 0; i < array.Length; i++)
                //{
                //    array[i] = random.Next(1000);
                //}
                var rs = new VLRanges(array)
                {
                    Array = array,
                };
                var k = 2;
                rs.GetProfits(k);
                rs.Ranges = rs.Ranges.OrderBy(c => c.Left).ToList();
                var profits = rs.Ranges.Where(c => c.Profit > 0).ToList();
                var sum = 0;
                foreach (var profit in profits)
                {
                    sum += profit.Profit;
                }
                //var sum = profits.Sum(c => c.Profit);

                //var times = 5;
                //var profit = GetMaxProfit(array, 0, array.Length, times, out int s, out int end);





            }

            if (false)
            {
                var tstr = "leetcode";
                Dictionary<char, List<int>> dicstr = new Dictionary<char, List<int>>();
                for (int i = 0; i < tstr.Length; i++)
                {
                    var item = tstr[i];
                    if (dicstr.ContainsKey(item))
                    {
                        dicstr[item].Add(i);
                    }
                    else
                    {
                        dicstr.Add(item, new List<int>() { i });
                    }
                }
                Console.WriteLine(string.Join("\r\n", dicstr.Select(c => c.Key + ":" + string.Join(",", c.Value))));
                Console.WriteLine(dicstr.FirstOrDefault(c => c.Value.Count() == 1).Value[0]); 
            }

            var pwd = GetHashValue("123456");
            Console.WriteLine(pwd);

            if (false)
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
        class VLRanges
        {
            public VLRanges(int[] array)
            {
                Array = array;
            }

            public int[] Array { set; get; }
            public List<VLRange> Ranges { set; get; } = new List<VLRange>();

            internal void GetProfits(int count)
            {
                while (count > 0)
                {
                    AddOne();
                    count--;
                }
            }

            private void AddOne()
            {
                if (Ranges.Count == 0)
                {
                    var left = 0;
                    var right = Array.Length - 1;
                    var maxProfit = 0;
                    var maxProfitLeft = 0;
                    var maxProfitRight = 0;
                    for (int i = left; i <= right; i++)
                    {
                        for (int j = i + 1; j <= right; j++)
                        {
                            var profit = Array[j] - Array[i];
                            if (profit > maxProfit)
                            {
                                maxProfit = profit;
                                maxProfitLeft = i;
                                maxProfitRight = j;
                            }
                        }
                    }
                    if (maxProfitLeft != left)
                    {
                        Ranges.Add(new VLRange(0, maxProfitLeft));
                    }
                    Ranges.Add(new VLRange(maxProfitLeft, maxProfitRight, maxProfit));
                    if (maxProfitRight != right)
                    {
                        Ranges.Add(new VLRange(maxProfitRight, right));
                    }
                }
                else
                {
                    foreach (var range in Ranges)
                    {
                        if (range.NextProfit == 0)
                        {
                            range.GetNextProfit(Array);
                        }
                    }
                    var maxProfit = Ranges.Max(c => c.NextProfit);
                    if (maxProfit == 0)
                    {
                        return;
                        throw new NotImplementedException("投资次数无法用尽");
                    }
                    var currentRange = Ranges.First(c => c.NextProfit == maxProfit);
                    currentRange.GetNextProfit(Array);
                    Ranges.Remove(currentRange);
                    Ranges.AddRange(currentRange.Split(Array));
                }
            }
        }

        class VLRange
        {
            public VLRange(int start, int end)
            {
                Left = start;
                Right = end;
            }

            public VLRange(int start, int end, int profit) : this(start, end)
            {
                Profit = profit;
            }

            public int Left { set; get; }
            public int Right { set; get; }
            public int Profit { set; get; }

            public bool DoAsc { set; get; }
            public int NextProfit { set; get; }
            public int NextLeft { set; get; }
            public int NextRight { set; get; }

            internal void GetNextProfit(int[] array)
            {
                var left = Left;
                var right = Right;
                var maxProfit = 0;
                var maxProfitLeft = 0;
                var maxProfitRight = 0;
                for (int i = left; i <= right; i++)
                {
                    for (int j = i + 1; j <= right; j++)
                    {
                        if (Profit>0)
                        {
                            var profit = array[i] - array[j];
                            if (profit > maxProfit)
                            {
                                if (Left == i && Right == j)
                                    continue;

                                maxProfit = profit;
                                maxProfitLeft = i;
                                maxProfitRight = j;
                            }
                        }
                        else
                        {
                            var profit = array[j] - array[i];
                            if (profit > maxProfit)
                            {
                                if (Left == i && Right == j)
                                    continue;

                                maxProfit = profit;
                                maxProfitLeft = i;
                                maxProfitRight = j;
                            }
                        }
                    }
                }
                NextProfit = maxProfit;
                NextLeft = maxProfitLeft;
                NextRight = maxProfitRight;
            }

            internal List<VLRange> Split(int[] array)
            {
                if (Profit <= 0)
                {
                    GetNextProfit(array);
                    var result = new List<VLRange>();
                    if (NextLeft != Left)
                    {
                        result.Add(new VLRange(Left, NextLeft, array[NextLeft] - array[Left]));
                    }
                    result.Add(new VLRange(NextLeft, NextRight, array[NextRight] - array[NextLeft]));
                    if (NextRight != Right)
                    {
                        result.Add(new VLRange(NextRight, Right, array[Right] - array[NextRight]));
                    }
                    return result;
                }
                else
                {
                    var result = new List<VLRange>();
                    if (Left != NextLeft)
                    {
                        result.Add(new VLRange(Left, NextLeft, array[NextLeft] - array[Left]));
                    }
                    result.Add(new VLRange(NextLeft, NextRight, array[NextRight] - array[NextLeft]));
                    if (Right != NextRight)
                    {
                        result.Add(new VLRange(NextRight, Right, array[Right] - array[NextRight]));
                    }
                    return result;
                }
            }
        }


        //private static int GetMaxProfit(int[] array, int times,int[] breakPoints,bool isIncrease)
        //{
        //    int profit;
        //    int start, end;
        //    //处理计算
        //    start = 4;
        //    end = 7;
        //    //处理结果
        //    if (times==1)
        //    {
        //        profit = 66;
        //    }
        //    else
        //    {
        //        var profits = GetMaxProfit(breakPoints);
        //        profit += Max(profits);
        //    }
        //}

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






