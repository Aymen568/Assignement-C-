namespace Assignement;

public record HeartbeatUpdate(
    Guid MachineId,
    DateTimeOffset Timestamp,
    Metrics Metrics,
    string CurrentJob);
