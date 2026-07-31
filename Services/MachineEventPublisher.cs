using Microsoft.AspNetCore.SignalR;

namespace Assignement;

public sealed class MachineEventPublisher(IHubContext<DashboardHub, IDashboardClient> hubContext) : IMachineEventPublisher
{
    public Task PublishMachineCreatedAsync(MachineCreatedEvent notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.MachineCreated(notification);
    }

    public Task PublishHeartbeatProcessedAsync(MachineHeartbeatProcessedEvent notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.HeartbeatProcessed(notification);
    }

    public Task PublishMachineStatusChangedAsync(MachineStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.MachineStatusChanged(notification);
    }

    public Task PublishMachineDeletedAsync(MachineDeletedEvent notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.MachineDeleted(notification);
    }
}
