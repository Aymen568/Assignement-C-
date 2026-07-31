namespace Assignement;

public sealed record HeartbeatUpdate(
    Guid MachineId,
    DateTimeOffset Timestamp,
    Metrics Metrics,
    string CurrentJob);
