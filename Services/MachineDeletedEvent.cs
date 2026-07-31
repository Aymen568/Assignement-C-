namespace Assignement;

public sealed record MachineDeletedEvent(Guid MachineId, DateTimeOffset OccurredAtUtc);
