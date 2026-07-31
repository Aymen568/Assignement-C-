namespace Assignement;

public sealed class MachineService(IMachineRepository repository, IMachineEventPublisher eventPublisher) : IMachineService
{
    public async Task<Machine> CreateAsync(Machine machine, CancellationToken cancellationToken)
    {
        var created = await repository.CreateAsync(machine, cancellationToken);
        await eventPublisher.PublishMachineCreatedAsync(
            new MachineCreatedEvent(created.Id, created.Name, created.Status, DateTimeOffset.UtcNow),
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
                new MachineDeletedEvent(id, DateTimeOffset.UtcNow),
                cancellationToken);
        }

        return deleted;
    }
}
