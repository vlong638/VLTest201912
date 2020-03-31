using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VL.API.Common.Controllers;

namespace VL.API.Controllers
{
    /// <summary>
    /// 产科
    /// </summary>
    public class PTController : VLControllerBase
    {
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
    }
}
