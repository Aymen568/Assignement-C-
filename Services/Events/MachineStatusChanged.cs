using Assignement;

namespace Assignement.Services.Events;

public record MachineStatusChanged(Guid MachineId, MachineStatus OldStatus, MachineStatus NewStatus, DateTimeOffset OccurredAtUtc);
