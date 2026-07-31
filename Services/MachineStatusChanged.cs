namespace Assignement.Events;

public record MachineStatusChanged(Guid MachineId, bool OldStatus, bool NewStatus, DateTimeOffset OccurredAtUtc);
