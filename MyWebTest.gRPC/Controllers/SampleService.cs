using System.Threading.Tasks;
using VLService;

namespace MyWebTest.gRPC.Controllers
{
    public class SampleService
    {
        public Task<HelloReply> SayHello(HelloRequest hello)
        {
            return Task.FromResult(new HelloReply { Message = "Hello " + hello.Name });
        }
    }
}