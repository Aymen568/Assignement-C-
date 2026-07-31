namespace Assignement;

public interface IDashboardClient
{
    Task MachineCreated(MachineCreatedEvent notification);

    Task HeartbeatProcessed(MachineHeartbeatProcessedEvent notification);

    Task MachineStatusChanged(MachineStatusChangedEvent notification);

    Task MachineDeleted(MachineDeletedEvent notification);
}
