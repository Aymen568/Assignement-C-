using Assignement.Events;

namespace Assignement;

public interface IDashboardClient
{
    Task MachineCreated(MachineCreated notification);

    Task HeartbeatProcessed(MachineHeartbeatProcessed notification);

    Task MachineStatusChanged(MachineStatusChanged notification);

    Task MachineDeleted(MachineDeleted notification);
}
