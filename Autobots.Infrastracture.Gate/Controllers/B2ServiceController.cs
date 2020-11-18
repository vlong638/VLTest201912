using Autobots.B1ServiceDefinition;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Autobots.B2ServiceDefinition.B2Service;

namespace Autobots.Infrastracture.Gate.Controllers
{
    /// <summary>
    /// 样例服务B1
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class B2ServiceController : ControllerBase
    {
        const string Server = "localhost";
        const int Port = 5000;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<HelloReply> SayHello(HelloRequest request)
        {
            Channel channel = new Channel($"{Server}:{Port}", ChannelCredentials.Insecure);
            var client = new B2ServiceClient(channel);
            var reply = await client.SayHelloAsync(request);
            return reply;
        }
    }
}
