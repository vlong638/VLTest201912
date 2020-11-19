using Autobots.B1Service;
using Autobots.Infrastracture.Common.ControllerSolution;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Autobots.Infrastracture.Gateway.Controllers
{
    /// <summary>
    /// 样例服务B1
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class B1ServiceController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<HelloReply>> SayHello([FromServices] B1ServiceRPCClientProvider provider, HelloRequest request)
        {
            var client = provider.GetClient();
            var reply = await client.SayHelloAsync(request);
            Console.WriteLine(reply);
            return new APIResult<HelloReply>(reply);
        }
    }
}
