namespace Assignement.Events;

public record MachineDeleted(Guid MachineId, DateTimeOffset OccurredAtUtc);
