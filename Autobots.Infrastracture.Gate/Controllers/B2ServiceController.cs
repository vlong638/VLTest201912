using Autobots.B2Service;
using Autobots.Infrastracture.Common.ControllerSolution;
using Autobots.ServiceProtocols;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Autobots.Infrastracture.Gateway.Controllers
{
    /// <summary>
    /// 样例服务B1
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class B2ServiceController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<HelloReply>> SayHello([FromServices] IOptions<ServiceDisvoveryOptions> serviceConfig, [FromServices] B2ServiceRPCClientProvider provider, HelloRequest request)
        {
            var client = provider.GetClient(serviceConfig.Value.ConsulService.DnsEndpoint.Address, serviceConfig.Value.ConsulService.DnsEndpoint.Port);
            var reply = await client.SayHelloAsync(request);
            Console.WriteLine(reply);
            return new APIResult<HelloReply>(reply);
        }
    }
}
