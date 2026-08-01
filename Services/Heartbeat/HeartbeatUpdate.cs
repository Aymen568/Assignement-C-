using Assignement;

namespace Assignement.Services.Heartbeat;

public record HeartbeatUpdate(
    Guid MachineId,
    DateTimeOffset Timestamp,
    Metrics Metrics,
    string CurrentJob);
