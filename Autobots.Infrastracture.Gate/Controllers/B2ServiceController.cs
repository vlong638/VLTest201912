using Autobots.B2ServiceDefinition;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Autobots.Infrastracture.Gate.Controllers
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
        public async Task<HelloReply> SayHello([FromServices] B2ServiceRPCClientProvider provider, HelloRequest request)
        {
            var client = provider.GetClient();
            var reply = await client.SayHelloAsync(request);
            return reply;
        }
    }
}
