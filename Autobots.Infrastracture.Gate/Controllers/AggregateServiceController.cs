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
    public class AggregateServiceController : ControllerBase
    {
        public class HelloRequestModel
        {
            public string Name { set; get; }
        }
        public class HelloReplyModel
        {
            public string Name { set; get; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<APIResult<HelloReplyModel>> BothSayHello([FromServices] IOptions<ServiceDisvoveryOptions> serviceConfig, [FromServices] B1ServiceRPCClientProvider b1Provider, [FromServices] B2ServiceRPCClientProvider b2Provider, HelloRequestModel request)
        {
            var b1Client = b1Provider.GetClient(serviceConfig.Value.ConsulService.DnsEndpoint.Address, serviceConfig.Value.ConsulService.DnsEndpoint.Port);
            var b2Client = b2Provider.GetClient(serviceConfig.Value.ConsulService.DnsEndpoint.Address, serviceConfig.Value.ConsulService.DnsEndpoint.Port);
            var b1Reply = await b1Client.SayHelloAsync(new B1Service.HelloRequest() { Name = request.Name });
            var b2Reply = await b2Client.SayHelloAsync(new B2Service.HelloRequest() { Name = request.Name });
            Console.WriteLine(b1Reply);
            Console.WriteLine(b2Reply);
            var result = new HelloReplyModel() { Name = b1Reply.ToString() + b2Reply.ToString() };
            return new APIResult<HelloReplyModel>(result);
        }
    }
}
