using Google.Protobuf.WellKnownTypes;
using Assignement.Services.Heartbeat;

namespace Assignement;

public static class HeartbeatRequestExtensions
{
    public static HeartbeatUpdate ToHeartbeatUpdate(this Assignement.Grpc.HeartbeatRequest request) => new(
        Guid.Parse(request.MachineId),
        request.Timestamp.ToDateTimeOffset(),
        new Metrics
        {
            CpuUsage = request.CpuUsage,
            MemoryUsage = request.MemoryUsage,
            Temperature = request.Temperature
        },
        request.CurrentJob);

    private static DateTimeOffset ToDateTimeOffset(this Timestamp timestamp) =>
        new(DateTime.SpecifyKind(timestamp.ToDateTime(), DateTimeKind.Utc));
}
