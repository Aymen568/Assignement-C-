namespace Assignement.Events;

public record MachineCreated(Guid MachineId, string Name, bool Status, DateTimeOffset OccurredAtUtc);
