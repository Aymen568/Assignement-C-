namespace Assignement;

public sealed class MachineNotFoundException : Exception
{
    public MachineNotFoundException(Guid machineId)
        : base($"Machine '{machineId}' was not found.")
    {
        MachineId = machineId;
    }

    public Guid MachineId { get; }
}
