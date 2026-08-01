using Grpc.Core;
using Assignement.Grpc;

namespace Assignement;

public class HeartbeatGrpcService(IHeartbeatProcessor processor) : HeartbeatService.HeartbeatServiceBase
{
    public override async Task<HeartbeatReply> SendHeartbeat(HeartbeatRequest request, ServerCallContext context)
    {
        try
        {
            // Validate machine_id parses as a Guid and is non-empty
            if (string.IsNullOrWhiteSpace(request.MachineId) || !Guid.TryParse(request.MachineId, out _))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "machine_id must be a valid, non-empty GUID."));
            }

            var machine = await processor.ProcessAsync(request.ToHeartbeatUpdate(), context.CancellationToken);
            return new HeartbeatReply
            {
                Accepted = true,
                Message = $"Heartbeat accepted for machine '{machine.Id}'."
            };
        }
        catch (RpcException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw ToRpcException(ex);
        }
    }

    private static RpcException ToRpcException(Exception exception) => exception switch
    {
        MachineNotFoundException ex => new RpcException(new Status(StatusCode.NotFound, ex.Message)),
        FormatException or InvalidOperationException => new RpcException(new Status(StatusCode.InvalidArgument, exception.Message)),
        OperationCanceledException => new RpcException(new Status(StatusCode.Cancelled, "The request was cancelled.")),
        _ => new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."))
    };
}
