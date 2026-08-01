using Assignement;

namespace Assignement.Services.Heartbeat;

public interface IHeartbeatProcessor
{
    Task<Machine> ProcessAsync(HeartbeatUpdate heartbeat, CancellationToken cancellationToken);
}
