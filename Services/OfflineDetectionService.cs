using Assignement.Events;

namespace Assignement;

public class OfflineDetectionService(IMachineRepository repository, IMachineEventPublisher eventPublisher) : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(5);
    private static readonly TimeSpan OfflineTimeout = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(CheckInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var machines = await repository.GetAllAsync(stoppingToken);
            var now = DateTimeOffset.UtcNow;

            foreach (var machine in machines)
            {
                if (machine.Status == MachineStatus.Offline || now - machine.LastHeartbeat <= OfflineTimeout)
                {
                    continue;
                }

                var previousStatus = machine.Status;
                machine.Status = MachineStatus.Offline;

                if (await repository.UpdateAsync(machine, stoppingToken))
                {
                    await eventPublisher.PublishMachineStatusChangedAsync(
                        new MachineStatusChanged(machine.Id, previousStatus, machine.Status, now),
                        stoppingToken);
                }
            }
        }
    }
}
