namespace Assignement.Events;

public record MachineCreated(Guid MachineId, string Name, MachineStatus Status, DateTimeOffset OccurredAtUtc);
