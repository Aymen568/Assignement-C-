using Assignement.Events;
using Microsoft.AspNetCore.SignalR;

namespace Assignement;

public class MachineEventPublisher(IHubContext<DashboardHub, IDashboardClient> hubContext) : IMachineEventPublisher
{
    public Task PublishMachineCreatedAsync(MachineCreated notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.MachineCreated(notification);
    }

    public Task PublishHeartbeatProcessedAsync(MachineHeartbeatProcessed notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.HeartbeatProcessed(notification);
    }

    public Task PublishMachineStatusChangedAsync(MachineStatusChanged notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.MachineStatusChanged(notification);
    }

    public Task PublishMachineDeletedAsync(MachineDeleted notification, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return hubContext.Clients.All.MachineDeleted(notification);
    }
}
