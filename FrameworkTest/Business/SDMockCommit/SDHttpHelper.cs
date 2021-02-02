using FrameworkTest.Common.HttpSolution;
using System;
using System.Collections.Generic;
using System.Net;

namespace FrameworkTest.Business.SDMockCommit
{
    public class SDHttpHelper
    {
        public static string Post(string url, string postData, ref CookieContainer container, string contentType = "application/x-www-form-urlencoded; charset=UTF-8", Action<HttpWebRequest> configRequest = null, List<KeyValuePair<string, string>> keyValues = null)
        {
            keyValues = keyValues ?? new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("Company", "e9c278ae05c40689bbb724f016d48875"));//000169 不同账号相同
            return HttpHelper.Post(url, postData, ref container, contentType, configRequest, keyValues);
        }

        public static string Get(string url, string postData, ref CookieContainer container)
        {
            return HttpHelper.Get(url, postData, ref container);
        }
    }
}
