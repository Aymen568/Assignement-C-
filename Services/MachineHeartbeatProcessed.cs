namespace Assignement.Events;

public record MachineHeartbeatProcessed(
    Guid MachineId,
    DateTimeOffset Timestamp,
    string CurrentJob,
    Metrics Metrics);
