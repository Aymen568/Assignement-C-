namespace Assignement;

public interface IMachineService
{
    Task<Machine> CreateAsync(CreateMachineRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Machine>> GetAllAsync(CancellationToken cancellationToken);

    Task<Machine?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
