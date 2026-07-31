namespace Assignement;

public sealed record MachineHeartbeatProcessedEvent(
    Guid MachineId,
    DateTimeOffset Timestamp,
    string CurrentJob,
    Metrics Metrics);
