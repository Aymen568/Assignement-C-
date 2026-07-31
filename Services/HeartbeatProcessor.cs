using Assignement.Events;

namespace Assignement;

public class HeartbeatProcessor(IMachineRepository repository, IMachineEventPublisher eventPublisher) : IHeartbeatProcessor
{
    public async Task<Machine> ProcessAsync(HeartbeatUpdate heartbeat, CancellationToken cancellationToken)
    {
        var machine = await repository.GetByIdAsync(heartbeat.MachineId, cancellationToken);
        if (machine is null)
        {
            throw new MachineNotFoundException(heartbeat.MachineId);
        }

        var previousStatus = machine.Status;
        machine.LastHeartbeat = heartbeat.Timestamp;
        machine.CurrentJob = heartbeat.CurrentJob;
        machine.Metrics = heartbeat.Metrics;
        machine.Status = true;

        var updated = await repository.UpdateAsync(machine, cancellationToken);
        if (!updated)
        {
            throw new MachineNotFoundException(heartbeat.MachineId);
        }

        await eventPublisher.PublishHeartbeatProcessedAsync(
            new MachineHeartbeatProcessed(machine.Id, heartbeat.Timestamp, machine.CurrentJob, machine.Metrics),
            cancellationToken);

        if (previousStatus != machine.Status)
        {
            await eventPublisher.PublishMachineStatusChangedAsync(
                new MachineStatusChanged(machine.Id, previousStatus, machine.Status, DateTimeOffset.UtcNow),
                cancellationToken);
        }

        return machine;
    }
}
