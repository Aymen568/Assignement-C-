using Assignement;

namespace Assignement.Services.Events;

public record MachineHeartbeatProcessed(
    Guid MachineId,
    DateTimeOffset Timestamp,
    string CurrentJob,
    Metrics Metrics);
