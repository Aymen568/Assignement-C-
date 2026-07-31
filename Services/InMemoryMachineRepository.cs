using System.Collections.Concurrent;

namespace Assignement;

public sealed class InMemoryMachineRepository : IMachineRepository
{
    private readonly ConcurrentDictionary<Guid, Machine> machines = new();

    public Task<Machine> CreateAsync(Machine machine, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var created = Clone(machine);
        created.Id = Guid.NewGuid();

        machines[created.Id] = Clone(created);
        return Task.FromResult(Clone(created));
    }

    public Task<IReadOnlyCollection<Machine>> GetAllAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult((IReadOnlyCollection<Machine>)machines.Values.Select(Clone).OrderBy(machine => machine.Name).ToArray());
    }

    public Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        machines.TryGetValue(id, out var machine);
        return Task.FromResult(machine is null ? null : Clone(machine));
    }

    public Task<bool> UpdateAsync(Machine machine, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        while (true)
        {
            if (!machines.TryGetValue(machine.Id, out var current))
            {
                return Task.FromResult(false);
            }

            var updated = Clone(machine);
            if (machines.TryUpdate(machine.Id, updated, current))
            {
                return Task.FromResult(true);
            }
        }
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(machines.TryRemove(id, out _));
    }

    private static Machine Clone(Machine machine) => new()
    {
        Id = machine.Id,
        Name = machine.Name,
        Status = machine.Status,
        LastHeartbeat = machine.LastHeartbeat,
        CurrentJob = machine.CurrentJob,
        Metrics = Clone(machine.Metrics),
        Metadata = machine.Metadata is null ? new Dictionary<string, string>() : new Dictionary<string, string>(machine.Metadata)
    };

    private static Metrics Clone(Metrics metrics) => new()
    {
        CpuUsage = metrics.CpuUsage,
        MemoryUsage = metrics.MemoryUsage,
        Temperature = metrics.Temperature
    };
}
