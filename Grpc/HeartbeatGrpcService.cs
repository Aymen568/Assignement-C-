using Grpc.Core;
using Assignement.Grpc;

namespace Assignement;

public sealed class HeartbeatGrpcService(IHeartbeatProcessor processor) : HeartbeatService.HeartbeatServiceBase
{
    public override async Task<HeartbeatReply> SendHeartbeat(HeartbeatRequest request, ServerCallContext context)
    {
        try
        {
            var machine = await processor.ProcessAsync(request.ToHeartbeatUpdate(), context.CancellationToken);
            return new HeartbeatReply
            {
                Accepted = true,
                Message = $"Heartbeat accepted for machine '{machine.Id}'."
            };
        }
        catch (MachineNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (FormatException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (OperationCanceledException)
        {
            throw new RpcException(new Status(StatusCode.Cancelled, "The request was cancelled."));
        }
        catch (Exception)
        {
            throw new RpcException(new Status(StatusCode.Internal, "An unexpected error occurred."));
        }
    }
}
