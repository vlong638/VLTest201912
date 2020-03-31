using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VL.API.Common.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class V3ControllerBase : ControllerBase
    {
    }
}
