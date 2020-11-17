using Autobots.B1ServiceDefinition;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Autobots.B1ServiceDefinition.B1Service;

namespace Autobots.Infrastracture.Gate.Controllers
{
    /// <summary>
    /// 样例服务B1
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class B1ServiceController : ControllerBase
    {
        const string Server = "localhost";
        const int Port = 30051;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HelloReply> SayHello(HelloRequest request)
        {
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new B1ServiceClient(channel);
            var reply = await client.SayHelloAsync(request);
            return reply;
        }
    }
}
