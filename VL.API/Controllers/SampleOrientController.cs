using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VL.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrientSampleController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrientSampleController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public OrientSampleController(ILogger<OrientSampleController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public WeatherForecast GetOne()
        {
            var rng = new Random();
            return new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WeatherForecast> GetSome()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public int GetInt()
        {
            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<int> GetInts()
        {
            return new List<int>() { 1, 2, 3 };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetString()
        {
            return "s123";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<string> GetStrings()
        {
            return new List<string>() { "s1", "s2", "s3" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Dictionary<string, int> GetDictionary()
        {
            return new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<Dictionary<string, object>> MockData(string input)
        {
            var s = @"[{""jiuzhenid"":""1000189789"",""nianling"":null,""xitongsj"":""2019 - 04 - 09 09:38:00"",""jiezhensj"":""2019 - 04 - 09 09:38:00"",""guahaoid"":""1000245303"",""keshidm"":""107"",""keshimc"":""产科"",""guahaobc"":""上午"",""guahaoxh"":""1"",""yishengxm"":""戴贤贤"",""chushengrq"":""1990 - 12 - 18"",""jiuzhenkh"":""2012040000545393"",""xingming"":""张晓玲"",""xingbie"":""2"",""zhengjianhm"":""330304199012189760"",""bingrenid"":""1429738"",""jiatingzz"":""浙江省温州市瓯海区新桥街道站前路１９７号１１幢４０５室"",""lianxidh"":""13566262593"",""status"":4}]";
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(s);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<Dictionary<string, object>> MockData2(string input)
        {
            throw new NotImplementedException("Mock Exceptions");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public void SaveJson_PersonNames()
        {
            List<string> names = new List<string>()
            {
                "张三","李四","王五"
            };
            var file = (Path.Combine(AppContext.BaseDirectory, "configs", "PersonNames.json"));
            System.IO.File.WriteAllText(file, Newtonsoft.Json.JsonConvert.SerializeObject(names));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetPublicKey()
        {
            var result = new { exponent = "010001", code = 200, modulus = @"00af8dfa5a14e97e58cac7238a5d4ca89478c
edcfd196ea643735d64c74df659cd259c8bd60ec046c4d3f6dec3965dc0351f117f8a0ae62ad61c3
a41d38c6a93215025c658587f4aa7ceaa9ed08c2ced8873254c417a77403aff9a0abb3bc1d2ff42f
856e1a4d447ed0a1626e1099f304b6602e69cdca1a376ae6bf0dad13844cf" };
            return new JsonResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public int MockLogin(LoginModel request)
        {
            return 1;
        }
    }

    public class LoginModel
    {
        public string url { set; get; }
        public int uid { set; get; }
        public string pwd { set; get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// 
        /// </summary>
        public string Summary { get; set; }
    }
}
