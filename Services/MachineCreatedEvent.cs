namespace Assignement;

public sealed record MachineCreatedEvent(Guid MachineId, string Name, bool Status, DateTimeOffset OccurredAtUtc);
