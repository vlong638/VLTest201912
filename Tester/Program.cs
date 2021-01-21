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
            var text = @"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";

            //byte[] myData = System.Text.Encoding.UTF8.GetBytes(text);
            //var ss = System.Text.Encoding.UTF8.GetString(myData);

            byte[] cbbb = Convert.FromBase64String(text);
            var a = System.Text.Encoding.Default.GetString(cbbb);

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





