namespace Assignement;

public interface IMachineRepository
{
    Task<Machine> CreateAsync(Machine machine, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Machine>> GetAllAsync(CancellationToken cancellationToken);

    Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> UpdateAsync(Machine machine, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
