using Autobots.Infrastracture.DataLogCenter;
using Autobots.Infrastracture.LogCenter;
using Grpc.Core;
using System.Threading.Tasks;

namespace Autobots.DataLogCenter
{
    public class DataLogServiceImpl : DataLogServiceDefinition.DataLogServiceDefinitionBase
    {
        public override Task<DataLogResponse> CommitDataLog(DataLogRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DataLogResponse() { Status = DataLogResponse.Types.ServingStatus.Serving });
        }
    }
}
