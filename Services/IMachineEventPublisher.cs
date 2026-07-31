namespace Assignement;

public interface IMachineEventPublisher
{
    Task PublishMachineCreatedAsync(MachineCreatedEvent notification, CancellationToken cancellationToken);

    Task PublishHeartbeatProcessedAsync(MachineHeartbeatProcessedEvent notification, CancellationToken cancellationToken);

    Task PublishMachineStatusChangedAsync(MachineStatusChangedEvent notification, CancellationToken cancellationToken);

    Task PublishMachineDeletedAsync(MachineDeletedEvent notification, CancellationToken cancellationToken);
}
