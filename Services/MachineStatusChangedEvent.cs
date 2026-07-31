namespace Assignement;

public sealed record MachineStatusChangedEvent(Guid MachineId, bool OldStatus, bool NewStatus);
