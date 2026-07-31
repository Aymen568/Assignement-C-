namespace Assignement;

public interface IHeartbeatProcessor
{
    Task<Machine> ProcessAsync(HeartbeatUpdate heartbeat, CancellationToken cancellationToken);
}
