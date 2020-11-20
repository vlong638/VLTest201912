using Autobots.Infrastracture.LogCenter;
using Grpc.Core;
using System.Threading.Tasks;

namespace Autobots.LogCenter
{
    public class LogServiceImpl : LogServiceDefinition.LogServiceDefinitionBase
    {
        public override Task<LogResponse> Info(LogRequest request, ServerCallContext context)
        {
            Log4NetLogger.Info(request.ToString());
            return Task.FromResult(new LogResponse() { Status= LogResponse.Types.ServingStatus.Serving });
        }
        public override Task<LogResponse> Warn(LogRequest request, ServerCallContext context)
        {
            return Task.FromResult(new LogResponse() { Status = LogResponse.Types.ServingStatus.Serving });
        }
        public override Task<LogResponse> Error(LogRequest request, ServerCallContext context)
        {
            return Task.FromResult(new LogResponse() { Status = LogResponse.Types.ServingStatus.Serving });
        }
    }
}
