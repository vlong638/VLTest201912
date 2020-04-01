using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VL.API.Common.Services;

namespace VL.API.Common.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class V3ControllerBase : ControllerBase
    {
        public APIResult<T> Success<T>(T data)
        {
            return new APIResult<T>(data);
        }
        public APIResult<T> Error<T>(T data, params string[] messages)
        {
            return new APIResult<T>(data, messages);
        }
        public APIResult<T> Error<T>(T data, int code, params string[] messages)
        {
            return new APIResult<T>(data, code, messages);
        }
    }
}
