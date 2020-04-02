﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrientSampleController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<OrientSampleController> _logger;

        public OrientSampleController(ILogger<OrientSampleController> logger)
        {
            _logger = logger;
        }

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

        [HttpGet]
        public int GetInt()
        {
            return 1;
        }

        [HttpGet]
        public List<int> GetInts()
        {
            return new List<int>() { 1, 2, 3 };
        }

        [HttpGet]
        public string GetString()
        {
            return "s123";
        }

        [HttpGet]
        public List<string> GetStrings()
        {
            return new List<string>() { "s1", "s2", "s3" };
        }

        [HttpGet]
        public Dictionary<string, int> GetDictionary()
        {
            return new Dictionary<string, int>() { { "a", 1 }, { "b", 2 } };
        }

        [HttpPost]
        public List<Dictionary<string, object>> MockData(string input)
        {
            var s = @"[{""jiuzhenid"":""1000189789"",""nianling"":null,""xitongsj"":""2019 - 04 - 09 09:38:00"",""jiezhensj"":""2019 - 04 - 09 09:38:00"",""guahaoid"":""1000245303"",""keshidm"":""107"",""keshimc"":""产科"",""guahaobc"":""上午"",""guahaoxh"":""1"",""yishengxm"":""戴贤贤"",""chushengrq"":""1990 - 12 - 18"",""jiuzhenkh"":""2012040000545393"",""xingming"":""张晓玲"",""xingbie"":""2"",""zhengjianhm"":""330304199012189760"",""bingrenid"":""1429738"",""jiatingzz"":""浙江省温州市瓯海区新桥街道站前路１９７号１１幢４０５室"",""lianxidh"":""13566262593"",""status"":4}]";
            var result=  Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(s);
            return result;
        }

        [HttpPost]
        public List<Dictionary<string, object>> MockData2(string input)
        {
            throw new NotImplementedException("MockData2 says hello");
        }
    }
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}