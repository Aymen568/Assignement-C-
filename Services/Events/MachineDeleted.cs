namespace Assignement.Services.Events;

public record MachineDeleted(Guid MachineId, DateTimeOffset OccurredAtUtc);
