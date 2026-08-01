using Assignement.Events;

namespace Assignement;

public class HeartbeatProcessor(IMachineRepository repository, IMachineEventPublisher eventPublisher) : IHeartbeatProcessor
{
    private const int MaxRetries = 3;

    public async Task<Machine> ProcessAsync(HeartbeatUpdate heartbeat, CancellationToken cancellationToken)
    {
        int retryCount = 0;

        while (retryCount < MaxRetries)
        {
            var machine = await repository.GetByIdAsync(heartbeat.MachineId, cancellationToken);
            if (machine is null)
            {
                throw new MachineNotFoundException(heartbeat.MachineId);
            }

            // Idempotency check: suppress duplicate or stale heartbeats
            if (heartbeat.Timestamp <= machine.LastHeartbeat)
            {
                return machine; // stale or duplicate heartbeat, no-op, no event
            }

            var previousStatus = machine.Status;
            machine.LastHeartbeat = heartbeat.Timestamp;
            machine.CurrentJob = heartbeat.CurrentJob;
            machine.Metrics = heartbeat.Metrics;
            machine.Status = MachineStatus.Online;

            if (await repository.UpdateAsync(machine, cancellationToken))
            {
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

            // CAS failed because of a concurrent write — retry
            retryCount++;
        }

        // Failed to update after max retries
        throw new InvalidOperationException($"Failed to process heartbeat for machine {heartbeat.MachineId} after {MaxRetries} retries due to concurrent updates.");
    }
}
