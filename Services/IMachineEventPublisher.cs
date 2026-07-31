using Assignement.Events;

namespace Assignement;

public interface IMachineEventPublisher
{
    Task PublishMachineCreatedAsync(MachineCreated notification, CancellationToken cancellationToken);

    Task PublishHeartbeatProcessedAsync(MachineHeartbeatProcessed notification, CancellationToken cancellationToken);

    Task PublishMachineStatusChangedAsync(MachineStatusChanged notification, CancellationToken cancellationToken);

    Task PublishMachineDeletedAsync(MachineDeleted notification, CancellationToken cancellationToken);
}
