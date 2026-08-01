using Assignement;
using Assignement.Services.Events;

namespace Assignement.Services.Machines;

public class MachineService(IMachineRepository repository, IMachineEventPublisher eventPublisher) : IMachineService
{
    public async Task<Machine> CreateAsync(CreateMachineRequest request, CancellationToken cancellationToken)
    {
        // Check for duplicate names (case-insensitive)
        var existing = await repository.GetAllAsync(cancellationToken);
        if (existing.Any(m => m.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException($"A machine with the name '{request.Name}' already exists.");
        }

        var machine = new Machine
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Metadata = request.Metadata ?? new(),
            Status = MachineStatus.Offline,  // always starts offline
            LastHeartbeat = default          // never received a heartbeat
        };

        var created = await repository.CreateAsync(machine, cancellationToken);
        await eventPublisher.PublishMachineCreatedAsync(
            new MachineCreated(created.Id, created.Name, created.Status, DateTimeOffset.UtcNow),
            cancellationToken);
        return created;
    }

    public Task<IReadOnlyCollection<Machine>> GetAllAsync(CancellationToken cancellationToken) =>
        repository.GetAllAsync(cancellationToken);

    public Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        repository.GetByIdAsync(id, cancellationToken);

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            await eventPublisher.PublishMachineDeletedAsync(
                new MachineDeleted(id, DateTimeOffset.UtcNow),
                cancellationToken);
        }

        return deleted;
    }
}
